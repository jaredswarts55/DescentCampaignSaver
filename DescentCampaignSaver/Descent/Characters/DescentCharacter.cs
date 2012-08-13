namespace DescentCampaignSaver.Descent.Characters
{
    public class DescentCharacter
    {
        public DescentCharacter()
        {
        }

        public string Name { get; set; }
        public ArchTypes ArchType { get; set; }
        public int Speed { get; set; }
        public int MaxHealth { get; set; }
        public int MaxFatigue { get; set; }
        public string Defense { get; set; }
        public int Will { get; set; }
        public int Might { get; set; }
        public int Knowledge { get; set; }
        public int Awareness { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is DescentCharacter)
            {
                var dc = obj as DescentCharacter;
                if (this.Name == dc.Name)
                    return true;
                else
                    return false;
            }
            return base.Equals(obj);
        }
    }
}