namespace DescentCampaignSaver.Descent
{
    using System.Collections.ObjectModel;

    using DescentCampaignSaver.Descent.Characters;
    using DescentCampaignSaver.Descent.Relics;
    using DescentCampaignSaver.Descent.SearchItems;
    using DescentCampaignSaver.Descent.Shop;

    /// <summary>
    /// The player.
    /// </summary>
    public class Hero : IPlayer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Hero"/> class.
        /// </summary>
        public Hero()
        {
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the image.
        /// </summary>
        public string Image
        {
            get
            {
                return @"Data\UIImages\playerIcon.png";
            }
        }

        /// <summary>
        /// Gets or sets the unspent exp.
        /// </summary>
        public int UnspentExp { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is diseased.
        /// </summary>
        public bool IsDiseased { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is stunned.
        /// </summary>
        public bool IsStunned { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is poisoned.
        /// </summary>
        public bool IsPoisoned { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is immobilized.
        /// </summary>
        public bool IsImmobilized { get; set; }

        /// <summary>
        /// Gets or sets the current health.
        /// </summary>
        public int CurrentHealth { get; set; }

        /// <summary>
        /// Gets or sets the current fatigue.
        /// </summary>
        public int CurrentFatigue { get; set; }

        /// <summary>
        /// Gets or sets the class abilites.
        /// </summary>
        public ObservableCollection<ClassAbility> ClassAbilites { get; set; }

        /// <summary>
        /// Gets or sets the shop items.
        /// </summary>
        public ObservableCollection<ShopItem> ShopItems { get; set; }

        /// <summary>
        /// Gets or sets the search card items.
        /// </summary>
        public ObservableCollection<SearchCardItem> SearchCardItems { get; set; }

        /// <summary>
        /// Gets or sets the player relics.
        /// </summary>
        public ObservableCollection<PlayerRelic> PlayerRelics { get; set; }

        /// <summary>
        /// Gets or sets the character.
        /// </summary>
        public DescentCharacter Character { get; set; }

        /// <summary>
        /// Gets or sets the class.
        /// </summary>
        public CharacterClasses? Class { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Hero)
            {
                var hobj = obj as Hero;
                if (hobj.Name == this.Name)
                    return true;
                else
                    return false;
            }
            return base.Equals(obj);
        }

    }
}