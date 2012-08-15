namespace DescentCampaignSaver.Descent.Other
{
    using System.Collections.ObjectModel;
    using System.IO;
    using System.IO.Compression;
    using System.Xml.Serialization;

    using DescentCampaignSaver.Descent.Heroes;
    using DescentCampaignSaver.Descent.Overlord;
    using DescentCampaignSaver.Descent.Relics;
    using DescentCampaignSaver.Descent.SearchItems;
    using DescentCampaignSaver.Descent.Shop;

    /// <summary>
    /// The player serializer.
    /// </summary>
    public class PlayerSerializer
    {
        #region Public Methods and Operators

        /// <summary>
        /// The de serialize.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <returns>
        /// The System.Collections.ObjectModel.ObservableCollection`1[T -&gt; DescentCampaignSaver.Descent.Hero].
        /// </returns>
        public static DescentCampaign DeSerialize(string path)
        {
            DescentCampaign campaign;
            var xsr = new XmlSerializer(
                typeof(DescentCampaign), 
                new[]
                    {
                        typeof(ObservableCollection<Hero>), typeof(DescentCampaign), typeof(OverlordCharacter), 
                        typeof(OverlordClassAbility), typeof(ObservableCollection<OverlordClassAbility>), 
                        typeof(OverlordRelic), typeof(ShopItem), typeof(PlayerRelic), typeof(ClassAbility), 
                        typeof(SearchCardItem), typeof(Hero), typeof(DescentCharacter),typeof(Scenario.Scenario),
                        typeof(Scenario.ScenarioTypes),typeof(Scenario.TiedCondition)
                    });
            using (var sr = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (var cr = new GZipStream(sr, CompressionMode.Decompress))
            {
                campaign = (DescentCampaign)xsr.Deserialize(cr);
            }

            return campaign;
        }

        /// <summary>
        /// The serialize.
        /// </summary>
        /// <param name="campaign">
        /// The campaign.
        /// </param>
        /// <param name="path">
        /// The path.
        /// </param>
        public static void Serialize(DescentCampaign campaign, string path)
        {
            using (var sw = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            using (var cw = new GZipStream(sw, CompressionMode.Compress))
            {
                var xsr = new XmlSerializer(
                    typeof(DescentCampaign), 
                    new[]
                        {
                        typeof(ObservableCollection<Hero>), typeof(DescentCampaign), typeof(OverlordCharacter), 
                        typeof(OverlordClassAbility), typeof(ObservableCollection<OverlordClassAbility>), 
                        typeof(OverlordRelic), typeof(ShopItem), typeof(PlayerRelic), typeof(ClassAbility), 
                        typeof(SearchCardItem), typeof(Hero), typeof(DescentCharacter),typeof(Scenario.Scenario),
                        typeof(Scenario.ScenarioTypes),typeof(Scenario.TiedCondition)
                        });
                xsr.Serialize(cw, campaign);
            }
        }

        #endregion
    }
}