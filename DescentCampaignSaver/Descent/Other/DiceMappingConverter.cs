namespace DescentCampaignSaver.Descent.Other
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DescentCampaignSaver.Descent.Shop;

    /// <summary>
    /// The dice mapping converter.
    /// </summary>
    public class DiceMappingConverter : IMappingConverter
    {
        #region Explicit Interface Methods

        /// <summary>
        /// The conversion method.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The System.Object.
        /// </returns>
        object IMappingConverter.ConversionMethod(string item)
        {
            if (string.IsNullOrWhiteSpace(item))
            {
                return new List<Die>();
            }

            var items = item.Trim().Split(';').Select(x => x.Trim());
            var dice = new List<Die>();
            foreach (var d in items)
            {
                dice.Add((Die)Enum.Parse(typeof(Die), d));
            }

            return dice;
        }

        #endregion
    }
}