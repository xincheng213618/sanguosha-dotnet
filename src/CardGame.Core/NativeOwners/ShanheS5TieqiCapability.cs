#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheS5TieqiCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineSlashSuitDiscardRuntime>> getCoreGameEngineSlashSuitDiscardRuntime) : IShanheS5TieqiProgramHost
{
    public SkillProgramStepOutcome SuppressAndJudgeAllColor(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreGameEngineSlashSuitDiscardRuntime().Value.BeginProgramSlashSuitDiscard(frame, effect);
}
