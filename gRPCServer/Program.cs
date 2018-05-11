using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Auth;
using Grpc.Core;
using Grpc.Core.Utils;
using GRPCDemo;

namespace gRPCServer
{
    class gRPCImpl : gRPC.gRPCBase
    {
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            Console.WriteLine($"request msg :: {request.Name}");

            Thread.Sleep(1000);

            return Task.FromResult(new HelloReply { Message = $"Hello {request.Name}" });
        }

        public override Task<HelloReply> Test(IAsyncStreamReader<HelloRequest> requestStream, ServerCallContext context)
        {
            var r = requestStream.ToListAsync().Result;

            foreach (var i in r)
            {
                Console.WriteLine($"request msg :: {i.Name}");
            }

            return Task.FromResult(new HelloReply { Message = $"Hello {requestStream.Current.Name}" });
        }
    }

    class Program
    {
        private const int Port = 9999;

        static void Main(string[] args)
        {
            Server server = new Server
            {
                Services = { gRPC.BindService(new gRPCImpl()) },
                Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
            };

            server.Start();

            Console.WriteLine($" grpc server listening on port :: {Port}");
            Console.ReadKey();

            server.ShutdownAsync().Wait();
        }
    }
}
