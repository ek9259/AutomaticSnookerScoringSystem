using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace SnookerScoringSystem.Domain.Messages
{
    public class ScoringEventData
    {
        public string Messages1 { get; set; }
        public string ColoredText { get; set; }
        public string Messages2 { get; set; }
        public int Classid { get; set; }

        public ScoringEventData(string messages1, string coloredText, string messages2, int classId)
        {
            Messages1 = messages1;
            ColoredText = coloredText;
            Messages2 = messages2;
            Classid = classId;
        }
    }

    public class ScoringEventPopupMessage : ValueChangedMessage<ScoringEventData>
    {
        public ScoringEventPopupMessage(ScoringEventData value) : base(value)
        {
        }
    }
}