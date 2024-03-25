namespace GrpcFileService.Backend
{
    public class FileToBytes : IFileToBytes
    {
        public byte[] ToByteArray(string fileName)
        {
            string fpath = fileName;
            return File.ReadAllBytes(fpath);
        }
    }
}
