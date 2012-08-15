namespace DescentCampaignSaver.Descent.Overlord
{
    using System.Collections.ObjectModel;
    using System.Windows;

    using DescentCampaignSaver.Descent.Relics;
    using DescentCampaignSaver.Descent.Shared;

    /// <summary>
    /// The overlord.
    /// </summary>
    public class OverlordCharacter : ITabular
    {
        #region Fields

        /// <summary>
        /// The name.
        /// </summary>
        private string name = "Overlord";

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the image.
        /// </summary>
        public string Image
        {
            get
            {
                return @"Data\UIImages\overlordIcon.png";
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
        /// Gets or sets the overlord class abilities.
        /// </summary>
        public ObservableCollection<OverlordClassAbility> OverlordClassAbilities { get; set; }

        /// <summary>
        /// Gets or sets the overlord relics.
        /// </summary>
        public ObservableCollection<OverlordRelic> OverlordRelics { get; set; }

        /// <summary>
        /// Gets or sets the player name.
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        /// Gets or sets the unspent exp.
        /// </summary>
        public int UnspentExp { get; set; }

        #endregion
    }
}