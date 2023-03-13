using System.Collections.Generic;
using System.Data;

namespace ConditionTable.Models
{
    public class RuleViewModel
    {
        public List<Rule> Rules { get; set; }
        public string ErrorMessage { get; set; }
    }
}
