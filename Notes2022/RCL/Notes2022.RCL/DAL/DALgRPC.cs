using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Notes2022.Server.Protos;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Components;
using Notes2022.Shared;

namespace Notes2022.RCL
{
    public class DALgRPC
    {

        public static  async Task<AboutModel> GetAbout(GrpcChannel Channel)
        {

            var client = new Notes2022gRPC.Notes2022gRPCClient(Channel);
            var xx = await (client.GetAboutAsync(new Empty())).ResponseAsync;

            AboutModel model = new AboutModel();
            model.PrimeAdminName = xx.About.PrimeAdminName;
            model.PrimeAdminEmail = xx.About.PrimeAdminEmail;
            model.StartupDateTime = xx.About.StartupDateTime.ToDateTime();

            return model;
        }

    }
}
