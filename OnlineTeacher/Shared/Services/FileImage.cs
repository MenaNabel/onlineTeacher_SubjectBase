
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using OnlineTeacher.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.Shared.Services
{
   
    public class FileImage : IFileImageUploading
    {
         enum Format { photo, file };

        private Dictionary<Format,List<string>> AcceptedExtentions = new Dictionary<Format, List<string>>
        {
            {   Format.photo ,new List<string>{ ".PNG",
                                                ".JPEG",
                                                ".JPG",
                                              }
            },
            {   Format.file ,new List<string>{  ".DOCX",
                                                ".XLSX",
                                                ".TXT",
                                                ".CSV",
                                                ".PDF",
                                             }
            }

        };
       // private readonly IWebHostEnvironment _AppEnvironment;

        public FileImage()//IWebHostEnvironment AppEnvironment)
        {
          //  _AppEnvironment = AppEnvironment;
        }

        private static string ConvertPhotoToBase64InString(byte[] arrayImage , string extention)
        {

            string base64String = Convert.ToBase64String(arrayImage, 0, arrayImage.Length);

            return $"data:image/{extention};base64," + base64String;

        } 
      

        public bool UploadPhoto(IFileImage FileOrImage, out string FileOrImagePath)
        {

            if (FileOrImage?.ImageOrFile?.FileName != null)
            {
                var extention = Path.GetExtension(FileOrImage.ImageOrFile.FileName).ToUpper();
                if (AcceptedExtentions[Format.photo].Contains(extention))
                {

                    Stream PhotoStream = FileOrImage.ImageOrFile.OpenReadStream();
                    BinaryReader binaryReader = new BinaryReader(PhotoStream);
                    var BinaryPhoto = binaryReader.ReadBytes((Int32)PhotoStream.Length);
                    FileOrImagePath = ConvertPhotoToBase64InString(BinaryPhoto, extention);
                    return true;
                }
            }
            FileOrImagePath = default;
            return false;



            //string uniqueFileName = null;

            //try
            //{
            //    if (FileOrImage.Image != null && FileOrImage.Image.Length > 0)
            //    {
            //        var fileExt = Path.GetExtension(FileOrImage.Image.FileName);
            //        if (fileExt.ToLower().EndsWith(".png") || fileExt.ToLower().EndsWith(".jpg"))
            //        {
            //            string fileName = Path.GetFileName(FileOrImage.Image.FileName);
            //            string ServerPath = Path.Combine(_AppEnvironment.WebRootPath + "Uploades");

            //            uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;
            //            string imgPath = Path.Combine(ServerPath, uniqueFileName);
            //            FileOrImagePath = "/Uploades/" + uniqueFileName;
            //            if (!Directory.Exists(ServerPath))
            //            {
            //                Directory.CreateDirectory(ServerPath);
            //            }
            //            var fileStream = new FileStream(imgPath, FileMode.Create);
            //            FileOrImage.Image.CopyTo(fileStream);
            //            return true;
            //        }
            //    }
            //    FileOrImagePath = default;
            //    return false;
            //}
            //catch (Exception e)
            //{
            //    throw e;
            //}

        }
    
        public bool UploadFile(IFileImage File, out string FileName ,out byte[] FileData )
        {

            if (File?.ImageOrFile?.FileName != null)
            {
                var extention = Path.GetExtension(File.ImageOrFile.FileName).ToUpper();
               
                if (AcceptedExtentions[Format.file].Contains(extention))
                {
                   
                    Stream FileStram = File.ImageOrFile.OpenReadStream();
                    BinaryReader binaryReader = new BinaryReader(FileStram);
                    var BinaryFile = binaryReader.ReadBytes((Int32)FileStram.Length);

                    FileName = File.ImageOrFile.FileName;
                    FileData = BinaryFile;
                    return true;
                }

            }
            FileName = default;
            FileData = default;
            return false;
        }
    }
}
