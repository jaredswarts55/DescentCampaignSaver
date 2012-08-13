using System.Collections.ObjectModel;
using DescentCampaignSaver.Descent.Characters;
using DescentCampaignSaver.Descent.Relics;
using DescentCampaignSaver.Descent.SearchItems;
using DescentCampaignSaver.Descent.Shop;

namespace DescentCampaignSaver.Descent
{
    public class Player
    {
        public Player()
        {
        }

        public string Name { get; set; }
        public int UnspentExp { get; set; }
        public bool IsDiseased { get; set; }
        public bool IsStunned { get; set; }
        public bool IsPoisoned { get; set; }
        public bool IsImmobilized { get; set; }
        public int CurrentHealth { get; set; }
        public int CurrentFatigue { get; set; }
        public ObservableCollection<ClassAbility> ClassAbilites { get; set; }
        public ObservableCollection<ShopItem> ShopItems { get; set; }
        public ObservableCollection<SearchCardItem> SearchCardItems { get; set; }
        public ObservableCollection<PlayerRelic> PlayerRelics { get; set; }
        public string Character { get; set; }
        public string Class { get; set; }
    }
}
