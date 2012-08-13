using System.ComponentModel;

namespace DescentCampaignSaver.Descent.Relics
{
    public class PlayerRelic:ISearchable
    {
        public PlayerRelic()
        {
        }

        public string Name { get; set; }
        public ItemTypes ItemType { get; set; }

        [Browsable(false)]
        public string Rules { get; set; }
        [Browsable(false)]
        public string Description
        {
            get
            {
                return string.Format("ItemType: {0}",
                    ItemType);
            }
        }

        [Browsable(false)]
        public GameTypes GameType
        {
            get
            {
                return GameTypes.Relic;
            }
        }
    }
}
