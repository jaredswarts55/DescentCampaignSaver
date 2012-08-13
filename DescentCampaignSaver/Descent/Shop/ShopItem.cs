using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Mapping;

namespace DescentCampaignSaver.Descent.Shop
{
    [DebuggerDisplay("Name: {Name} Cost: {Cost}")]
    public class ShopItem:ISearchable
    {
        public ShopItem()
        {
        }

        public string Name { get; set; }
        public int Cost { get; set; }
        public int Quantity { get; set; }
        public EquipType Equip { get; set; }
        public AttackTypes AttackType { get; set; }
        [TypeConversionMethod(typeof(DiceMappingConverter))]
        [Browsable(false)]
        public List<Die> Dice { get; set; }
        public ItemTypes ItemType { get; set; }
        [Browsable(false)]
        public string Rules { get; set; }
        [Browsable(false)]
        public string Description
        {
            get
            {
                return string.Format("ItemType: {0}\tCost: {1}",
                    ItemType,
                    Cost);
            }
        }
        [Browsable(false)]
        public GameTypes GameType
        {
            get { return GameTypes.ShopItem; }
        }
    }
}
