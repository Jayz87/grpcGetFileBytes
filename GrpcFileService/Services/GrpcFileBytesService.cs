using Google.Protobuf;
using Grpc.Core;
using GrpcFileService;
using GrpcFileService.Backend;
using System.Formats.Tar;
using System.IO;

namespace GrpcFileService.Services
{
    public class GrpcFileBytesService : FileReaderService.FileReaderServiceBase
    {
        private readonly ILogger<GrpcFileBytesService> logger;
        private readonly IFileToBytes fileToBytesService;
        public GrpcFileBytesService(ILogger<GrpcFileBytesService> logger, IFileToBytes fileToBytesService)
        {
            this.logger = logger;
            this.fileToBytesService = fileToBytesService;
        }

        public override async Task GetFileBytes(GetFileBytesRequest request, IServerStreamWriter<GetFileBytesResponse> responseStream, ServerCallContext context)
        {
            if (!File.Exists(request.InputString))
                await responseStream.WriteAsync(null);
            byte[] fileBytes = fileToBytesService.ToByteArray(request.InputString);
            //await responseStream.WriteAsync(new GetFileBytesResponse { Data = ByteString.CopyFrom(fileBytes) });
            for (int i = 0; i < fileBytes.Length; i += 16)
            {
                int remainingBytes = Math.Min(16, fileBytes.Length - i);
                byte[] chunk = new byte[remainingBytes];
                Array.Copy(fileBytes, i, chunk, 0, remainingBytes);

                // Create a response with the chunk of data
                GetFileBytesResponse response = new GetFileBytesResponse
                {
                    Data = ByteString.CopyFrom(chunk)
                };

                // Write the response to the stream
                await responseStream.WriteAsync(response);
            }

        }
    }
}
