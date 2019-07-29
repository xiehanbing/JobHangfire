using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Hangfire;
using Hangfire.Annotations;
using Hangfire.RecurringJobExtensions;
using Hangfire.RecurringJobExtensions.Configuration;
using Jobs.Core.Common.Extension;

namespace Jobs.Core.Common
{
    public class JobProvider : IConfigurationProvider
    {
        public IEnumerable<RecurringJobInfo> Load()
        {
            var result = new List<RecurringJobInfo>();
            var assembly = Assembly.GetEntryAssembly();
            if (assembly == null) return result;
            foreach (var type in assembly.GetTypes())
            {
                foreach (var method in type.GetTypeInfo().DeclaredMethods)
                {
                    //if(!method.IsDefined(typeof(RecurringJobInfo),false))
                    //    continue;
                    var attribute = method.GetCustomAttribute<RecurringJobAttribute>(false);
                    if (attribute == null) continue;
                    if (string.IsNullOrWhiteSpace(attribute.RecurringJobId))
                    {
                        attribute.RecurringJobId =method.GetRecurringJobId();
                    }
                    result.Add(new RecurringJobInfo()
                    {
                        RecurringJobId = attribute.RecurringJobId,
                        Cron = attribute.Cron,
                        Queue = attribute.Queue,
                        Enable = attribute.Enabled,
                        Method = method,
                        TimeZone = TimeZoneInfo.Local
                    });
                }
            }

            return result;
        }
       
    }


   
}