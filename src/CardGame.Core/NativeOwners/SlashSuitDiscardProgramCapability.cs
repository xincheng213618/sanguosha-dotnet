#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class SlashSuitDiscardProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineSlashSuitDiscardRuntime>> getCoreGameEngineSlashSuitDiscardRuntime) : ISlashSuitDiscardProgramHost
{
    public SkillProgramStepOutcome SuppressCurrentSlashTargetAndJudgeSuitDiscard(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreGameEngineSlashSuitDiscardRuntime().Value.BeginProgramSlashSuitDiscard(frame, effect);
}
