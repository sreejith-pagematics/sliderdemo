using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SliderDemo1
{
    internal class LoginResponse
    {
        public class DragCompletedMessage : ValueChangedMessage<string>
        {
            public DragCompletedMessage(string value) : base(value)
            {
            }
        }
    }
}
