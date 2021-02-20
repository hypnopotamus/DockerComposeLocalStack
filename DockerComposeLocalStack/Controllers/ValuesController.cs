using System;
using System.Collections.Generic;
using DockerComposeLocalStack.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace DockerComposeLocalStack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly Func<IGetMessagesCommand> _getMessagesCommandFactory;
        private readonly Func<ISaveMessageCommand> _saveMessageCommandFactory;

        public ValuesController
        (
            Func<IGetMessagesCommand> getMessagesCommandFactory,
            Func<ISaveMessageCommand> saveMessageCommandFactory
        )
        {
            _getMessagesCommandFactory = getMessagesCommandFactory;
            _saveMessageCommandFactory = saveMessageCommandFactory;
        }

        [HttpPut]
        public void AddMessage(string message)
        {
            using var command = _saveMessageCommandFactory();

            command.Save(message);
        }

        [HttpGet]
        public IEnumerable<string> GetValues()
        {
            using var command = _getMessagesCommandFactory();

            return command.GetMessages();
        }
    }
}