using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzFunctions_AnimeToNotion
{
    public class UpdateMALRatingsFunction
    {
        [FunctionName("UpdateMALRatingsFunction")]
        public void Run([TimerTrigger("0 1 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
