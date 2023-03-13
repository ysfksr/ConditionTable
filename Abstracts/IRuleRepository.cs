using ConditionTable.Entity;
using System.Collections.Generic;

namespace ConditionTable.Abstracts
{
    public interface IRuleRepository
    {
        bool AddRule(List<RuleDbModel> model);
        List<RuleDbModel> GetAllRules();
        void TruncateTable();
        bool IsDataExist();
    }
}
