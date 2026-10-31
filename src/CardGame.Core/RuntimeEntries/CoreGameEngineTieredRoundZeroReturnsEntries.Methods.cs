#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTieredRoundZeroReturnsEntries
{
    internal bool HasTieredRoundZeroDyingRide(long dyingId) => _resolve().HasTieredRoundZeroDyingRide(dyingId);
    internal bool IsCappedConversionAwaitedMovement(ProgramSkillFrame frame, ProgramMovementContinuation pending) => _resolve().IsCappedConversionAwaitedMovement(frame, pending);
    internal bool IsTieredRoundZeroFireAttackUse(long id) => _resolve().IsTieredRoundZeroFireAttackUse(id);
    internal bool IsTieredRoundZeroProcessingParent(long id, int cardId) => _resolve().IsTieredRoundZeroProcessingParent(id, cardId);
    internal bool IsTieredRoundZeroRescueProgramDying() => _resolve().IsTieredRoundZeroRescueProgramDying();
    internal bool MatchesTieredRoundZeroFireAttackAttack(NativeOwners.CardAttackHandle attack) => _resolve().MatchesTieredRoundZeroFireAttackAttack(attack);
    internal bool MatchesTieredRoundZeroUseAction(long id, CardActionContext action, int cardId) => _resolve().MatchesTieredRoundZeroUseAction(id, action, cardId);
    internal Card ReadTieredRoundUseAppearance(long ownerId, CardAppearanceReference appearance) => _resolve().ReadTieredRoundUseAppearance(ownerId, appearance);
    internal bool SkipTieredRoundZeroFinishedMovement(long id, Card card) => _resolve().SkipTieredRoundZeroFinishedMovement(id, card);
    internal bool TieredRoundZeroDyingRescueRide(int dyingIndex, DyingFrame dying) => _resolve().TieredRoundZeroDyingRescueRide(dyingIndex, dying);
}
