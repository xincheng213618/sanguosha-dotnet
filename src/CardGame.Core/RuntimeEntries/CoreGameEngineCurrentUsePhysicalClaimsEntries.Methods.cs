#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCurrentUsePhysicalClaimsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome ClaimCurrentUsePhysicalCards(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ClaimCurrentUsePhysicalCards(frame);
    internal global::CardGame.Core.CardUseFrame CurrentFinalTargetSlashOwner(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CurrentFinalTargetSlashOwner(frame);
    internal bool IsClaimedUseCardEntity(long frameId, int cardId) => _resolve().IsClaimedUseCardEntity(frameId, cardId);
    internal bool IsCurrentUsePhysicalCardClaim(long frameId, int cardId) => _resolve().IsCurrentUsePhysicalCardClaim(frameId, cardId);
    internal void PreventCurrentTargetSlashCancellationByRule(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PreventCurrentTargetSlashCancellationByRule(frame);
}
