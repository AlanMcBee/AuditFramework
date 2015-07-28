using System;
using System.ComponentModel;

namespace CodeCharm.Utility
{
    public static class SystemEventHelper
    {
        public static void Raise(this EventHandler eventHandler, object sender)
        {
            var handlers = eventHandler;
            if (handlers != null)
                handlers(sender, EventArgs.Empty);
        }

        public static void Raise(this PropertyChangedEventHandler eventHandler, object sender, string propertyName)
        {
            var handlers = eventHandler;
            if (handlers != null)
                handlers(sender, new PropertyChangedEventArgs(propertyName));
        }

    }
}
