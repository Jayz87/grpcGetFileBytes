namespace GrpcFileService.Backend
{
    public interface IFileToBytes
    {
        byte[] ToByteArray(string fileName);
    }
}
