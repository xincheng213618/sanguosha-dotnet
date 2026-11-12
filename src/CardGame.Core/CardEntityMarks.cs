using System.Text.Json.Serialization;

namespace CardGame.Core;

public readonly record struct CardEntityMarkChange(int CardId, string Mark, bool Marked);

public interface ICardEntityMarkFact : IGameEvent
{
    int ChangeCount { get; }
    CardEntityMarkChange ChangeAt(int index);
}

public sealed record CardEntityMarkedEvent(long FrameId, int CardId, string Mark, int? InitialOwnerSeat = null) : ICardEntityMarkFact
{
    int ICardEntityMarkFact.ChangeCount => 1;
    CardEntityMarkChange ICardEntityMarkFact.ChangeAt(int index) => index == 0
        ? new(CardId, Mark, true) : throw new ArgumentOutOfRangeException(nameof(index));
}

public sealed record CardEntityMarksChangedEvent : ICardEntityMarkFact
{
    public CardEntityMarksChangedEvent(long frameId, IReadOnlyList<CardEntityMarkChange> changes)
    { FrameId = frameId; Changes = changes; }

    public long FrameId { get; init; }
    private readonly IReadOnlyList<CardEntityMarkChange> _changes = [];
    public IReadOnlyList<CardEntityMarkChange> Changes
    { get => _changes; init => _changes = Array.AsReadOnly(value.ToArray()); }
    int ICardEntityMarkFact.ChangeCount => Changes.Count;
    CardEntityMarkChange ICardEntityMarkFact.ChangeAt(int index) => Changes[index];
}
public sealed record OpeningHandEntity(int OwnerSeat, int CardId, CardKind PrintedKind, long EntrySequence);

public sealed partial record ProgramLifecycleTriggerWindowFrame
{
    private readonly IReadOnlyList<OpeningHandEntity>? _openingHandEntities;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<OpeningHandEntity>? OpeningHandEntities
    {
        get => _openingHandEntities;
        init => _openingHandEntities = value is null ? null : Array.AsReadOnly(value.ToArray());
    }
}

public sealed partial class SkillProgramEffect
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? CardMark { get; internal init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? CardMarkValue { get; internal init; }
}

internal interface ICardEntityMarkQueries : IProgramSelectionQuery
{
    bool Has(int cardId, string mark);
    int? InitialOwnerSeat(int cardId, string mark);
    IReadOnlyList<int> MarkedCardIds(string mark);
    int CountMarked(IEnumerable<int> cardIds, string mark);
    int CountMarked(int seat, string mark, CardZoneKind zone);
    IReadOnlyList<(string Mark, string Label)> MarkedNames(int cardId);
}

internal interface ICardEntityMarkOperations : IProgramRuntimeOperations
{
    void Mark(long frameId, string mark, IReadOnlyList<int> cardIds, int? initialOwnerSeat = null);
    void Set(long frameId, IReadOnlyList<CardEntityMarkChange> changes);
    void MarkInitialHand(ProgramSkillFrame frame, string mark);
    void MarkCards(ProgramSkillFrame frame, string bind, string mark, bool? marked = null);
}

[ProgramRuntimeModule, ProgramCapability]
internal sealed class CardEntityMarkRuntime(IProgramEventHistoryQueries history, ICardZoneQueries cards,
    IProgramRuntimeEvents events, IProgramRuntimeFrames frames, IProgramContentQueries content)
    : ICardEntityMarkQueries, ICardEntityMarkOperations
{
    public bool Has(int cardId, string mark)
    {
        var facts = history.Read<ICardEntityMarkFact>();
        for (var index = facts.Count - 1; index >= 0; index--)
            for (var changeIndex = facts[index].ChangeCount - 1; changeIndex >= 0; changeIndex--)
            {
                var change = facts[index].ChangeAt(changeIndex);
                if (change.CardId == cardId && change.Mark == mark) return change.Marked;
            }
        return false;
    }
    public IReadOnlyList<(string Mark, string Label)> MarkedNames(int cardId) => Array.AsReadOnly(Changes()
        .Where(change => change.CardId == cardId).GroupBy(change => change.Mark, StringComparer.Ordinal)
        .Where(group => group.Last().Marked).Select(group => group.Key).Order(StringComparer.Ordinal)
        .Select(mark => (Mark: mark, Label: content.CardMarkLabel(mark))).Where(value => value.Label is not null)
        .Select(value => (value.Mark, value.Label!)).ToArray());
    public int? InitialOwnerSeat(int cardId, string mark) => history.Read<CardEntityMarkedEvent>()
        .FirstOrDefault(fact => fact.CardId == cardId && fact.Mark == mark && fact.InitialOwnerSeat is not null)?.InitialOwnerSeat;
    public IReadOnlyList<int> MarkedCardIds(string mark) => Array.AsReadOnly(Changes()
        .Where(change => change.Mark == mark).GroupBy(change => change.CardId)
        .Where(group => group.Last().Marked).Select(group => group.Key).ToArray());
    public int CountMarked(IEnumerable<int> cardIds, string mark)
    {
        var marked = MarkedCardIds(mark).ToHashSet();
        return cardIds.Count(marked.Contains);
    }
    public int CountMarked(int seat, string mark, CardZoneKind zone) => CountMarked(cards.CardsAt(zone switch
    {
        CardZoneKind.Hand => CardLocation.Hand(seat),
        CardZoneKind.Equipment => CardLocation.Equipment(seat),
        _ => throw new ArgumentOutOfRangeException(nameof(zone))
    }).Select(card => card.Id), mark);

    public void Mark(long frameId, string mark, IReadOnlyList<int> cardIds, int? initialOwnerSeat = null)
    {
        foreach (var cardId in cardIds)
            if (!Has(cardId, mark) || initialOwnerSeat is not null && InitialOwnerSeat(cardId, mark) is null)
                events.AdvanceFact(new CardEntityMarkedEvent(frameId, cardId, mark, initialOwnerSeat));
    }

    public void Set(long frameId, IReadOnlyList<CardEntityMarkChange> changes)
    {
        if (frameId <= 0 || changes.Any(change => change.CardId <= 0 || string.IsNullOrWhiteSpace(change.Mark)) ||
            changes.Select(change => (change.CardId, change.Mark)).Distinct().Count() != changes.Count)
            throw new InvalidOperationException("A physical mark change requires its actual frame and distinct entity/mark pairs.");
        var changed = changes.Where(change => Has(change.CardId, change.Mark) != change.Marked).ToArray();
        if (changed.Length > 0) events.AdvanceFact(new CardEntityMarksChangedEvent(frameId, changed));
    }

    private IEnumerable<CardEntityMarkChange> Changes()
    {
        foreach (var fact in history.Read<ICardEntityMarkFact>())
            for (var index = 0; index < fact.ChangeCount; index++)
                yield return fact.ChangeAt(index);
    }

    public void MarkInitialHand(ProgramSkillFrame frame, string mark)
    {
        if (OpeningParent(frame.WindowContext) is not { OpeningHandEntities: { } opening } parent ||
            !MatchesCurrentCandidate(frame, parent))
            throw new InvalidOperationException("Initial hand marks require their issued opening window.");
        Mark(frame.Id, mark, opening.Where(entity => entity.OwnerSeat == frame.OwnerSeat).Select(entity => entity.CardId).ToArray(), frame.OwnerSeat);
    }

    public void MarkCards(ProgramSkillFrame frame, string bind, string mark, bool? marked = null)
    {
        var ids = frames.ActiveProgram(frame.Id).CardSetBindings.Single(binding => binding.Name == bind).CardIds;
        if (marked is { } value) Set(frame.Id, ids.Select(id => new CardEntityMarkChange(id, mark, value)).ToArray());
        else Mark(frame.Id, mark, ids);
    }

    [ProgramIssuedTrigger(900005)]
    internal bool IsIssuedOpeningMark(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) =>
        OpeningParent(context) is { } parent && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count &&
        parent.Candidates[parent.CandidateIndex] == candidate && context.OwnerSeat == candidate.OwnerSeat &&
        content.Skill(candidate.SkillId).Program is { } program && program.GameplayHash == candidate.GameplayHash &&
        program.Triggers.Any(trigger => trigger.Id == candidate.BindingId && trigger.Effects is [{ Op: SkillProgramEffectOp.MarkInitialHand }]);

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 900005)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 900005)]
    internal bool RetainOpeningMark(ProgramSkillFrame frame) =>
        frame.InstructionIndex is 0 or 1 && OpeningParent(frame.WindowContext) is { } parent && MatchesCurrentCandidate(frame, parent) &&
        IsIssuedOpeningMark(parent.Candidates[parent.CandidateIndex], frame.WindowContext!);

    private ProgramLifecycleTriggerWindowFrame? OpeningParent(ProgramSkillWindowContext? context) =>
        context is { Window: SkillProgramTriggerWindow.GameStarting } && frames.Find(context.ParentFrameId) is
            ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.GameStarting,
                Continuation: ProgramLifecycleContinuation.CompleteGameStarting, OpeningHandEntities: not null } parent ? parent : null;

    private static bool MatchesCurrentCandidate(ProgramSkillFrame frame, ProgramLifecycleTriggerWindowFrame parent) =>
        parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count && parent.Candidates[parent.CandidateIndex] is { } candidate &&
        candidate.OwnerSeat == frame.OwnerSeat && candidate.SkillId == frame.SkillId && candidate.BindingId == frame.TriggerId &&
        candidate.SkillInstanceId == frame.SkillInstanceId && candidate.GameplayHash == frame.GameplayHash;
}

internal abstract class CardEntityMarkDescriptor(SkillProgramEffectOp op) : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => op;
    public override ISkillProgramEffectHandler Handler { get; } = new CardEntityMarkHandler(op);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.CaptureSelectedCards, static (_, _) => { });
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)
    {
        reader.AllowOnly("op", "target", "mark", "bind", "marked", "condition");
        if (reader.Has("target") && reader.RequiredEnum<SkillProgramEffectTarget>("target") != SkillProgramEffectTarget.Owner ||
            (op == SkillProgramEffectOp.MarkCards) != reader.Has("bind") ||
            reader.Has("marked") && op != SkillProgramEffectOp.MarkCards)
            throw new InvalidOperationException($"Invalid card mark operation at {reader.Path}.");
        return new(op, SkillProgramEffectTarget.Owner, 0, reader.Condition(),
            sourceBind: op == SkillProgramEffectOp.MarkCards ? reader.RequiredIdentifier("bind") : null)
            { CardMark = reader.RequiredIdentifier("mark"), CardMarkValue = reader.Has("marked") ? reader.RequiredBool("marked") : null };
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        op == SkillProgramEffectOp.MarkInitialHand ? [new RequireTriggerWindow(SkillProgramTriggerWindow.GameStarting)] : [new ReadCardSet(effect.SourceBind!)];
    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext context) =>
        op == SkillProgramEffectOp.MarkCards ||
        op == SkillProgramEffectOp.MarkInitialHand && context.Frame.WindowContext?.Window == SkillProgramTriggerWindow.GameStarting;
}
internal sealed class MarkInitialHandDescriptor() : CardEntityMarkDescriptor(SkillProgramEffectOp.MarkInitialHand);
internal sealed class MarkCardsDescriptor() : CardEntityMarkDescriptor(SkillProgramEffectOp.MarkCards);
internal sealed class CardEntityMarkHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host)
    {
        var marks = host.GetCapability<ICardEntityMarkOperations>();
        if (op == SkillProgramEffectOp.MarkInitialHand) marks.MarkInitialHand(frame, effect.CardMark!);
        else marks.MarkCards(frame, effect.SourceBind!, effect.CardMark!, effect.CardMarkValue);
        return SkillProgramStepOutcome.Continue;
    }
}
