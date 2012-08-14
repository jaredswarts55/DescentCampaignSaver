namespace DescentCampaignSaver.Descent.Shop
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;

    using Mapping;

    /// <summary>
    /// The shop item.
    /// </summary>
    [DebuggerDisplay("Name: {Name} Cost: {Cost}")]
    public class ShopItem : ISearchable
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the cost.
        /// </summary>
        public int Cost { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the equip.
        /// </summary>
        public EquipType Equip { get; set; }

        /// <summary>
        /// Gets or sets the attack type.
        /// </summary>
        public AttackTypes AttackType { get; set; }

        /// <summary>
        /// Gets or sets the dice.
        /// </summary>
        [TypeConversionMethod(typeof(DiceMappingConverter))]
        [Browsable(false)]
        public List<Die> Dice { get; set; }

        /// <summary>
        /// Gets or sets the item type.
        /// </summary>
        public ItemTypes ItemType { get; set; }

        /// <summary>
        /// Gets or sets the rules.
        /// </summary>
        [Browsable(false)]
        public string Rules { get; set; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        [Browsable(false)]
        public string Description
        {
            get
            {
                return string.Format("ItemType: {0}\tCost: {1}", ItemType, Cost);
            }
        }

        /// <summary>
        /// Gets the game type.
        /// </summary>
        [Browsable(false)]
        public GameTypes GameType
        {
            get
            {
                return GameTypes.ShopItem;
            }
        }
    }
}