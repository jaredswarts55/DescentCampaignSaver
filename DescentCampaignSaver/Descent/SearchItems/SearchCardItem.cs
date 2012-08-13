using System.ComponentModel;
using DescentCampaignSaver.Descent.Relics;

namespace DescentCampaignSaver.Descent.SearchItems
{
    public class SearchCardItem : ISearchable
    {
        public string Name { get; set; }
        public ItemTypes SearchItemType { get; set; }
        public int Value { get; set; }
        [Browsable(false)]
        public string Rules { get; set; }
        [Browsable(false)]
        public string Description
        {
            get
            {
                return string.Format("ItemType: {0}\tValue: {1}",
                    SearchItemType,
                    Value);
            }
        }

        [Browsable(false)]
        public GameTypes GameType
        {
            get {
                return GameTypes.SearchCard;
            }
        }
    }
}
