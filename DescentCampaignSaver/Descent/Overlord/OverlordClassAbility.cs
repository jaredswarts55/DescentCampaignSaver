namespace DescentCampaignSaver.Descent.Overlord
{
    using DescentCampaignSaver.Descent.Shared;

    /// <summary>
    /// An overlord class ability.
    /// </summary>
    public class OverlordClassAbility : ISearchable
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the ability type.
        /// </summary>
        public OverlordAbilityTypes AbilityType { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        public OverlordCategoryType Category { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the exp cost.
        /// </summary>
        public int ExpCost { get; set; }

        /// <summary>
        /// Gets the game type.
        /// </summary>
        public GameTypes GameType
        {
            get
            {
                return GameTypes.OverlordClassAbility;
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        #endregion
    }
}