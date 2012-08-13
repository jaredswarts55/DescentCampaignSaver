using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;
using DescentCampaignSaver.Descent.Characters;
using DescentCampaignSaver.Descent.Relics;
using DescentCampaignSaver.Descent.SearchItems;
using DescentCampaignSaver.Descent.Shop;

namespace DescentCampaignSaver.Descent
{
    public class PlayerSerializer
    {
        public static void Serialize(ObservableCollection<Player> players, string path)
        {
            using (var sw = new StreamWriter(path, false))
            {
                var xsr = new XmlSerializer(typeof(ObservableCollection<Player>),
                                                      new Type[] { typeof(ShopItem),typeof(PlayerRelic), typeof(ClassAbility),typeof(SearchCardItem), typeof(Player), typeof(DescentCharacter) });
                xsr.Serialize(sw, players);
            }
        }
        public static ObservableCollection<Player> DeSerialize(string path)
        {
            ObservableCollection<Player> players;
            var xsr = new XmlSerializer(typeof(ObservableCollection<Player>),
                            new Type[] { typeof(ShopItem),typeof(SearchCardItem),typeof(PlayerRelic),typeof(ClassAbility), typeof(Player), typeof(DescentCharacter) });
            using (var sr = new StreamReader(path))
            {
                players = (ObservableCollection<Player>)xsr.Deserialize(sr);
            }
            return players;
        }
    }
}
