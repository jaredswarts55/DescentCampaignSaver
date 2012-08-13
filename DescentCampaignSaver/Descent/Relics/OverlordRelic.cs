using System.ComponentModel;

namespace DescentCampaignSaver.Descent.Relics
{
    public class OverlordRelic:ISearchable
    {
        public OverlordRelic()
        {
        }

        public string Name { get; set; }
        public string Rules { get; set; }
        [Browsable(false)]
        public string Description { get; set; }

        public GameTypes GameType
        {
            get { return GameTypes.Relic; }
        }
    }
}
