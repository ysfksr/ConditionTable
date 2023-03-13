using ConditionTable.Models;
using System.Collections.Generic;
using System;
using ConditionTable.Entity;
using ConditionTable.Abstracts;
using ConditionTable.Enums;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ConditionTable.Service
{
    public class RuleService : IRuleService
    {
        private readonly IRuleRepository _ruleRepository;
        public RuleService(IRuleRepository ruleRepository)
        {
            _ruleRepository = ruleRepository;
        }
        public RuleViewModel AddRule(RuleDbModel model)
        {
            if (model == null || model.UpperBound <= model.LowerBound)
            {
                return new RuleViewModel
                {
                    ErrorMessage = "Please Provide a Correct Interval, Try Again",
                    Rules = GetAllRules().Rules
                };
            }

            model.Id = GuidWithoutScore();
            var ruleList = new List<RuleDbModel>();

            bool isDataExist = _ruleRepository.IsDataExist();

            if (isDataExist)
            {
                var allRules = _ruleRepository.GetAllRules();

                if (AnyValidationError(model, allRules))
                {
                    return new RuleViewModel
                    {
                        ErrorMessage = "You Have Entered a Value Within the Current Value Range, Try Again",
                        Rules = GetAllRules().Rules
                    };
                }

                var upperBoundIntersection = allRules.Where(x => x.LowerBound <= model.UpperBound && x.UpperBound == decimal.MaxValue).FirstOrDefault();
                var lowerBoundIntersection = allRules.Where(x => x.UpperBound >= model.LowerBound && x.LowerBound == decimal.MinValue).FirstOrDefault();

                if (upperBoundIntersection != null)
                {
                    allRules.Remove(upperBoundIntersection);
                    upperBoundIntersection.LowerBound = model.UpperBound;
                    ruleList.Add(upperBoundIntersection);
                }

                if (lowerBoundIntersection != null)
                {
                    allRules.Remove(lowerBoundIntersection);
                    lowerBoundIntersection.UpperBound = model.LowerBound;
                    ruleList.Add(lowerBoundIntersection);
                }

                ruleList.Add(model);

                foreach (var item in allRules)
                {
                    ruleList.Add(item);
                }
            }
            else
            {
                ruleList.Add(model);

                ruleList.Add(new RuleDbModel
                {
                    Id = GuidWithoutScore(),
                    Name = model.Name,
                    LowerBound = decimal.MinValue,
                    UpperBound = model.LowerBound,
                    LeftEquality = model.LeftEquality == (int)EqualityTypes.LowerAndEqual ? (int)EqualityTypes.Lower : (int)EqualityTypes.LowerAndEqual,
                    RightEquality = model.RightEquality == (int)EqualityTypes.GreaterAndEqual ? (int)EqualityTypes.Greater : (int)EqualityTypes.GreaterAndEqual,
                });

                ruleList.Add(new RuleDbModel
                {
                    Id = GuidWithoutScore(),
                    Name = model.Name,
                    LowerBound = model.UpperBound,
                    UpperBound = decimal.MaxValue,
                    LeftEquality = model.LeftEquality == (int)EqualityTypes.LowerAndEqual ? (int)EqualityTypes.Lower : (int)EqualityTypes.LowerAndEqual,
                    RightEquality = model.RightEquality == (int)EqualityTypes.GreaterAndEqual ? (int)EqualityTypes.Greater : (int)EqualityTypes.GreaterAndEqual,
                });

            }

            bool result = _ruleRepository.AddRule(ruleList);

            if (result)
            {
                return GetAllRules();
            }

            return new RuleViewModel();
        }
        public RuleViewModel GetAllRules()
        {
            var allRules = _ruleRepository.GetAllRules();

            if (allRules == null)
            {
                return new RuleViewModel();
            }

            var returnModel = new RuleViewModel()
            {
                Rules = new List<Rule>()
            };


            foreach (var rule in allRules)
            {
                string Interval = RuleAsString(rule);

                returnModel.Rules.Add(new Rule()
                {
                    Id = rule.Id,
                    Interval = Interval,
                    Result = rule.Result
                });
            }

            return returnModel;
        }
        public RuleViewModel GetRuleByIntervalValue(decimal value)
        {
            var rules = _ruleRepository.GetAllRules().Where(x => x.UpperBound >= value && x.LowerBound <= value);

            var returnModel = new RuleViewModel();
            returnModel.Rules = new List<Rule>();
            if (!rules.Any())
            {
                returnModel.ErrorMessage = "Undefined Value";
                return returnModel;
            }

            if (rules.Count() > 1)
            {
                if(rules.Any(x => x.UpperBound == value && x.RightEquality == (int)EqualityTypes.GreaterAndEqual))
                {
                    var rule = rules.Where(x => x.UpperBound == value && x.RightEquality == (int)EqualityTypes.GreaterAndEqual).FirstOrDefault();

                    string Interval = RuleAsString(rule);

                    returnModel.Rules.Add(new Rule
                    {
                        Interval = Interval,
                        Result = rule.Result
                    });

                    return returnModel;
                }
                else if(rules.Any(x => x.LowerBound == value && x.LeftEquality == (int)EqualityTypes.LowerAndEqual))
                {
                    var rule = rules.Where(x => x.LowerBound == value && x.LeftEquality == (int)EqualityTypes.LowerAndEqual).FirstOrDefault();

                    string Interval = RuleAsString(rule);

                    returnModel.Rules.Add(new Rule
                    {
                        Interval = Interval,
                        Result = rule.Result
                    });

                    return returnModel;
                }
                return returnModel;
            }
            else
            {
                var rule = rules.FirstOrDefault();
                string Interval = RuleAsString(rule);

                returnModel.Rules.Add(new Rule
                {
                    Interval = Interval,
                    Result = rule.Result
                });

                return returnModel;
            }
        }
        public void TruncateTable()
        {
            _ruleRepository.TruncateTable();
        }
        public void Delete(string id)
        {
            var allRules = _ruleRepository.GetAllRules();

            var deleteRule = allRules.FirstOrDefault(x => x.Id == id);

            allRules.Remove(deleteRule);

            _ruleRepository.AddRule(allRules);

        }
        private string RuleAsString(RuleDbModel rule)
        {
            string ruleAsString = string.Empty;

            ruleAsString += rule.LowerBound == decimal.MinValue ? "-∞" : rule.LowerBound.ToString();

            if (rule.LeftEquality == (int)EqualityTypes.Lower)
            {
                ruleAsString += " < ";
            }
            else
            {
                ruleAsString += " ≤ ";
            }

            ruleAsString += rule.Name;

            if (rule.RightEquality == (int)EqualityTypes.Greater)
            {
                ruleAsString += " < ";
            }
            else
            {
                ruleAsString += " ≤ ";
            }

            ruleAsString += rule.UpperBound == decimal.MaxValue ? "+∞" : rule.UpperBound.ToString();

            return ruleAsString;
        }
        private bool AnyValidationError(RuleDbModel model, List<RuleDbModel> allRules)
        {
            if (!allRules.Any())
            {
                return false;
            }
            //Fluent Validation Kullanacaktım Pek Uygun Olmadı :)
            var intersectionRules = allRules.Where(x => x.LowerBound != decimal.MinValue && x.UpperBound != decimal.MaxValue);
            bool lowerBoundEquality;
            bool upperBoundEquality;
            if (model.LeftEquality == (int)EqualityTypes.Lower)
            {
                lowerBoundEquality = intersectionRules.Any(x => x.LowerBound < model.LowerBound && x.UpperBound > model.LowerBound);
            }
            else
            {
                lowerBoundEquality = intersectionRules.Any(x => x.LowerBound <= model.LowerBound && x.UpperBound >= model.LowerBound);

            }


            if (model.RightEquality == (int)EqualityTypes.Greater)
            {
                upperBoundEquality = intersectionRules.Any(x => x.UpperBound > model.UpperBound && x.LowerBound < model.UpperBound);
            }
            else
            {
                upperBoundEquality = intersectionRules.Any(x => x.UpperBound >= model.UpperBound && x.LowerBound <= model.UpperBound);

            }

            return lowerBoundEquality || upperBoundEquality;
        }
        private string GuidWithoutScore()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
        public void EditRule(Rule rule)
        {
            var allRules = _ruleRepository.GetAllRules();
            var modifiedRule = allRules.FirstOrDefault(X => X.Id == rule.Id);

            allRules.Remove(modifiedRule);
            modifiedRule.Result = rule.Result;
            allRules.Add(modifiedRule);
            _ruleRepository.AddRule(allRules);
        }
    }
}
