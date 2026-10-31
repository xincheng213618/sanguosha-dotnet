#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class DamageTargetBudgetCapability(Func<RuntimeEntries.CoreGameEngineDamageTargetBudgetEntries> getCoreGameEngineDamageTargetBudgetRuntime) : IDamageTargetBudgetHost
{
    public SkillProgramStepOutcome ApplyDamageTargetBudget(SkillProgramEffect e, ProgramSkillFrame f) => getCoreGameEngineDamageTargetBudgetRuntime().ApplyDamageTargetBudget(e, f);
}
