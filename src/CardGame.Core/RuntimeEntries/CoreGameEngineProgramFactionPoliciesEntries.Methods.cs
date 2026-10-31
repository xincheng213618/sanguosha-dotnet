#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramFactionPoliciesEntries
{
    internal string FactionPolicyName(FactionResponsePolicySource source) => _resolve().FactionPolicyName(source);
    internal string FactionProviderDescription(FactionResponsePolicySource source) => _resolve().FactionProviderDescription(source);
    internal IReadOnlyList<int> GetFactionDefenseCandidateSeats(int ownerSeat) => _resolve().GetFactionDefenseCandidateSeats(ownerSeat);
    internal FactionResponsePolicySource? GetFactionResponsePolicy(CharacterState owner, CardKind requiredKind) => _resolve().GetFactionResponsePolicy(owner, requiredKind);
    internal IReadOnlyList<int> GetFactionSlashCandidateSeats(int ownerSeat) => _resolve().GetFactionSlashCandidateSeats(ownerSeat);
}
