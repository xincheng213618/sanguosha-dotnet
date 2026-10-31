#nullable enable
using System.Text.Json.Serialization;

namespace CardGame.Core.NativeOwners;
internal sealed class WushuangCombatHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreWushuangCombatProgramsEntries> getCoreWushuangCombatProgramsRuntime) : IWushuangCombatHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffect e) => getCoreWushuangCombatProgramsRuntime().ExecuteWushuangCombat(f, e);
}
