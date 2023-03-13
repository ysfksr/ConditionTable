using System;

namespace ConditionTable.Models
{
    public class Rule
    {
        public Guid Id { get; set; }
        public string Interval { get; set; }
        public decimal Result { get; set; }
    }
}
