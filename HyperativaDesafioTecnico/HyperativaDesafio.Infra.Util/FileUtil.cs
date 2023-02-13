using Microsoft.AspNetCore.Http;
using System.IO;

namespace HyperativaDesafio.Infra.Util
{
    public static class FileUtil
    {
        public static byte[] ConvertFileInByteArray(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public static string SaveFile(IFormFile file, string path)
        {

            string fullPath = path + "\\" + file.FileName;

            try
            {
                byte[] bytesFile = ConvertFileInByteArray(file);
                System.IO.File.WriteAllBytesAsync(fullPath, bytesFile);


            }
            catch (Exception)
            {

                fullPath = "";
            }

            return fullPath;
        }


    }
}