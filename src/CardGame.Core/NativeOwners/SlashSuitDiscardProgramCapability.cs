#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class SlashSuitDiscardProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSlashSuitDiscardEntries> getCoreGameEngineSlashSuitDiscardRuntime) : ISlashSuitDiscardProgramHost
{
    public SkillProgramStepOutcome SuppressCurrentSlashTargetAndJudgeSuitDiscard(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreGameEngineSlashSuitDiscardRuntime().BeginProgramSlashSuitDiscard(frame, effect);
}
