namespace CardGame.Core;

public sealed partial class GameEngine
{
internal sealed record AttackCompletionReceipt(long ResolutionId, bool DelayedTurn,
        long? ProgramFrameId, int? DelayedTurnSeat,
        bool RequestedSlashChild = false, bool DamageWasApplied = false, int FinalTargetSeat = 0, ProgramDamageTargetDuelOrigin? DamageTargetDuelReturn = null, ProgramDualColorDuelOrigin? DualColorDuelReturn = null,
        SharedSlashBenefitReturn? SharedSlashBenefit = null, bool SharedSlashCausedDamage = false,
        ForeignTurnContestSlashReturn? ForeignTurnContestSlashReturn = null, PindianWinnerSlashReturn? PindianWinnerSlashReturn = null, ConditionalDiscardDuelOrigin? ConditionalDiscardDuelReturn = null, ShiQuanDuelOrigin? ShiQuanDuelReturn = null, EndingHistoricalUseReturn? EndingHistoricalUseReturn = null, InspectedHandSlashReturn? InspectedHandSlashReturn = null, KuangfuSlashReturn? KuangfuSlashReturn = null, ForeignSelectedCardSlashReturn? ForeignSelectedCardSlashReturn = null, FriendRecastDuelOrigin? FriendRecastDuelReturn = null, CaoChunNativeSlash? CaoChunSlash = null, WangCanNativeReceipt? DenglouNative = null, QigongNativeReceipt? Qigong = null, ZhenyingDuelOrigin? ZhenyingDuel = null, HeJinMouzhuUseOrigin? HeJinMouzhu = null, WuAnGuoLiyongDuelOrigin? WuAnGuoLiyong = null);
internal interface ICardContinuationHandle { long OwnerFrameId { get; } }

}
