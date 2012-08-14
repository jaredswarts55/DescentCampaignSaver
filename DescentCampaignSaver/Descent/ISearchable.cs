namespace DescentCampaignSaver.Descent
{
    /// <summary>
    /// The Searchable interface.
    /// </summary>
    public interface ISearchable
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the game type.
        /// </summary>
        GameTypes GameType { get; }
    }
}