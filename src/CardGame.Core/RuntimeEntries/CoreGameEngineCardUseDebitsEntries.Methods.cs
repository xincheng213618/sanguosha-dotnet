#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardUseDebitsEntries
{
    internal CardUseDebitIdentity? GetCardUseDebit(long actionId) => _resolve().GetCardUseDebit(actionId);
    internal bool IsCardUseDebitActive(long actionId) => _resolve().IsCardUseDebitActive(actionId);
    internal void RecordSlashUseDebit(long cardUseFrameId, int actorSeat) => _resolve().RecordSlashUseDebit(cardUseFrameId, actorSeat);
    internal void RefundProgramCardUseDebit(ProgramSkillFrame frame) => _resolve().RefundProgramCardUseDebit(frame);
    internal void ResetCardUseDebitPhase() => _resolve().ResetCardUseDebitPhase();
    internal bool TryRefundCardUseDebit(CardUseDebitIdentity identity) => _resolve().TryRefundCardUseDebit(identity);
}
