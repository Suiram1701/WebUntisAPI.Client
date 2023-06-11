using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebUntisAPI.Client.Models;

namespace WebUntisAPI.Client
{
    public partial class WebUntisClient
    {
        public async Task<Timegrid> GetTimegridAsync(string id = "getTimegrid", CancellationToken ct = default)
        {
            Timegrid timeGrid = await MakeRequestAsync<object, Timegrid>(id, "getTimegridUnits", new object(), ct);
            return timeGrid;
        }
    }
}
