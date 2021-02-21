using System.Collections.Generic;
using System.Linq;
using Messages;

namespace DockerComposeLocalStack.DataAccess
{
    public class GetMessageCommand : IGetMessagesCommand
    {
        private readonly MessagesDbContext _context;

        public GetMessageCommand(MessagesDbContext context)
        {
            _context = context;
        }

        public IEnumerable<string> GetMessages() => _context.Messages.Select(m => m.Value).ToArray();

        public void Dispose()
        {
        }
    }
}