using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace SnookerScoringSystem.Domain.Messages
{
    public class GoToScoreBoardPageMessage : ValueChangedMessage<string>
    {
        public GoToScoreBoardPageMessage(string value) : base(value)
        {
        }
    }
}
