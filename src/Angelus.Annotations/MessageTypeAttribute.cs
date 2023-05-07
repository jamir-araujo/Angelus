using System;

namespace Angelus
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class MessageTypeAttribute : Attribute
    {
        public MessageTypeAttribute(string messageType)
        {
            MessageType = messageType;
        }

        public string MessageType { get; }
    }
}
