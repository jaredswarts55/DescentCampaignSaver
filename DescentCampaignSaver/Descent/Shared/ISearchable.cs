namespace DescentCampaignSaver.Descent.Shared
{
    /// <summary>
    /// The Searchable interface.
    /// </summary>
    public interface ISearchable
    {
        #region Public Properties

        /// <summary>
        /// Gets the description.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the game type.
        /// </summary>
        GameTypes GameType { get; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        string Name { get; set; }

        #endregion
    }
}