namespace DescentCampaignSaver.Descent
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;

    using DescentCampaignSaver.Descent.Heroes;
    using DescentCampaignSaver.Descent.Overlord;
    using DescentCampaignSaver.Descent.Shared;

    /// <summary>
    /// The descent campaign.
    /// </summary>
    public class DescentCampaign : ITabular, INotifyPropertyChanged
    {
        #region Fields

        /// <summary>
        /// The name.
        /// </summary>
        private string name = "Campaign";

        /// <summary>
        /// The notes.
        /// </summary>
        private string notes;

        /// <summary>
        /// The overlord.
        /// </summary>
        private OverlordCharacter overlord = null;

        /// <summary>
        /// Gets or sets the players.
        /// </summary>
        private ObservableCollection<Hero> players;

        /// <summary>
        /// The unspent player gold.
        /// </summary>
        private int? unspentPlayerGold;

        #endregion

        #region Public Events

        /// <summary>
        /// The property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        public int TotalExp { get; set; }

        /// <summary>
        /// Gets the image.
        /// </summary>
        public string Image
        {
            get
            {
                return @"Data\UIImages\campaign.jpg";
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.OnPropertyChanged("Name");
                this.name = value;
            }
        }

        public Visibility IsClosable
        {
            get
            {
                return Visibility.Hidden;
            }
        }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>
        /// The notes.
        /// </value>
        public string Notes
        {
            get
            {
                return this.notes;
            }

            set
            {
                this.OnPropertyChanged("Notes");
                this.notes = value;
            }
        }

        /// <summary>
        /// Gets or sets the overlord.
        /// </summary>
        /// <value>
        /// The overlord.
        /// </value>
        public OverlordCharacter Overlord
        {
            get
            {
                return this.overlord;
            }

            set
            {
                this.OnPropertyChanged("Overlord");
                this.overlord = value;
            }
        }

        public ObservableCollection<Scenario.Scenario> Scenarios { get; set; }

        /// <summary>
        /// Gets or sets the players.
        /// </summary>
        public ObservableCollection<Hero> Players
        {
            get
            {
                return this.players;
            }

            set
            {
                this.OnPropertyChanged("Players");
                this.players = value;
            }
        }

        /// <summary>
        /// Gets or sets the unspent player gold.
        /// </summary>
        /// <value>
        /// The unspent player gold.
        /// </value>
        public int? UnspentPlayerGold
        {
            get
            {
                return this.unspentPlayerGold;
            }

            set
            {
                this.OnPropertyChanged("UnspentPlayerGold");
                this.unspentPlayerGold = value;
            }
        }

        public bool ExpAutomatic
        {
            get
            {
                return this.expAutomatic;
            }
            set
            {
                this.expAutomatic = value;
            }
        }

        #endregion

        #region Methods

        public void RecalculateExperience()
        {
            if (!ExpAutomatic)
                return;
            this.TotalExp = this.Scenarios.Count(x => x.HasOverlordWon == true || x.HasPlayerWon == true);
            foreach (var player in this.Players)
            {
                var spentExp = player.ClassAbilites.Sum(x => x.ExpCost);
                player.UnspentExp = TotalExp - spentExp;
            }

        }

        private bool expAutomatic = true;
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

        #endregion
    }
}