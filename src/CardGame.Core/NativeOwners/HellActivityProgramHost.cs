#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HellActivityProgramHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreHellActivityProgramsEntries> getCoreHellActivityProgramsRuntime) : IHellActivityProgramHost
{
    public SkillProgramStepOutcome ExecuteHellActivity(ProgramSkillFrame f, SkillProgramEffectOp op) => getCoreHellActivityProgramsRuntime().ExecuteHellActivity(f, op);
}
