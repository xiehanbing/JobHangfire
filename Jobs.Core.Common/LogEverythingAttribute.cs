using System;
using System.IO;
using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Logging;
using Hangfire.Server;
using Hangfire.States;
using Hangfire.Storage;
using Newtonsoft.Json;

namespace Jobs.Core.Common
{
    public class LogEverythingAttribute : JobFilterAttribute, IClientFilter, IServerFilter, IElectStateFilter, IApplyStateFilter
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
        public void OnCreating(CreatingContext filterContext)
        {
            Logger.InfoFormat("Creating a job based on method `{0}`...", filterContext.Job.Method.Name);
            WriteLog(
                $"{ApiConfig.HangfireLogUrl}\\OnCreating-{(filterContext.Job.Method.Name)}-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.txt",
                $"Creating a job based on method `{filterContext.Job.Method.Name}`... . 时间为:{DateTime.Now:yyyy-MM-dd-HH-mm-ss}");
        }

        public void OnCreated(CreatedContext filterContext)
        {
            WriteLog(
                $"{ApiConfig.HangfireLogUrl}\\OnCreated-{(filterContext.BackgroundJob?.Id ?? "0")}-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.txt", 
                $"Job that is based on method `{filterContext.Job.Method.Name}` has been created with id `{filterContext.BackgroundJob?.Id}` . 时间为:{DateTime.Now:yyyy-MM-dd-HH-mm-ss} \r\n");
            Logger.InfoFormat(
                "Job that is based on method `{0}` has been created with id `{1}`",
                filterContext.Job.Method.Name,
                filterContext.BackgroundJob?.Id);
        }

        public void OnPerforming(PerformingContext filterContext)
        {
            Logger.InfoFormat("Starting to perform job `{0}`", filterContext.BackgroundJob.Id);
            WriteLog(
                $"{ApiConfig.HangfireLogUrl}\\OnPerforming-{(filterContext.BackgroundJob?.Id ?? "0")}-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.txt",
                $"Starting to perform job `{filterContext.BackgroundJob.Id}` . 时间为:{DateTime.Now:yyyy-MM-dd-HH-mm-ss} \r\n");
        }

        public void OnPerformed(PerformedContext filterContext)
        {
            Logger.InfoFormat("Job `{0}` has been performed", filterContext.BackgroundJob.Id);
            WriteLog(
                $"{ApiConfig.HangfireLogUrl}\\OnPerformed-{(filterContext.BackgroundJob?.Id ?? "0")}-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.txt",
                $"Job `{ filterContext.BackgroundJob.Id}` has been performed . 时间为:{DateTime.Now:yyyy-MM-dd-HH-mm-ss} \r\n");
        }

        public void OnStateElection(ElectStateContext context)
        {
            var failedState = context.CandidateState as FailedState;
            if (failedState != null)
            {
                Logger.WarnFormat(
                    "Job `{0}` has been failed due to an exception `{1}`",
                    context.BackgroundJob.Id,
                    failedState.Exception);
                WriteLog(
                    $"{ApiConfig.HangfireLogUrl}\\OnStateElection-{(context.BackgroundJob?.Id ?? "0")}-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.txt",
                    $"Job `{context.BackgroundJob.Id}` has been failed due to an exception `{(JsonConvert.SerializeObject(failedState.Exception))}` . 时间为:{DateTime.Now:yyyy-MM-dd-HH-mm-ss} \r\n");
            }
        }

        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            Logger.InfoFormat(
                "Job `{0}` state was changed from `{1}` to `{2}`",
                context.BackgroundJob.Id,
                context.OldStateName,
                context.NewState.Name);
            WriteLog(
                $"{ApiConfig.HangfireLogUrl}\\OnStateApplied-{(context.BackgroundJob?.Id ?? "0")}-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.txt",
                $"Job `{context.BackgroundJob.Id}` state was changed from `{context.OldStateName}` to `{context.NewState.Name}` . 时间为:{DateTime.Now:yyyy-MM-dd-HH-mm-ss} \r\n");
        }

        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            Logger.InfoFormat(
                "Job `{0}` state `{1}` was unapplied.",
                context.BackgroundJob.Id,
                context.OldStateName);
            WriteLog(
                $"{ApiConfig.HangfireLogUrl}\\OnStateUnapplied-{(context.BackgroundJob?.Id ?? "0")}-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.txt",
                $"Job `{context.BackgroundJob.Id}` state `{context.OldStateName}` was unapplied . 时间为:{DateTime.Now:yyyy-MM-dd-HH-mm-ss} \r\n");
        }
        /// <summary>
        /// 写入文本
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="message"></param>
        public void WriteLog(string path,string message)
        {
            var fileUrl = path;
            if (!File.Exists(fileUrl))
            {
                File.Create(fileUrl).Close();
            }
            FileStream fs = new FileStream(fileUrl, FileMode.Append);
            byte[] data = System.Text.Encoding.Default.GetBytes(message??"");
            fs.Write(data, 0, data.Length);
            fs.Close();
        }
    }
}