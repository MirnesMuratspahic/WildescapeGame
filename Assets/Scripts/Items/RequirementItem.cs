using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public  class RequirementItem
    {
        public string RequirementName { get; set; }
        public int RequirementAmount { get; set; }

        public RequirementItem(string name, int amount = 0) 
        {
            RequirementName = name;
            RequirementAmount = amount;
        }
    }
}
