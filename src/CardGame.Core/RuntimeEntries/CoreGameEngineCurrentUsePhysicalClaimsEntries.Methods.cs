#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCurrentUsePhysicalClaimsEntries
{
    internal SkillProgramStepOutcome ClaimCurrentUsePhysicalCards(ProgramSkillFrame frame) => _resolve().ClaimCurrentUsePhysicalCards(frame);
    internal CardUseFrame CurrentFinalTargetSlashOwner(ProgramSkillFrame frame) => _resolve().CurrentFinalTargetSlashOwner(frame);
    internal void PreventCurrentTargetSlashCancellationByRule(ProgramSkillFrame frame) => _resolve().PreventCurrentTargetSlashCancellationByRule(frame);
}
