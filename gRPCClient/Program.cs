using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Grpc.Auth;
using Grpc.Core;
using Grpc.Core.Utils;
using GRPCDemo;

namespace gRPCClient
{
    public class Client
    {
        private readonly gRPC.gRPCClient _gRpcClient;
        private AsyncDuplexStreamingCall<HelloRequest, HelloReply> _call;

        public Client()
        {
            Channel channel = new Channel("127.0.0.1", 9999, ChannelCredentials.Insecure);
            _gRpcClient = new gRPC.gRPCClient(channel);
            var withWaitForReady = new CallOptions().WithWaitForReady(true);
        }

        public async Task Received()
        {
            using (_call = _gRpcClient.Test2())
            {
                while (await _call.ResponseStream.MoveNext(CancellationToken.None))
                {
                    var msg = _call.ResponseStream.Current;
                    Console.WriteLine($"server Msg :: {msg.Message}");
                }
            }
        }

        public async Task Send()
        {
            if (_call != null)
            {
                for (int i = 0; i < 10; i++)
                {
                    var request = (new HelloRequest { Name = $" cash {i:00} " });
                    await _call.RequestStream.WriteAsync(request);
                }

                Console.WriteLine(" stream complete ");
            }
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new Client();
            //await client.Received();
            await client.Send();

            Console.ReadKey();
        }
    }
}
