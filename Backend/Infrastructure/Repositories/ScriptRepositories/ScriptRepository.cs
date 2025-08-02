using Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ScriptModule.DTOs;
using ScriptModule.Enums;
using ScriptModule.Interfaces;
using ScriptModule.Models;
using Services.FileStorageServices.Interfaces;
using SharedModule.Settings;
using SharedModule.Utils;

namespace Infrastructure.Repositories.ScriptRepositories
{
    public class ScriptRepository : IScriptService
    {
        private readonly IFileStorageService cloudinary;
        private readonly ILogger<ScriptRepository> logger;
        private readonly BaraContext dbContext;
        private readonly AppSettings settings;
        private readonly Secrets secrets;
        private readonly IMemoryCache memoryCache;

        private const string ALL_SCRIPTS_CACHE_KEY = "All_Scripts_Cache";
        public ScriptRepository(IFileStorageService fileStorageService, ILogger<ScriptRepository> logger, BaraContext baraContext, IOptions<Secrets> secrets, IOptions<AppSettings> appSettings, IMemoryCache memoryCache)
        {
            cloudinary = fileStorageService;
            this.logger = logger;
            dbContext = baraContext;
            this.secrets = secrets.Value;
            this.settings = appSettings.Value;
            this.memoryCache = memoryCache;
        }
        public async Task<ResponseDetail<Script>> AddScript(PostScriptDetailDTO scriptDetails, Guid writerId)
        {
            try
            {
                var writer = await dbContext.Writers.Select(x => new { x.Id, x.FirstName, x.LastName }).FirstOrDefaultAsync(x => x.Id == writerId);
                if (writer is null)
                {
                    return ResponseDetail<Script>.Failed($"Writer with profileId {writerId} does not exist");
                }

                var script = scriptDetails.File;
                var sizeLimit = 10 * 1024 * 1024;
                var scriptName = script.FileName.ToUpper().Trim();
                var scriptExtension = Path.GetExtension(scriptName).ToLower();

                var allowedExtensions = new[] { ".pdf", ".doc", ".docx" };
                var allowedMimeTypes = new[]
                {
                    "application/msword",
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    "application/pdf"
                };

                var scriptFormatIsAcceptable = allowedExtensions.Contains(scriptExtension) && allowedMimeTypes.Contains(script.ContentType);
                if (!scriptFormatIsAcceptable)
                {
                    return ResponseDetail<Script>.Failed($"An invalid script format was uploaded. The allowed extensions are: " +
                                                                    $"{string.Join(", ", allowedExtensions)}...\n" +
                                                                    $"Allowed mime types are: {string.Join(", ", allowedMimeTypes)}", 415, "Invalid File Types");
                }

                var scriptExceedsLimit = script.Length > sizeLimit;
                if (scriptExceedsLimit)
                {
                    return ResponseDetail<Script>.Failed($"Script exceeds limit of {sizeLimit}", 413, "File Limit Exceeded");
                }

                var userDirectoryName = $"{writer.FirstName}_{writer.LastName}-{writerId}";
                var uploadScriptResponse = await cloudinary.UploadScriptAsync(userDirectoryName, script);
                if (!uploadScriptResponse)
                {
                    logger.LogError($"An error occured while uploading the script titled {scriptDetails.Title} for {writer.FirstName} {writer.LastName}");
                    return ResponseDetail<Script>.Failed($"An error occured while uploading the script", 500, "Umexpected Error");
                }

                var newScriptDetail = new Script
                {
                    Genre = scriptDetails.Genre,
                    Logline = scriptDetails.Logline,
                    OwnershipRights = scriptDetails.OwnershipRights,
                    Image = scriptDetails.Image,
                    IsScriptRegistered = scriptDetails.IsScriptRegistered,
                    Price = scriptDetails.Price,
                    ProofUrl = scriptDetails.ProofUrl,
                    RegistrationBody = scriptDetails.RegistrationBody,
                    Synopsis = scriptDetails.Synopsis,
                    Title = scriptDetails.Title.ToUpper(),
                    WriterId = writerId,
                    WriterName = $"{writer.FirstName}-{writer.LastName}",
                    Path = $"{secrets.CloudinaryFolderName}/{userDirectoryName}/scripts/{scriptName}",
                    Url = $"{settings.CloudinaryBaseURL}/{secrets.CloudinaryFolderName}/{userDirectoryName}/scripts/{scriptName}"
                };

                await dbContext.Scripts.AddAsync(newScriptDetail);
                await dbContext.SaveChangesAsync();

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(10),
                    SlidingExpiration = TimeSpan.FromMinutes(5),
                };

                var allScriptsCache = memoryCache.TryGetValue<List<Script>>(ALL_SCRIPTS_CACHE_KEY, out var allScripts);

                if (allScripts is not null)
                {
                    allScripts.Add(newScriptDetail);
                }
                else
                {
                    memoryCache.Set(ALL_SCRIPTS_CACHE_KEY, new List<Script> { newScriptDetail }, cacheEntryOptions);
                }

                var writerCacheKey = $"Writer_{writerId}_Scripts";
                var writerScriptsCache = memoryCache.TryGetValue<List<Script>>(writerCacheKey, out var writerScripts);
                if (writerScripts is not null)
                {
                    writerScripts.Add(newScriptDetail);
                }
                else
                {
                    memoryCache.Set(writerCacheKey, new List<Script> { newScriptDetail }, cacheEntryOptions);
                }

                return ResponseDetail<Script>.Successful(newScriptDetail, "Script added successfully", 201);
            }
            catch (Exception ex)
            {
                logger.LogError($"An exception was thrown while adding a script, \nException: {ex.GetType().Name}\n Base Exception: {ex.GetBaseException().GetType().Name}", $"Exception Code: {ex.HResult}");
                return ResponseDetail<Script>.Failed("Your request cannot be completed at this time... Please try again later", 500, "Unexpected error");
            }
        }

        public async Task<ResponseDetail<bool>> DeleteScript(Guid scriptId, Guid writerId)
        {
            try
            {
                var writerCacheKey = $"Writer_{writerId}_Scripts";
                var writerScriptsCache = memoryCache.TryGetValue<List<Script>>(writerCacheKey, out var writerScripts);
                if (writerScriptsCache && writerScripts != null)
                {
                    var scriptInWriterCache = writerScripts.FirstOrDefault(x => x.Id == scriptId);
                    if (scriptInWriterCache != null)
                        writerScripts.Remove(scriptInWriterCache);
                }

                var allcriptsCache = memoryCache.TryGetValue<List<Script>>(ALL_SCRIPTS_CACHE_KEY, out var allScripts);
                if (allcriptsCache && allScripts != null)
                {
                    var scriptInAll = allScripts.FirstOrDefault(x => x.Id == scriptId);
                    if (scriptInAll != null)
                        allScripts.Remove(scriptInAll);
                }

                var script = await dbContext.Scripts.FindAsync(scriptId);
                if (script is null)
                {
                    return ResponseDetail<bool>.Failed("Invalid script Id");
                }
                if (script.Status == ScriptStatus.Deleted)
                {
                    return ResponseDetail<bool>.Failed("Script already deleted");
                }
                var deleteFromStorage = await cloudinary.DeleteAsync(script.Path);
                if (!deleteFromStorage)
                {
                    return ResponseDetail<bool>.Failed("An error occurred while deleting the script", 500, "Unexpected Error");
                }

                script.Status = ScriptStatus.Deleted;
                await dbContext.SaveChangesAsync();

                return ResponseDetail<bool>.Successful(true, "Script deleted successfully");
            }
            catch (Exception ex)
            {
                logger.LogError($"An exception was thrown while deleting script. \nException: {ex.GetType().Name}\n Base Exception: {ex.GetBaseException().GetType().Name}", $"Exception Code: {ex.HResult}");
                return ResponseDetail<bool>.Failed("Your request cannot be completed at this time... Please try again later", 500, "Unexpected error");
            }
        }

        public async Task<ResponseDetail<Script>> GetScriptById(Guid scriptId, Guid? writerId)
        {
            try
            {
                Script script;

                if (writerId.HasValue)
                {
                    var writerCacheKey = $"Writer_{writerId}_Scripts";
                    memoryCache.TryGetValue<List<Script>>(writerCacheKey, out var writerScriptsList);
                    if (writerScriptsList is not null)
                    {
                        script = writerScriptsList.FirstOrDefault(x => x.Id == scriptId);
                        if (script is not null)
                        {
                            return ResponseDetail<Script>.Successful(script);
                        }
                    }

                    script = await dbContext.Scripts.FirstOrDefaultAsync(x => x.Id == scriptId && x.WriterId == writerId);
                }
                else
                {
                    script = await dbContext.Scripts.FindAsync(scriptId);
                }

                if (script == null)
                {
                    return ResponseDetail<Script>.Failed($"Script not found", 404, "Not Found");
                }

                return ResponseDetail<Script>.Successful(script);
            }
            catch (Exception ex)
            {
                logger.LogError($"An exception was thrown while script... \nException: {ex.GetType().Name}\n Base Exception: {ex.GetBaseException().GetType().Name}", $"Exception Code: {ex.HResult}");
                return ResponseDetail<Script>.Failed("Your request cannot be completed at this time... Please try again later", 500, "Unexpected error");
            }
        }

        public async Task<ResponseDetail<List<Script>>> GetScripts(int pageNumber, int pageSize)
        {
            try
            {
                memoryCache.TryGetValue<List<Script>>(ALL_SCRIPTS_CACHE_KEY, out var allScripts);
                if (allScripts == null)
                {
                    var writer = await dbContext.Writers.Select(x => new
                    {
                        PremiumStatus = x.IsPremiumMember,
                        x.Scripts,
                        x.CreatedAt
                    }).ToListAsync();

                    allScripts = writer
                                .OrderByDescending(x => x.PremiumStatus)
                                .ThenByDescending(x => x.CreatedAt)
                                .SelectMany(x => x.Scripts.Where(x => x.Status == ScriptStatus.Available))
                                .ToList();

                    var cacheOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                    memoryCache.Set(ALL_SCRIPTS_CACHE_KEY, allScripts, cacheOptions);
                }

                var totalCount = allScripts.Count;
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                var paginatedScripts = allScripts
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                if (totalCount < 1)
                {
                    return ResponseDetail<List<Script>>.SuccessfulPaginatedResponse(paginatedScripts, totalCount, totalPages, pageNumber, "No available script(s)", 204);
                }

                return ResponseDetail<List<Script>>.SuccessfulPaginatedResponse(paginatedScripts, totalCount, totalPages, pageNumber);
            }
            catch (Exception ex)
            {
                logger.LogError($"An exception {ex.GetType().Name} while fetching scripts...\n Base Exception{ex.GetBaseException().GetType().Name}", $"Exception Code: {ex.HResult}");
                return ResponseDetail<List<Script>>.Failed("Your request cannot be completed at this time... Please try again later", 500, "Unexpected error");
            }
        }

        public async Task<ResponseDetail<List<Script>>> GetScriptsByWriterId(Guid writerId, int pageNumber, int pageSize)
        {
            try
            {
                var cacheKey = $"Writer_{writerId}'s_Scripts";
                memoryCache.TryGetValue<List<Script>>(cacheKey, out var cachedScripts);
                if (cachedScripts is null)
                {
                    cachedScripts = await dbContext.Scripts
                                                    .Where(x => x.WriterId == writerId && x.Status != ScriptStatus.Deleted)
                                                    .OrderByDescending(x => x.CreatedAt)
                                                    .ToListAsync();
                    var cacheOptions = new MemoryCacheEntryOptions()
                       .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
                       .SetSlidingExpiration(TimeSpan.FromMinutes(5));
                    memoryCache.Set(cacheKey, cachedScripts, cacheOptions);
                }

                if (cachedScripts is null)
                {
                    return ResponseDetail<List<Script>>.Failed($"Writer with Id: {writerId} does not exist", 400, "Invalid User");
                }

                var totalCount = cachedScripts.Count;
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                var paginatedResult = cachedScripts
                                        .Skip((pageNumber - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToList();

                if (totalCount < 1)
                {
                    return ResponseDetail<List<Script>>.SuccessfulPaginatedResponse(paginatedResult, totalCount, totalPages, pageNumber, "You have no script(s)", 204);
                }

                return ResponseDetail<List<Script>>.SuccessfulPaginatedResponse(paginatedResult, totalCount, totalPages, pageNumber, "Scripts retrieved successfully");
            }
            catch (Exception ex)
            {
                logger.LogError($"An exception {ex.InnerException} was thrown while retrieving scripts for writer: {writerId}... \nBase Exception{ex.GetBaseException().GetType().Name}", $"Exception Code: {ex.HResult}");
                return ResponseDetail<List<Script>>.Failed("Your request cannot be completed at this time... Please try again later", 500, "Unexpected error");
            }
        }

        public async Task<ResponseDetail<GetScriptDTO>> DownloadScript(Guid scriptId)
        {
            try
            {
                var script = await dbContext.Scripts.FindAsync(scriptId);
                if (script == null)
                {
                    return ResponseDetail<GetScriptDTO>.Failed($"Script with id {scriptId} doesn't exist", 404, "Not Found");
                }

                var (stream, contentType) = await cloudinary.DownloadAsync(script.Path);
                var fileBytes = stream.ToArray();

                return ResponseDetail<GetScriptDTO>.Successful(new GetScriptDTO
                {
                    ContentType = contentType,
                    File = fileBytes,
                    Name = script.Title
                });
            }
            catch (Exception ex)
            {
                logger.LogError($"An exception {ex.GetType().Name} was thrown while downloading script with ID: {scriptId}... Base Exception {ex.GetBaseException().GetType().Name}", ex.Message);
                return ResponseDetail<GetScriptDTO>.Failed("Your request cannot be completed at this time... Please try again later", 500, "Unexpected error");
            }
        }

        public Task<ResponseDetail<Script>> UpdateScript(PostScriptDetailDTO scriptDetails, Guid writerId, Guid scriptId)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDetail<Script>> UpdateScriptStatus(ScriptStatus status, Guid scriptId, Guid writerId)
        {
            throw new NotImplementedException();
        }
    }
}
