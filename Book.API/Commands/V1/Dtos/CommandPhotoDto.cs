namespace Book.API.Commands.V1.Dtos
{
    public class CommandPhotoDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }
    }
}