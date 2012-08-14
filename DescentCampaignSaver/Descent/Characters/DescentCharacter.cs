namespace DescentCampaignSaver.Descent.Characters
{
    /// <summary>
    /// The descent character.
    /// </summary>
    public class DescentCharacter
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the arch type.
        /// </summary>
        public ArchTypes ArchType { get; set; }

        /// <summary>
        /// Gets or sets the speed.
        /// </summary>
        public int Speed { get; set; }

        /// <summary>
        /// Gets or sets the max health.
        /// </summary>
        public int MaxHealth { get; set; }

        /// <summary>
        /// Gets or sets the max fatigue.
        /// </summary>
        public int MaxFatigue { get; set; }

        /// <summary>
        /// Gets or sets the defense.
        /// </summary>
        public string Defense { get; set; }

        /// <summary>
        /// Gets or sets the will.
        /// </summary>
        public int Will { get; set; }

        /// <summary>
        /// Gets or sets the might.
        /// </summary>
        public int Might { get; set; }

        /// <summary>
        /// Gets or sets the knowledge.
        /// </summary>
        public int Knowledge { get; set; }

        /// <summary>
        /// Gets or sets the awareness.
        /// </summary>
        public int Awareness { get; set; }

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The System.Boolean.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is DescentCharacter)
            {
                var dc = obj as DescentCharacter;
                if (this.Name == dc.Name)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return base.Equals(obj);
        }
    }
}