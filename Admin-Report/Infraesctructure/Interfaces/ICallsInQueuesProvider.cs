

using Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infraesctructure.Interfaces
{
    public  interface ICallsInQueuesProvider
    {
        Task<List<CallsInQueues>> FetchAllAsync(string queryParams = "");
        Task<List<Queue>> FetchQueuesAsync();

        (bool, string) TestConnection();
    }
}
