#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HellActivityProgramHost(Func<RuntimeEntries.CoreHellActivityProgramsEntries> getCoreHellActivityProgramsRuntime) : IHellActivityProgramHost
{
    public SkillProgramStepOutcome ExecuteHellActivity(ProgramSkillFrame f, SkillProgramEffectOp op) => getCoreHellActivityProgramsRuntime().ExecuteHellActivity(f, op);
}
