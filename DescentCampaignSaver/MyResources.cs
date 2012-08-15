namespace DescentCampaignSaver
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using DescentCampaignSaver.Descent;
    using DescentCampaignSaver.Descent.Characters;
    using DescentCampaignSaver.Descent.Relics;
    using DescentCampaignSaver.Descent.SearchItems;
    using DescentCampaignSaver.Descent.Shop;

    /// <summary>
    /// The my resources.
    /// </summary>
    public class MyResources
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MyResources"/> class.
        /// </summary>
        public MyResources()
        {
        }

        /// <summary>
        /// Gets the classes.
        /// </summary>
        public static List<CharacterClasses> Classes
        {
            get
            {
                return Enum.GetValues(typeof(CharacterClasses)).Cast<CharacterClasses>().ToList();
            }
        }

        /// <summary>
        /// The item names.
        /// </summary>
        public static List<string> itemNames = new List<string>();

        /// <summary>
        /// The shop items.
        /// </summary>
        public static ObservableCollection<ShopItem> shopItems = new ObservableCollection<ShopItem>();

        /// <summary>
        /// The descent characters.
        /// </summary>
        public static ObservableCollection<DescentCharacter> descentCharacters = new ObservableCollection<DescentCharacter>();

        /// <summary>
        /// The class abilities.
        /// </summary>
        public static ObservableCollection<ClassAbility> classAbilities = new ObservableCollection<ClassAbility>();

        /// <summary>
        /// The search cards.
        /// </summary>
        public static ObservableCollection<SearchCardItem> searchCards = new ObservableCollection<SearchCardItem>();

        /// <summary>
        /// The overlord relics.
        /// </summary>
        public static ObservableCollection<OverlordRelic> overlordRelics = new ObservableCollection<OverlordRelic>();

        /// <summary>
        /// The player relics.
        /// </summary>
        public static ObservableCollection<PlayerRelic> playerRelics = new ObservableCollection<PlayerRelic>();

        /// <summary>
        /// The all searchable items.
        /// </summary>
        public static ObservableCollection<ISearchable> allSearchableItems = new ObservableCollection<ISearchable>();

        public static ObservableCollection<ISearchable> overlordSearchableItems = new ObservableCollection<ISearchable>();

        public static ObservableCollection<OverlordClassAbility> overlordClassAbilities = new ObservableCollection<OverlordClassAbility>();
    }
}