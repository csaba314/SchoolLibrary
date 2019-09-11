namespace Services.Models
{
    public interface IFileHelper
    {
        string ContentType { get; set; }
        byte[] FileContent { get; set; }
        string FileName { get; set; }
    }
}