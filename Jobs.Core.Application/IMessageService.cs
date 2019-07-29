namespace Jobs.Core.Application
{
    public interface IMessageService
    {
        void Receive(string message);

        void Send(string message);
    }
}