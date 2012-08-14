namespace DescentCampaignSaver.Descent
{
    using System.Collections.ObjectModel;
    using System.IO;
    using System.IO.Compression;
    using System.Xml.Serialization;

    using DescentCampaignSaver.Descent.Characters;
    using DescentCampaignSaver.Descent.Relics;
    using DescentCampaignSaver.Descent.SearchItems;
    using DescentCampaignSaver.Descent.Shop;

    /// <summary>
    /// The player serializer.
    /// </summary>
    public class PlayerSerializer
    {
        /// <summary>
        /// The serialize.
        /// </summary>
        /// <param name="players">
        /// The players.
        /// </param>
        /// <param name="path">
        /// The path.
        /// </param>
        public static void Serialize(ObservableCollection<Player> players, string path)
        {
            using (var sw = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            using (var cw = new GZipStream(sw, CompressionMode.Compress))
            {
                var xsr = new XmlSerializer(
                    typeof(ObservableCollection<Player>),
                    new[]
                        {
                            typeof(ShopItem),
                            typeof(PlayerRelic), 
                            typeof(ClassAbility),
                            typeof(SearchCardItem),
                            typeof(Player),
                            typeof(DescentCharacter)
                        });
                xsr.Serialize(cw, players);
            }
        }

        /// <summary>
        /// The de serialize.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <returns>
        /// The System.Collections.ObjectModel.ObservableCollection`1[T -&gt; DescentCampaignSaver.Descent.Player].
        /// </returns>
        public static ObservableCollection<Player> DeSerialize(string path)
        {
            ObservableCollection<Player> players;
            var xsr = new XmlSerializer(
                typeof(ObservableCollection<Player>),
                new[]
                    {
                        typeof(ShopItem),
                        typeof(SearchCardItem),
                        typeof(PlayerRelic),
                        typeof(ClassAbility),
                        typeof(Player),
                        typeof(DescentCharacter)
                    });
            using (var sr = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (var cr = new GZipStream(sr, CompressionMode.Decompress))
            {
                players = (ObservableCollection<Player>)xsr.Deserialize(cr);
            }

            return players;
        }
    }
}