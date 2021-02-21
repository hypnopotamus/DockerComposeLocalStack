using Messages;

namespace DockerComposeLocalStack.DataAccess
{
    public class SaveMessageCommand : ISaveMessageCommand
    {
        private readonly MessagesDbContext _context;

        public SaveMessageCommand(MessagesDbContext context)
        {
            _context = context;
        }

        public void Save(string message)
        {
            _context.Messages.Add(new Message(message));
            _context.SaveChanges();
        }

        public void Dispose()
        {
        }
    }
}