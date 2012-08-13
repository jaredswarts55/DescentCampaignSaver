using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using DescentCampaignSaver.Descent;
using DescentCampaignSaver.Descent.Characters;
using DescentCampaignSaver.Descent.Relics;
using DescentCampaignSaver.Descent.SearchItems;
using DescentCampaignSaver.Descent.Shop;

namespace DescentCampaignSaver
{
    public class MyResources
    {
        public MyResources()
        {
        }
        public static List<CharacterClasses> Classes
        {
            get
            {
                return Enum.GetValues(typeof(CharacterClasses)).Cast<CharacterClasses>().ToList();
            }
        }
        public static List<string> itemNames = new List<string>();
        public static ObservableCollection<ShopItem> shopItems = new ObservableCollection<ShopItem>();
        public static ObservableCollection<DescentCharacter> descentCharacters = new ObservableCollection<DescentCharacter>();
        public static ObservableCollection<ClassAbility> classAbilities = new ObservableCollection<ClassAbility>();
        public static ObservableCollection<SearchCardItem> searchCards = new ObservableCollection<SearchCardItem>();
        public static ObservableCollection<PlayerRelic> playerRelics = new ObservableCollection<PlayerRelic>();
        public static ObservableCollection<ISearchable> allSearchableItems = new ObservableCollection<ISearchable>();
    }
}
