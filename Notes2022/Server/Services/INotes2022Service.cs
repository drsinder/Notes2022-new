

using Notes2022.Shared;
using System.ServiceModel;

namespace Notes2022.Server.Services
{

    [ServiceContract]
    public interface INotes2022Service
    {

        [OperationContract]
        Task<AboutModel> GetAbout();

    }
}
