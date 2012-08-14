namespace DescentCampaignSaver.Descent
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    /// <summary>
    /// The descent campaign.
    /// </summary>
    public class DescentCampaign : INotifyPropertyChanged
    {
        private OverlordCharacter overlord = null;

        /// <summary>
        /// Gets or sets the players.
        /// </summary>
        public ObservableCollection<Player> Players { get; set; }

        private int? unspentPlayerGold;

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

        private void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}