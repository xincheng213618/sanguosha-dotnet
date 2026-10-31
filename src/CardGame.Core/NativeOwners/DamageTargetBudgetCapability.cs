#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class DamageTargetBudgetCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageTargetBudgetEntries> getCoreGameEngineDamageTargetBudgetRuntime) : IDamageTargetBudgetHost
{
    public SkillProgramStepOutcome ApplyDamageTargetBudget(SkillProgramEffect e, ProgramSkillFrame f) => getCoreGameEngineDamageTargetBudgetRuntime().ApplyDamageTargetBudget(e, f);
}
