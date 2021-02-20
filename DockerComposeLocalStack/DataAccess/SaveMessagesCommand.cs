using System.Data;

namespace DockerComposeLocalStack.DataAccess
{
    public class SaveMessagesCommand : ISaveMessageCommand
    {
        private readonly IDbConnection _connection;

        public SaveMessagesCommand(IDbConnection connection)
        {
            _connection = connection;
            _connection.Open();
        }

        public void Save(string message)
        {
            using var command = _connection.CreateCommand();

            command.CommandType = CommandType.Text;
            command.CommandText = @"
INSERT INTO Message
(Value)
VALUES(@message);";

            var messageParameter = command.CreateParameter();
            messageParameter.DbType = DbType.AnsiString;
            messageParameter.ParameterName = "@message";
            messageParameter.Value = message;
            command.Parameters.Add(messageParameter);

            command.ExecuteNonQuery();
        }

        public void Dispose() => _connection.Dispose();
    }
}