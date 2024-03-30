using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace SnookerScoringSystem.Domain.Messages
{
    public class ResetPlayerScoreMessage : ValueChangedMessage<string>
    {
        public ResetPlayerScoreMessage(string value) : base(value)
        {
        }
    }
}
