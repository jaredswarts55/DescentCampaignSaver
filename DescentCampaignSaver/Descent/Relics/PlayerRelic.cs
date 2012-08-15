namespace DescentCampaignSaver.Descent.Relics
{
    using System.ComponentModel;

    using DescentCampaignSaver.Descent.Shared;

    /// <summary>
    /// The player relic.
    /// </summary>
    public class PlayerRelic : ISearchable
    {
        #region Public Properties

        /// <summary>
        /// Gets the description.
        /// </summary>
        [Browsable(false)]
        public string Description
        {
            get
            {
                return string.Format("ItemType: {0}", this.ItemType);
            }
        }

        /// <summary>
        /// Gets the game type.
        /// </summary>
        [Browsable(false)]
        public GameTypes GameType
        {
            get
            {
                return GameTypes.Relic;
            }
        }

        /// <summary>
        /// Gets or sets the item type.
        /// </summary>
        public ItemTypes ItemType { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the rules.
        /// </summary>
        [Browsable(false)]
        public string Rules { get; set; }

        #endregion
    }
}