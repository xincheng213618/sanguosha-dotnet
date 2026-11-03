using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record CardActionProhibitionExpiredEvent(long GrantSequence, SkillStateBoundary Boundary) : IGameEvent;

public sealed class CardActionProhibitionOptions(IReadOnlyList<SkillStateBoundarySpec> until, int? physicalMaterialCount)
{
    public IReadOnlyList<SkillStateBoundarySpec> Until { get; } = Array.AsReadOnly(until.ToArray());
    public int? PhysicalMaterialCount { get; } = physicalMaterialCount;
}

public sealed partial class SkillProgramEffect
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public CardActionProhibitionOptions? CardActionProhibition { get; internal init; }
}

internal interface IAppliedCardActionRestrictions : IProgramRuntimeOperations
{
    bool IsMaterialUseForbidden(int actorSeat, IReadOnlyList<int> physicalCardIds, Suit? finalEffectiveSuit);
    TurnCardActionProhibition GrantNextActualUseSuit(ProgramSkillFrame frame, int recipientSeat, Suit suit);
    TurnCardActionProhibition Grant(ProgramSkillFrame frame, int recipientSeat, SkillProgramEffect effect);
}

[ProgramRuntimeModule, ProgramCapability]
internal sealed class AppliedCardActionRestrictionsRuntime(EngineOwners.IKernelTurnCardUseEffectsAccess effects,
    IProgramTurnQueries turn, IProgramRuleLifetimes lifetimes,
    IProgramRuntimeFrames frames, IProgramRuntimeEvents events) : IAppliedCardActionRestrictions
{
    private readonly IReadOnlyList<SkillStateBoundarySpec> NextUseUntil = Array.AsReadOnly(new[]
    {
        new SkillStateBoundarySpec(SkillStateBoundaryEvent.ActualUse, SkillStateSubject.Target,
            SkillStateBoundaryTiming.Before, SkipIssued: true),
        new SkillStateBoundarySpec(SkillStateBoundaryEvent.ActualTurnReturned, SkillStateSubject.TurnOwner,
            SkillStateBoundaryTiming.After)
    });

    public bool IsMaterialUseForbidden(int actorSeat, IReadOnlyList<int> physicalCardIds, Suit? finalEffectiveSuit)
    {
        if (finalEffectiveSuit is null or Suit.None) return false;
        var count = physicalCardIds.Count;
        return effects._turnCardUseEffects.ActionProhibitions.Any(item =>
            (item.Lifetime is not null || item.TurnNumber == turn.TurnNumber && item.TurnSeat == turn.TurnOwnerSeat) &&
            item.AffectedSeat == actorSeat && item.CardKinds.Count == 0 && item.ActionTypes.Contains(CardActionType.Use) &&
            item.Suits?.Contains(finalEffectiveSuit.Value) == true &&
            (item.PhysicalMaterialCount is null || item.PhysicalMaterialCount == count));
    }

    public TurnCardActionProhibition GrantNextActualUseSuit(ProgramSkillFrame frame, int recipientSeat, Suit suit)
    {
        if (suit == Suit.None) throw new InvalidOperationException("A next-use suit restriction requires a chromatic suit.");
        return Grant(frame, recipientSeat, new SkillProgramEffect(SkillProgramEffectOp.GrantTurnCardActionProhibition,
            SkillProgramEffectTarget.Owner, 0, new(SkillProgramConditionKind.Always, 0, []),
            suits: [suit], actionTypes: [CardActionType.Use])
        { CardActionProhibition = new(NextUseUntil, 1) });
    }

    public TurnCardActionProhibition Grant(ProgramSkillFrame supplied, int recipientSeat, SkillProgramEffect effect)
    {
        var frame = frames.ActiveProgram(supplied.Id);
        var options = effect.CardActionProhibition ?? throw new InvalidOperationException("An applied action restriction requires its declared lifetime.");
        var prior = effects._turnCardUseEffects.ActionProhibitions.SingleOrDefault(item =>
            item.ParentFrameId == frame.Id && item.EffectIndex == frame.InstructionIndex - 1 && item.AffectedSeat == recipientSeat);
        var lifetime = lifetimes.CreateApplicationLifetime(frame.OwnerSeat, recipientSeat, options.Until,
            new(frame.OwnerSeat, recipientSeat, IssuedTurnNumber: turn.TurnNumber))
            ?? throw new InvalidOperationException("An applied action restriction requires a valid recipient.");
        // Reentering one issued instruction reuses its first cutoff; all other
        // boundary fields remain part of the store's immutable grant contract.
        if (prior?.Lifetime is { } issued && issued.Until.Count == lifetime.Until.Count)
            lifetime = new(lifetime.Origin, lifetime.Until.Select((end, index) =>
                end with { IssuedOccurrence = issued.Until[index].IssuedOccurrence }).ToArray());
        var granted = effects._turnCardUseEffects.GrantActionProhibition(turn.TurnNumber, turn.TurnOwnerSeat,
            frame.Id, frame.InstructionIndex - 1,
            new(frame.SkillId, frame.TriggerId ?? frame.ActivationId, frame.OwnerSeat, frame.SkillInstanceId),
            effect.CardKinds, effect.ActionTypes, effect.Suits.Count == 0 ? null : effect.Suits,
            recipientSeat, options.PhysicalMaterialCount, lifetime);
        if (prior is null) events.AdvanceFact(new CardActionProhibitionGrantedEvent(granted));
        return granted;
    }

}
