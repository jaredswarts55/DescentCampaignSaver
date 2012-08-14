namespace DescentCampaignSaver.Descent
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    /// <summary>
    /// The descent campaign.
    /// </summary>
    public class DescentCampaign : IPlayer, INotifyPropertyChanged
    {
        /// <summary>
        /// The overlord.
        /// </summary>
        private OverlordCharacter overlord = null;

        /// <summary>
        /// The name.
        /// </summary>
        private string name = "Campaign";

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
        /// Gets or sets the players.
        /// </summary>
        private ObservableCollection<Hero> players;

        /// <summary>
        /// The unspent player gold.
        /// </summary>
        private int? unspentPlayerGold;

        /// <summary>
        /// The notes.
        /// </summary>
        private string notes;

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
                OnPropertyChanged("Overlord");
                this.overlord = value;
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
                OnPropertyChanged("Notes");
                this.notes = value;
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
                OnPropertyChanged("UnspentPlayerGold");
                this.unspentPlayerGold = value;
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
                OnPropertyChanged("Name");
                this.name = value;
            }
        }

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
        /// The on property changed.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        private void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        /// <summary>
        /// The property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}