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
    class Program
    {
        static void Main(string[] args)
        {
            Channel channel = new Channel("127.0.0.1", 9999, ChannelCredentials.Insecure);

            var client = new gRPC.gRPCClient(channel);

            var withWaitForReady = new CallOptions().WithWaitForReady(true);
            //var reply = client.SayHello(new HelloRequest { Name = "cash" }, withWaitForReady);

            //Console.WriteLine($" reply :: {reply.Message}");

            while (true)
            {
                var call = client.Test(withWaitForReady);

                var request = new List<HelloRequest>(); 
                for (int i = 0; i < 10; i++)
                {
                    request.Add(new HelloRequest {Name = $" cash {i:00} "});
                }

                call.RequestStream.WriteAllAsync(request).Wait();
                Console.WriteLine(" stream complete ");

                Thread.Sleep(2000);
            }
        }
    }
}
