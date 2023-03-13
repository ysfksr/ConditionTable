using CsvHelper.Configuration.Attributes;
using System;

namespace ConditionTable.Entity
{
    public class RuleDbModel
    {
        [Name("Id")]
        public string Id { get; set; }
        [Name("Name")]
        public string Name { get; set; }
        [Name("LowerBound")]
        public decimal LowerBound { get; set; }
        [Name("LeftEquality")]
        public int LeftEquality { get; set; }
        [Name("UpperBound")]
        public decimal UpperBound { get; set; }
        [Name("RightEquality")]
        public int RightEquality { get; set; }
        [Name("Result")]
        public decimal Result { get; set; }
    }
}
