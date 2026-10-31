using System.Text.Json.Serialization;
using CardGame.Core.EngineOwners;
using CardGame.Core.RuntimeEntries;

namespace CardGame.Core;

public sealed record EquipmentDepartureCandidate(int CardId, ProgramTriggerCandidate Candidate);
public sealed partial record CardMovementBatchContext
{
    private readonly IReadOnlyList<EquipmentDepartureCandidate>? _equipmentDepartures;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<EquipmentDepartureCandidate>? EquipmentDepartures
    { get => _equipmentDepartures; init => _equipmentDepartures = value is null ? null : Array.AsReadOnly(value.ToArray()); }
}
public sealed partial class SkillProgramTrigger
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool OwnEquipmentOnly { get; internal init; }
}

internal interface IEquipmentMovementCapture : IProgramSelectionQuery
{
    IReadOnlyList<EquipmentDepartureCandidate>? Capture(IEnumerable<CardLocation> sources);
}

/// <summary>Freezes only the departing equipment's effective trigger, then authorizes its own movement window.</summary>
[ProgramRuntimeModule, ProgramCapability]
internal sealed class EquipmentMovementRuntime(ICardZoneQueries cards, IMatchParticipantStateAccess players,
    IProgramContentQueries content, CoreGameEngineProgramLifecycleEntries bindings,
    DeferredRuntime<CoreGameEngineEquipmentContentRuntime> equipment) : IEquipmentMovementCapture
{
    public IReadOnlyList<EquipmentDepartureCandidate>? Capture(IEnumerable<CardLocation> sources)
    {
        var result = new List<EquipmentDepartureCandidate>();
        foreach (var source in sources.Where(s => s.Zone == CardZoneKind.Equipment && s.OwnerSeat is not null))
            foreach (var card in cards.CardsAt(source))
            {
                var skill = equipment.Value.EquipmentRules.GetSkillBinding(card.Kind);
                if (skill is null || content.FindSkill(skill.SkillId)?.Program?.Triggers.Any(t =>
                    t.OwnEquipmentOnly && t.Window == SkillProgramTriggerWindow.CardsMoved) != true) continue;
                result.AddRange(bindings.CollectProgramTriggerCandidates(players.At(source.OwnerSeat!.Value), SkillProgramTriggerWindow.CardsMoved)
                    .Where(c => c.SkillId == skill.SkillId && c.SkillInstanceId == skill.CreateGrantId(card.Id) &&
                        bindings.GetProgramTrigger2(c).OwnEquipmentOnly).Select(c => new EquipmentDepartureCandidate(card.Id, c)));
            }
        return result.Count == 0 ? null : Array.AsReadOnly(result.ToArray());
    }
    internal IEnumerable<ProgramTriggerCandidate> Departures(CardMovementBatchContext batch, int seat) =>
        (batch.EquipmentDepartures ?? []).Where(d => d.Candidate.OwnerSeat == seat && batch.Movements.Any(m =>
            m.CardId == d.CardId && m.From == CardLocation.Equipment(seat) && m.From != m.To)).Select(d => d.Candidate);

    internal bool MatchesOwnCard(ProgramTriggerCandidate candidate, CardMovementRecord movement) =>
        equipment.Value.EquipmentRules.GetSkillBinding(movement.CardKind) is { } binding &&
        binding.SkillId == candidate.SkillId && binding.CreateGrantId(movement.CardId) == candidate.SkillInstanceId;

    internal bool IsDeparture(CardMovementBatchContext batch, ProgramTriggerCandidate candidate) =>
        batch.EquipmentDepartures?.Any(d => d.Candidate == candidate with { OccurrenceIndex = d.Candidate.OccurrenceIndex } &&
            candidate.OccurrenceIndex >= 0 && candidate.OccurrenceIndex < batch.Movements.Count &&
            batch.Movements[candidate.OccurrenceIndex].CardId == d.CardId &&
            batch.Movements[candidate.OccurrenceIndex].From == CardLocation.Equipment(candidate.OwnerSeat)) == true;

    internal static void ValidateTrigger(string path, SkillProgramTrigger trigger)
    {
        if (!trigger.OwnEquipmentOnly) return;
        if (trigger.MovementOccurrence != SkillProgramMovementOccurrence.PerCard ||
            !(trigger.Window == SkillProgramTriggerWindow.CardsMoved && trigger.SourceZones.SequenceEqual([CardZoneKind.Equipment]) ||
              trigger.Window == SkillProgramTriggerWindow.CardsGained && trigger.DestinationZones.SequenceEqual([CardZoneKind.Equipment])))
            throw new InvalidOperationException(path + ": ownEquipmentOnly requires a per-card equipment entry or departure window.");
    }
}
