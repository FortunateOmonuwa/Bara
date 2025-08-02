namespace ScriptModule.DTOs
{
    public class GetScriptDTO
    {
        public string Name { get; set; }
        public string ContentType { get; set; }
        public byte[] File { get; set; }
    }
}
