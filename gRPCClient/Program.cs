using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Grpc.Auth;
using Grpc.Core;
using GRPCDemo;

namespace gRPCClient
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Channel channel = new Channel("127.0.0.1", 9999, ChannelCredentials.Insecure);

                var client = new gRPC.gRPCClient(channel);

                var withWaitForReady = new CallOptions().WithWaitForReady(true);
                var reply = client.SayHello(new HelloRequest { Name = "cash" }, withWaitForReady);

                Console.WriteLine($" reply :: {reply.Message}");

                Thread.Sleep(1000);
            }
        }
    }
}
