namespace DescentCampaignSaver.Descent
{
    public interface ISearchable
    {
        string Name { get; set; }
        string Description { get;}
        GameTypes GameType { get;}
    }
}