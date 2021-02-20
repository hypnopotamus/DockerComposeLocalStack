using System;
using System.Collections.Generic;

namespace DockerComposeLocalStack.DataAccess
{
    public interface IGetMessagesCommand : IDisposable
    {
        IEnumerable<string> GetMessages();
    }
}