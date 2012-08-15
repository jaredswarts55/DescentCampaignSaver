namespace DescentCampaignSaver.Descent.Scenario
{
    using System.ComponentModel;
    using System.Data;

    /// <summary>
    /// The Scenario class type
    /// </summary>
    public class Scenario
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        [Browsable(false)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the act.
        /// </summary>
        /// <value>
        /// The act.
        /// </value>
        public int? Act { get; set; }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        [Browsable(false)]
        public int? Parent { get; set; }

        /// <summary>
        /// Gets or sets the condition.
        /// </summary>
        /// <value>
        /// The condition.
        /// </value>
        [Browsable(false)]
        public TiedCondition Condition { get; set; }

        /// <summary>
        /// Gets or sets the type of the scenario.
        /// </summary>
        /// <value>
        /// The type of the scenario.
        /// </value>
        public ScenarioTypes ScenarioType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has overlord won.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has overlord won; otherwise, <c>false</c>.
        /// </value>
        [DisplayName("Overlord Won")]
        public bool HasOverlordWon { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has player won.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has player won; otherwise, <c>false</c>.
        /// </value>
        [DisplayName("Players Won")]
        public bool HasPlayerWon { get; set; }

    }
}