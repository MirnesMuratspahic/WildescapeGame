using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class Blueprint
    {
        public string Name { get; set; }
        public List<RequirementItem> Requirements {  get; set; }

        public Blueprint(string name, List<RequirementItem> requirements) 
        {
            Name = name;
            Requirements = requirements;
        }

    }
}
