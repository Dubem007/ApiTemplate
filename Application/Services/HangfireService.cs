using Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class HangFireService
{
    public static void RunJobs()
    {
        //run cron job every 5hours
        //RecurringJob.AddOrUpdate<PayrollRunService>(x => x.InitializePayrollRun(), "0 0 17 * *");
    }
}

