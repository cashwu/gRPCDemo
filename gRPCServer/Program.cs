using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using GRPCDemo;

namespace gRPCServer
{
    class gRPCImpl : gRPC.gRPCBase
    {
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            Console.WriteLine($"request msg :: {request.Name}");
            return Task.FromResult(new HelloReply { Message = $"Hello {request.Name}" });
        }
    }

    class Program
    {
        private const int Port = 9999;

        static void Main(string[] args)
        {
            Server server = new Server
            {
                Services = {gRPC.BindService(new gRPCImpl())},
                Ports = {new ServerPort("localhost", Port, ServerCredentials.Insecure)}
            };

            server.Start();

            Console.WriteLine($" grpc server listening on port :: {Port}");
            Console.ReadKey();

            server.ShutdownAsync().Wait();
        }
    }
}
