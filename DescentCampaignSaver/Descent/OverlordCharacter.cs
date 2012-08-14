﻿namespace DescentCampaignSaver.Descent
{
    using System.Collections.ObjectModel;

    using DescentCampaignSaver.Descent.Relics;

    /// <summary>
    /// The overlord.
    /// </summary>
    public class OverlordCharacter
    {
        public string PlayerName { get; set; }

        public int UnspentExp { get; set; }

        /// <summary>
        /// Gets or sets the overlord relics.
        /// </summary>
        public ObservableCollection<OverlordRelic> OverlordRelics { get; set; }

        /// <summary>
        /// Gets or sets the overlord class abilities.
        /// </summary>
        public ObservableCollection<OverlordClassAbility> OverlordClassAbilities { get; set; }
    }
}