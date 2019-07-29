using Jobs.Core.Common;
using System;
using System.IO;

namespace Jobs.Core.Application
{
    public class MessageService:IMessageService
    {
        [LogEverything]
        public void Receive(string message)
        {
            Console.WriteLine($"接收消息{message}");
            var fileUrl = $"D:\\HangfireLog\\Receive-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.txt";
            if (!File.Exists(fileUrl))
            {
                File.Create(fileUrl).Close();
            }
            FileStream fs = new FileStream(fileUrl, FileMode.Append);
            byte[] data = System.Text.Encoding.Default.GetBytes($"这是由Hangfire后台任务发送的消息:{message},时间为:{DateTime.Now}");
            fs.Write(data, 0, data.Length);
            fs.Close();
        }
        [LogEverything]
        public void Send(string message)
        {
            Console.WriteLine($"发送消息{message}");
            var fileUrl = $"D:\\HangfireLog\\Send-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.txt";
            if (!File.Exists(fileUrl))
            {
                File.Create(fileUrl).Close();
            }
            FileStream fs = new FileStream(fileUrl, FileMode.Append);
            byte[] data = System.Text.Encoding.Default.GetBytes($"这是由Hangfire后台任务发送的消息:{message},时间为:{DateTime.Now}");
            fs.Write(data, 0, data.Length);
            fs.Close();
        }
    }
}
