using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using DescentCampaignSaver.Descent.Relics;

namespace DescentCampaignSaver.Descent
{
    public class Overlord
    {
        public ObservableCollection<OverlordRelic> OverlordRelics { get; set; }
    }
}
