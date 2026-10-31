#nullable enable
using System.Text.Json.Serialization;

namespace CardGame.Core.NativeOwners;
internal sealed class WenheActivityProgramHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreWenheActivityProgramsEntries> getCoreWenheActivityProgramsRuntime) : IWenheActivityProgramHost
{
    public SkillProgramStepOutcome ExecuteWenheActivity(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCoreWenheActivityProgramsRuntime().ExecuteWenheActivity(frame, op);
}
