using System.ComponentModel;

namespace DescentCampaignSaver.Descent.Characters
{
    public class ClassAbility:ISearchable
    {
        public string Name { get; set; }
        [Browsable(false)]
        public GameTypes GameType { get { return GameTypes.ClassCard; } }
        public int ExpCost { get; set; }
        public CharacterClasses Class { get; set; }
        public ArchTypes ArchType { get; set; }

        [Browsable(false)]
        public string Description
        {
            get
            {
                return string.Format("Class: {0}\tExp Cost: {1}",
                    Class,
                    ExpCost);
            }
        }
    }
}
