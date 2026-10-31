#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTieredRoundZeroReturnsEntries
{
    internal bool HasTieredRoundZeroDyingRide(long dyingId) => _resolve().HasTieredRoundZeroDyingRide(dyingId);
    internal bool IsCappedConversionAwaitedMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsCappedConversionAwaitedMovement(frame, pending);
    internal bool IsTieredRoundZeroFireAttackUse(long id) => _resolve().IsTieredRoundZeroFireAttackUse(id);
    internal bool IsTieredRoundZeroProcessingParent(long id, int cardId) => _resolve().IsTieredRoundZeroProcessingParent(id, cardId);
    internal bool IsTieredRoundZeroRescueProgramDying() => _resolve().IsTieredRoundZeroRescueProgramDying();
    internal bool MatchesTieredRoundZeroFireAttackAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().MatchesTieredRoundZeroFireAttackAttack(attack);
    internal bool MatchesTieredRoundZeroUseAction(long id, global::CardGame.Core.CardActionContext action, int cardId) => _resolve().MatchesTieredRoundZeroUseAction(id, action, cardId);
    internal global::CardGame.Core.Card ReadTieredRoundUseAppearance(long ownerId, global::CardGame.Core.CardAppearanceReference appearance) => _resolve().ReadTieredRoundUseAppearance(ownerId, appearance);
    internal bool SkipTieredRoundZeroFinishedMovement(long id, global::CardGame.Core.Card card) => _resolve().SkipTieredRoundZeroFinishedMovement(id, card);
    internal bool TieredRoundZeroDyingRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => _resolve().TieredRoundZeroDyingRescueRide(dyingIndex, dying);
}
