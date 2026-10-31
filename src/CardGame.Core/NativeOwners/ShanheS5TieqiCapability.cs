#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheS5TieqiCapability(Func<RuntimeEntries.CoreGameEngineSlashSuitDiscardEntries> getCoreGameEngineSlashSuitDiscardRuntime) : IShanheS5TieqiProgramHost
{
    public SkillProgramStepOutcome SuppressAndJudgeAllColor(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreGameEngineSlashSuitDiscardRuntime().BeginProgramSlashSuitDiscard(frame, effect);
}
