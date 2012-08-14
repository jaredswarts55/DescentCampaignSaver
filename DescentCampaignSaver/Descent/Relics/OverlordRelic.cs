namespace DescentCampaignSaver.Descent.Relics
{
    using System.ComponentModel;

    /// <summary>
    /// The overlord relic.
    /// </summary>
    public class OverlordRelic : ISearchable
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the rules.
        /// </summary>
        public string Rules { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [Browsable(false)]
        public string Description { get; set; }

        /// <summary>
        /// Gets the game type.
        /// </summary>
        public GameTypes GameType
        {
            get
            {
                return GameTypes.Relic;
            }
        }
    }
}