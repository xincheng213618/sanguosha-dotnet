#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardUseDebitsEntries
{
    internal global::CardGame.Core.CardUseDebitIdentity? GetCardUseDebit(long actionId) => _resolve().GetCardUseDebit(actionId);
    internal bool IsCardUseDebitActive(long actionId) => _resolve().IsCardUseDebitActive(actionId);
    internal void RecordSlashUseDebit(long cardUseFrameId, int actorSeat) => _resolve().RecordSlashUseDebit(cardUseFrameId, actorSeat);
    internal void RefundProgramCardUseDebit(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().RefundProgramCardUseDebit(frame);
    internal void ResetCardUseDebitPhase() => _resolve().ResetCardUseDebitPhase();
    internal bool TryRefundCardUseDebit(global::CardGame.Core.CardUseDebitIdentity identity) => _resolve().TryRefundCardUseDebit(identity);
}
