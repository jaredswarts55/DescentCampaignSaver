﻿namespace DescentCampaignSaver.Descent.SearchItems
{
    using System.ComponentModel;

    using DescentCampaignSaver.Descent.Shared;

    /// <summary>
    /// The search card item.
    /// </summary>
    public class SearchCardItem : ISearchable
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
                return string.Format("ItemType: {0}\tValue: {1}", this.SearchItemType, this.Value);
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

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the rules.
        /// </summary>
        [Browsable(false)]
        public string Rules { get; set; }

        /// <summary>
        /// Gets or sets the search item type.
        /// </summary>
        public ItemTypes SearchItemType { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public int Value { get; set; }

        #endregion
    }
}