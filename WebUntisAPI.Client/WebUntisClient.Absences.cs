using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebUntisAPI.Client.Models.Absence;
using WebUntisAPI.Client.Models.Interfaces;
namespace WebUntisAPI.Client
{
    partial class WebUntisClient
    {
        /// <summary>
        /// Gets the absences for the specified timespan.
        /// </summary>
        /// <param name="startDate">The start date of the absence period, formatted as yyyyMMdd.</param>
        /// <param name="endDate">The end date of the absence period, formatted as yyyyMMdd.</param>
        /// <param name="user">Person which absence is requested</param>
        /// <param name="excuseStatusId">The ID representing the status of the excuse. Default is -1, which indicates no specific filter for the excuse status.</param>
        /// <param name="ct">A <see cref="CancellationToken"/> that can be used to signal cancellation of the operation.</param>
        /// <returns>
        /// A <see cref="Task{AbsenceData}"/> representing the asynchronous operation. The task result contains an <see cref="AbsenceData"/> object,
        /// which holds the list of absences for the specified student within the provided date range.
        /// </returns>
        public async Task<AbsenceData> GetAbsencesAsync(DateOnly startDate, DateOnly endDate,IUser user, int excuseStatusId = -1, CancellationToken ct = default)
        {
            ThrowWhenNotAvailable();
            UriBuilder uriBuilder = new()
            {
                Scheme = Uri.UriSchemeHttps,
                Host = ServerName,
                Path = "/WebUntis/api/classreg/absences/students",
                Query = $"startDate={startDate:yyyyMMdd}&endDate={endDate:yyyyMMdd}&studentId={user.Id}&excuseStatusId={excuseStatusId}"
            };
            string responseString = await InternalApiRequestAsync(uriBuilder.Uri, ct);
            return JObject.Parse(responseString)["data"]!.ToObject<AbsenceData>()!;

        }
    }
}
