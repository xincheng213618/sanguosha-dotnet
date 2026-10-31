#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class SlashSuitDiscardProgramCapability(Func<RuntimeEntries.CoreGameEngineSlashSuitDiscardEntries> getCoreGameEngineSlashSuitDiscardRuntime) : ISlashSuitDiscardProgramHost
{
    public SkillProgramStepOutcome SuppressCurrentSlashTargetAndJudgeSuitDiscard(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreGameEngineSlashSuitDiscardRuntime().BeginProgramSlashSuitDiscard(frame, effect);
}
