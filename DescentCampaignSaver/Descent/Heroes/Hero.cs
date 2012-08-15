namespace DescentCampaignSaver.Descent.Heroes
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;

    using DescentCampaignSaver.Descent.Relics;
    using DescentCampaignSaver.Descent.SearchItems;
    using DescentCampaignSaver.Descent.Shared;
    using DescentCampaignSaver.Descent.Shop;

    /// <summary>
    /// The player.
    /// </summary>
    public class Hero : ITabular,INotifyPropertyChanged
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Hero"/> class.
        /// </summary>
        public Hero()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the character.
        /// </summary>
        public DescentCharacter Character { get; set; }

        /// <summary>
        /// Gets or sets the class.
        /// </summary>
        public CharacterClasses? Class { get; set; }

        /// <summary>
        /// Gets or sets the class abilites.
        /// </summary>
        public ObservableCollection<ClassAbility> ClassAbilites { get; set; }

        /// <summary>
        /// Gets or sets the current fatigue.
        /// </summary>
        public int CurrentFatigue { get; set; }

        /// <summary>
        /// Gets or sets the current health.
        /// </summary>
        public int CurrentHealth { get; set; }

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
        /// Gets or sets a value indicating whether is diseased.
        /// </summary>
        public bool IsDiseased { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is immobilized.
        /// </summary>
        public bool IsImmobilized { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is poisoned.
        /// </summary>
        public bool IsPoisoned { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is stunned.
        /// </summary>
        public bool IsStunned { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        public Visibility IsClosable
        {
            get
            {
                return Visibility.Visible;
            }
        }

        /// <summary>
        /// Gets or sets the player relics.
        /// </summary>
        public ObservableCollection<PlayerRelic> PlayerRelics { get; set; }

        /// <summary>
        /// Gets or sets the search card items.
        /// </summary>
        public ObservableCollection<SearchCardItem> SearchCardItems { get; set; }

        /// <summary>
        /// Gets or sets the shop items.
        /// </summary>
        public ObservableCollection<ShopItem> ShopItems { get; set; }

        private int unspentExp = 0;
        /// <summary>
        /// Gets or sets the unspent exp.
        /// </summary>
        public int UnspentExp
        {
            get
            {
                return unspentExp;
            }
            set
            {
                this.unspentExp = value;
                this.OnPropertyChanged("UnspentExp");
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The System.Boolean.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is Hero)
            {
                var hobj = obj as Hero;
                if (hobj.Name == this.Name)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return base.Equals(obj);
        }

        #endregion
        /// <summary>
        /// The on property changed.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        private void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}