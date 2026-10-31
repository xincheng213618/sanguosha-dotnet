#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramFactionPoliciesRuntime
{
    internal FactionResponsePolicySource? GetFactionResponsePolicy(CharacterState owner, CardKind requiredKind) => _characterGuanYuRuntimeOperations.GetDemonDodgeRequest(owner, requiredKind) ?? _coreProgramCardPoliciesOperations.CardPolicies(owner, SkillProgramCardPolicyKind.FactionResponseRequest, requiredKind: requiredKind).Where(item => _coreNativeEquipmentAndFactionQueriesOperations.GetFactionProviderSeats(owner.Seat, item.Policy.FactionId!).Count > 0).Where(item => item.Policy.DiscardCost == 0 || _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment(owner).Count >= item.Policy.DiscardCost).Select(item => new FactionResponsePolicySource(item.Source.SkillId, item.Source.SkillInstanceId, item.Policy.Id, item.Policy.FactionId!, requiredKind, item.Policy.DiscardCost, item.Policy.ProviderDrawCount)).FirstOrDefault();
    internal string FactionPolicyName(FactionResponsePolicySource source) => _contentRegistryOperations._contentRegistry!.GetSkill(source.SkillId).Name;
    internal string FactionProviderDescription(FactionResponsePolicySource source) => source.FactionId == "magic-any" ? "其他角色" : $"其他{(CoreMatchPresentationRuntime.GetFactionName(source.FactionId))}势力角色";
    internal IReadOnlyList<int> GetFactionDefenseCandidateSeats(int ownerSeat) => GetFactionResponsePolicy(_coreMatchParticipantsPlayersAccess._players[ownerSeat], CardKind.Dodge)is { } source ? _coreNativeEquipmentAndFactionQueriesOperations.GetFactionProviderSeats(ownerSeat, source.FactionId) : [];
    internal IReadOnlyList<int> GetFactionSlashCandidateSeats(int ownerSeat) => GetFactionResponsePolicy(_coreMatchParticipantsPlayersAccess._players[ownerSeat], CardKind.Slash)is { } source ? _coreNativeEquipmentAndFactionQueriesOperations.GetFactionProviderSeats(ownerSeat, source.FactionId) : [];
}
