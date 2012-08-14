namespace DescentCampaignSaver.Descent
{
    /// <summary>
    /// An overlord class ability.
    /// </summary>
    public class OverlordClassAbility : ISearchable
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public GameTypes GameType { get; set; }
    }
}