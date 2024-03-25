using FileBytes.Service;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcFileService;
using Microsoft.Extensions.Logging;
using System.IO;

namespace FileBytesService
{
    public class GetFileClientservice : IGetFileClientService
    {
        private readonly ILogger<GetFileClientservice> logger;
        public GetFileClientservice(ILogger<GetFileClientservice> logger)
        {
            this.logger = logger;
        }
        public async Task<List<byte>> GetFileAsync(string fileName)
        {
            try
            {
                var receivedBytes = new List<byte>();
                var input = new GetFileBytesRequest { InputString  = fileName };
                var channel = GrpcChannel.ForAddress("https://localhost:7201/"); // should be read from Appsettings in future
                var client = new FileReaderService.FileReaderServiceClient(channel);
                using (var call = client.GetFileBytes(input))
                {
                   while(await call.ResponseStream.MoveNext())
                    { 
                        receivedBytes.AddRange(call.ResponseStream.Current.Data.ToByteArray());
                    }
                }
                return receivedBytes;
            }
            catch (Exception ex)
            {
                logger.LogError(1, ex, $"{nameof(GetFileClientservice)} : {nameof(GetFileAsync)} : {ex.Message}");
                return null;
            }
        }
    }
}
