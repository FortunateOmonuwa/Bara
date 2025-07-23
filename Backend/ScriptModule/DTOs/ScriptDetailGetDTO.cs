namespace ScriptModule.DTOs
{
    public class ScriptDetailGetDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Logline { get; set; }
        public string Synopsis { get; set; }
        public decimal Price { get; set; }
        public string ScriptRegisterationStatus { get; set; }
        public string RegistrationBody { get; set; }
        public string Image { get; set; }
        public string CopyrightNumber { get; set; }
        public string OwnershipRight { get; set; }
        public string ScriptStatus { get; set; }
        public string Writer { get; set; }
        public string WriterName { get; set; }
        public string Path { get; set; }
        public string Url { get; set; }
    }
}
