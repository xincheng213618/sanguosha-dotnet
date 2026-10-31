#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramFactionPoliciesEntries
{
    internal string FactionPolicyName(global::CardGame.Core.FactionResponsePolicySource source) => _resolve().FactionPolicyName(source);
    internal string FactionProviderDescription(global::CardGame.Core.FactionResponsePolicySource source) => _resolve().FactionProviderDescription(source);
    internal global::System.Collections.Generic.IReadOnlyList<int> GetFactionDefenseCandidateSeats(int ownerSeat) => _resolve().GetFactionDefenseCandidateSeats(ownerSeat);
    internal global::CardGame.Core.FactionResponsePolicySource? GetFactionResponsePolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind requiredKind) => _resolve().GetFactionResponsePolicy(owner, requiredKind);
    internal global::System.Collections.Generic.IReadOnlyList<int> GetFactionSlashCandidateSeats(int ownerSeat) => _resolve().GetFactionSlashCandidateSeats(ownerSeat);
}
