using Services.Models;
using Services.UnitOfWork;
using System;
using System.Linq;
using System.Web;

namespace Services.Services
{
    public class FileServices : IFileServices
    {
        private IUnitOfWork _unitOfWork;

        public FileServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IFileModel Get(int id)
        {
            return _unitOfWork.Get<FileModel>(id);
        }

        public int CheckDbAndUploadeFile(HttpPostedFileBase uploadedFile)
        {
            byte[] fileByteArray = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(fileByteArray, 0, fileByteArray.Length);

            string fileName = uploadedFile.FileName;
            string fileType = uploadedFile.ContentType;
            string fileContent = Convert.ToBase64String(fileByteArray);

            FileModel fileFromDb = _unitOfWork.GetAll<FileModel>().SingleOrDefault(f => f.FileContent.Equals(fileContent));

            if (fileFromDb == null)
            {
                DateTime dateTimeToInsert = DateTime.Now;

                _unitOfWork.Add(new FileModel
                {
                    FileName = fileName,
                    FileType = fileType,
                    FileContent = fileContent,
                    DateAdded = dateTimeToInsert
                });
                _unitOfWork.Commit();

                return _unitOfWork.GetAll<FileModel>().FirstOrDefault(f => f.FileContent.Equals(fileContent)).FileModelId;

            }
            return fileFromDb.FileModelId;
        }

        public IFileHelper GetFile(int id)
        {
            var fileInDb = _unitOfWork.Get<FileModel>(id);

            if (fileInDb != null)
            {
                try
                {
                    byte[] byteContent = Convert.FromBase64String(fileInDb.FileContent);
                    return new FileHelper(byteContent, fileInDb.FileType);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return null;
        }
    }
}
