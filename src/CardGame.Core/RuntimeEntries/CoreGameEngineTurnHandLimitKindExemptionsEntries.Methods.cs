#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTurnHandLimitKindExemptionsEntries
{
    internal IReadOnlyList<TurnHandLimitCardKindExemption>? GetTurnHandLimitCardKindExemptionsSnapshot(int ownerSeat) => _resolve().GetTurnHandLimitCardKindExemptionsSnapshot(ownerSeat);
    internal void GrantProgramTurnHandLimitCardKindExemption(ProgramSkillFrame frame, IReadOnlyList<CardKind> cardKinds, SkillProgramDamageModifierExpiration expiration = (SkillProgramDamageModifierExpiration)0) => _resolve().GrantProgramTurnHandLimitCardKindExemption(frame, cardKinds, expiration);
}
