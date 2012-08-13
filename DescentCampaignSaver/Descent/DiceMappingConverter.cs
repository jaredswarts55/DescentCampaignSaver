using System;
using System.Collections.Generic;
using System.Linq;
using DescentCampaignSaver.Descent.Shop;
using Mapping;

namespace DescentCampaignSaver.Descent
{
    public class DiceMappingConverter:IMappingConverter
   {
        object IMappingConverter.ConversionMethod(string item)
        {
            if (String.IsNullOrWhiteSpace(item))
            {
                return new List<Die>();
            }
            else
            {
                var items = item.Trim().Split(';').Select(x => x.Trim());
                var dice = new List<Die>();
                foreach (var d in items)
                    dice.Add((Die)Enum.Parse(typeof(Die), d));
                return dice;
            }
        }
    }
}
