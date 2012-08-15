namespace DescentCampaignSaver.Descent.Heroes
{
    using System.ComponentModel;

    using DescentCampaignSaver.Descent.Shared;

    /// <summary>
    /// The class ability.
    /// </summary>
    public class ClassAbility : ISearchable
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the arch type.
        /// </summary>
        public ArchTypes ArchType { get; set; }

        /// <summary>
        /// Gets or sets the class.
        /// </summary>
        public CharacterClasses Class { get; set; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        [Browsable(false)]
        public string Description
        {
            get
            {
                return string.Format("Class: {0}\tExp Cost: {1}", this.Class, this.ExpCost);
            }
        }

        /// <summary>
        /// Gets or sets the exp cost.
        /// </summary>
        public int ExpCost { get; set; }

        /// <summary>
        /// Gets the game type.
        /// </summary>
        [Browsable(false)]
        public GameTypes GameType
        {
            get
            {
                return GameTypes.ClassCard;
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        #endregion
    }
}