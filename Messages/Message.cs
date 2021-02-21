using System;

namespace Messages
{
    public record Message(string Value)
    {
        public Guid Key { get; } = Guid.NewGuid();
    };
}