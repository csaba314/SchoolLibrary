using System.Web;
using Services.Models;

namespace Services.Services
{
    public interface IFileServices
    {
        IFileModel Get(int id);
        int CheckDbAndUploadeFile(HttpPostedFileBase uploadedFile);
        IFileHelper GetFile(int id);
    }
}