using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClassLibrary.Enum
{
    public enum MessageTopic
    {
        Log,
        Error,
        Rundown,
        DetailLock,
        ControlRoom
    }

    public static class MessageTopicExtensions
    {
        public static string ToKafkaTopic(this MessageTopic topic)
        {
            return topic.ToString().ToLower(); 
        }
    }

}
