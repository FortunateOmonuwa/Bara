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
                var writer = await dbContext.Writers.FindAsync(writerId);
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
                    Title = scriptDetails.Title,
                    WriterId = writerId,
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
                logger.LogError($"An exception {ex.InnerException} was thrown while adding a script", ex.Message);
                return ResponseDetail<Script>.Failed(ex.Message, ex.HResult, "Caught Exception");
            }
        }

        public Task<ResponseDetail<bool>> DeleteScript(Guid scriptId, Guid? writerId)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDetail<Script>> GetScriptById(Guid scriptId, Guid? writerId)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDetail<List<Script>>> GetScripts(int pageNumber, int pageSize)
        {
            try
            {
                var scriptsCache = !memoryCache.TryGetValue<List<Script>>(ALL_SCRIPTS_CACHE_KEY, out var allScripts);
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
                logger.LogError($"Error: {ex.InnerException}", ex.Message);
                return ResponseDetail<List<Script>>.Failed(ex.Message, ex.HResult, "Caught Exception");
            }
        }

        public async Task<ResponseDetail<List<Script>>> GetScriptsByWriterId(Guid writerId, int pageNumber, int pageSize)
        {
            try
            {
                var cacheKey = $"Writer_{writerId}'s_Scripts";
                var cache = memoryCache.TryGetValue<List<Script>>(cacheKey, out var cachedScripts);
                if (cachedScripts is null)
                {
                    cachedScripts = await dbContext.Scripts.Where(x => x.WriterId == writerId).OrderByDescending(x => x.CreatedAt).ToListAsync();
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
                logger.LogError($"An exception {ex.InnerException} was thrown while retrieving scripts for writer: {writerId}", ex.Message);
                return ResponseDetail<List<Script>>.Failed(ex.Message, ex.HResult, "Caught Exception");
            }
        }

        public Task<ResponseDetail<byte[]>> GetScriptFile(Guid scriptId)
        {
            throw new NotImplementedException();
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
