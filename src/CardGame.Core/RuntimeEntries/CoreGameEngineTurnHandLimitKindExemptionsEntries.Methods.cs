#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTurnHandLimitKindExemptionsEntries
{
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.TurnHandLimitCardKindExemption>? GetTurnHandLimitCardKindExemptionsSnapshot(int ownerSeat) => _resolve().GetTurnHandLimitCardKindExemptionsSnapshot(ownerSeat);
    internal void GrantProgramTurnHandLimitCardKindExemption(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind> cardKinds, global::CardGame.Core.SkillProgramDamageModifierExpiration expiration = (global::CardGame.Core.SkillProgramDamageModifierExpiration)0) => _resolve().GrantProgramTurnHandLimitCardKindExemption(frame, cardKinds, expiration);
}
