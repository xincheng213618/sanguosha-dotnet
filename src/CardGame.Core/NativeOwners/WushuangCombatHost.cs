#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class WushuangCombatHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreWushuangCombatProgramsRuntime>> getCoreWushuangCombatProgramsRuntime) : IWushuangCombatHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffect e) => getCoreWushuangCombatProgramsRuntime().Value.ExecuteWushuangCombat(f, e);
}
