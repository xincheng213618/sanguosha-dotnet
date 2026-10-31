#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardRecoveryModifiersEntries
{
    internal int ApplyIssuedCardRecoveryModifiers(long useId, int actorSeat, int targetSeat, int baseAmount) => _resolve().ApplyIssuedCardRecoveryModifiers(useId, actorSeat, targetSeat, baseAmount);
    internal void GrantProgramCardRecoveryModifier(ProgramSkillFrame frame, IReadOnlyList<CardKind> cardKinds, int amount, SkillProgramDamageModifierExpiration expiration = (SkillProgramDamageModifierExpiration)0) => _resolve().GrantProgramCardRecoveryModifier(frame, cardKinds, amount, expiration);
    internal void ObserveIssuedCardRecoveryModifiers(IGameEvent payload) => _resolve().ObserveIssuedCardRecoveryModifiers(payload);
}
