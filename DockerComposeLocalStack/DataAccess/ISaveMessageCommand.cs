using System;

namespace DockerComposeLocalStack.DataAccess
{
    public interface ISaveMessageCommand : IDisposable
    {
        void Save(string message);
    }
}