#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AdoptedChildSelectionQuery(EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime) : IAdoptedChildSelectionQuery
{
    public IReadOnlyList<AdoptedChildParticipant> Participants => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Select(player => new AdoptedChildParticipant(player.Seat, player.IsAlive, player.Markers.GetValueOrDefault(PlayerMarkerKind.AdoptedChild) > 0, getCoreGameEngineNationalSkillsRuntime().HasRuntimeSkill(player, "ol:cixiao"))).ToArray());
}
