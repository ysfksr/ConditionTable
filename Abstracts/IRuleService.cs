using ConditionTable.Entity;
using ConditionTable.Models;

namespace ConditionTable.Abstracts
{
    public interface IRuleService
    {
        RuleViewModel AddRule(RuleDbModel model);
        RuleViewModel GetAllRules();
        void Delete(string id);
        void EditRule(Rule rule);
        RuleViewModel GetRuleByIntervalValue(decimal value);
    }
}
