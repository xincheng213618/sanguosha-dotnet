#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheS5XuncanProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheS5XuncanEntries> getCoreGameEngineShanheS5XuncanRuntime) : IShanheS5XuncanHost
{
    public SkillProgramStepOutcome ResolveShanheS5Xuncan(ProgramSkillFrame f, SkillProgramEffect e) => getCoreGameEngineShanheS5XuncanRuntime().ResolveShanheS5Xuncan(f, e);
}
