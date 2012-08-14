namespace DescentCampaignSaver.Descent.SearchItems
{
    using System.ComponentModel;

    /// <summary>
    /// The search card item.
    /// </summary>
    public class SearchCardItem : ISearchable
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the search item type.
        /// </summary>
        public ItemTypes SearchItemType { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Gets or sets the rules.
        /// </summary>
        [Browsable(false)]
        public string Rules { get; set; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        [Browsable(false)]
        public string Description
        {
            get
            {
                return string.Format("ItemType: {0}\tValue: {1}", SearchItemType, Value);
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
                return GameTypes.SearchCard;
            }
        }
    }
}