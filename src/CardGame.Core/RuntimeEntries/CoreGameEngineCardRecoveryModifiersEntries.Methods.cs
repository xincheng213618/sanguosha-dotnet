#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardRecoveryModifiersEntries
{
    internal int ApplyIssuedCardRecoveryModifiers(long useId, int actorSeat, int targetSeat, int baseAmount) => _resolve().ApplyIssuedCardRecoveryModifiers(useId, actorSeat, targetSeat, baseAmount);
    internal void GrantProgramCardRecoveryModifier(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind> cardKinds, int amount, global::CardGame.Core.SkillProgramDamageModifierExpiration expiration = (global::CardGame.Core.SkillProgramDamageModifierExpiration)0) => _resolve().GrantProgramCardRecoveryModifier(frame, cardKinds, amount, expiration);
    internal void ObserveIssuedCardRecoveryModifiers(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveIssuedCardRecoveryModifiers(payload);
}
