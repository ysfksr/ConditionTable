using Autofac;
using ConditionTable.Abstracts;
using ConditionTable.Repository;
using ConditionTable.Service;


namespace ConditionTable.DependencyResolvers
{
    public class AutofacBusinessModule
    {
        public class AutoFacBusinessModule : Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                builder.RegisterType<RuleService>().As<IRuleService>();
                builder.RegisterType<RuleRepository>().As<IRuleRepository>();
            }
        }
    }
}
