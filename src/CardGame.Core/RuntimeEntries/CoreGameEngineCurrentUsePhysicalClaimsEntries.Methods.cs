#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCurrentUsePhysicalClaimsEntries
{
    internal SkillProgramStepOutcome ClaimCurrentUsePhysicalCards(ProgramSkillFrame frame) => _resolve().ClaimCurrentUsePhysicalCards(frame);
    internal CardUseFrame CurrentFinalTargetSlashOwner(ProgramSkillFrame frame) => _resolve().CurrentFinalTargetSlashOwner(frame);
    internal bool IsClaimedUseCardEntity(long frameId, int cardId) => _resolve().IsClaimedUseCardEntity(frameId, cardId);
    internal bool IsCurrentUsePhysicalCardClaim(long frameId, int cardId) => _resolve().IsCurrentUsePhysicalCardClaim(frameId, cardId);
    internal void PreventCurrentTargetSlashCancellationByRule(ProgramSkillFrame frame) => _resolve().PreventCurrentTargetSlashCancellationByRule(frame);
}
