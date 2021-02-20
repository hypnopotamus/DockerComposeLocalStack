using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;

namespace DockerComposeLocalStack.DataAccess
{
    public class GetMessagesCommand : IGetMessagesCommand
    {
        private readonly IDbConnection _connection;

        public GetMessagesCommand(IDbConnection connection)
        {
            _connection = connection;
            _connection.Open();
        }

        public IEnumerable<string> GetMessages()
        {
            using var command = _connection.CreateCommand();

            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT Value FROM Message;";

            using var reader = command.ExecuteReader();
            var valuePosition = reader.GetOrdinal("Value");

            var messages = new Collection<string>();
            while (reader.Read())
            {
                messages.Add(reader.GetString(valuePosition));
            }

            return messages;
        }

        public void Dispose() => _connection.Dispose();
    }
}