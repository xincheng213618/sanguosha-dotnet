#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class WushuangCombatHost(Func<RuntimeEntries.CoreWushuangCombatProgramsEntries> getCoreWushuangCombatProgramsRuntime) : IWushuangCombatHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffect e) => getCoreWushuangCombatProgramsRuntime().ExecuteWushuangCombat(f, e);
}
