#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class DamageSourceHandChoiceCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageSourceHandChoiceEntries> getCoreGameEngineDamageSourceHandChoiceRuntime) : IDamageSourceHandChoiceHost
{
    public SkillProgramStepOutcome ResolveDamageSourceHandChoice(ProgramSkillFrame f) => getCoreGameEngineDamageSourceHandChoiceRuntime().BeginDamageSourceHandChoice(f);
}
