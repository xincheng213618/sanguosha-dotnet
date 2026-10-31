using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record CardEntityMarkedEvent(long FrameId, int CardId, string Mark, int? InitialOwnerSeat = null) : IGameEvent;
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
    void MarkInitialHand(ProgramSkillFrame frame, string mark);
    void MarkCards(ProgramSkillFrame frame, string bind, string mark);
}

[ProgramRuntimeModule, ProgramCapability]
internal sealed class CardEntityMarkRuntime(IProgramEventHistoryQueries history, ICardZoneQueries cards,
    IProgramRuntimeEvents events, IProgramRuntimeFrames frames, IProgramContentQueries content)
    : ICardEntityMarkQueries, ICardEntityMarkOperations
{
    public bool Has(int cardId, string mark) => history.Read<CardEntityMarkedEvent>().Any(fact => fact.CardId == cardId && fact.Mark == mark);
    public IReadOnlyList<(string Mark, string Label)> MarkedNames(int cardId) => Array.AsReadOnly(history.Read<CardEntityMarkedEvent>()
        .Where(fact => fact.CardId == cardId).Select(fact => fact.Mark).Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal)
        .Select(mark => (Mark: mark, Label: content.CardMarkLabel(mark))).Where(value => value.Label is not null)
        .Select(value => (value.Mark, value.Label!)).ToArray());
    public int? InitialOwnerSeat(int cardId, string mark) => history.Read<CardEntityMarkedEvent>()
        .FirstOrDefault(fact => fact.CardId == cardId && fact.Mark == mark && fact.InitialOwnerSeat is not null)?.InitialOwnerSeat;
    public IReadOnlyList<int> MarkedCardIds(string mark) => Array.AsReadOnly(history.Read<CardEntityMarkedEvent>()
        .Where(fact => fact.Mark == mark).Select(fact => fact.CardId).Distinct().ToArray());
    public int CountMarked(IEnumerable<int> cardIds, string mark) => cardIds.Count(cardId => Has(cardId, mark));
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

    public void MarkInitialHand(ProgramSkillFrame frame, string mark)
    {
        if (OpeningParent(frame.WindowContext) is not { OpeningHandEntities: { } opening } parent ||
            !MatchesCurrentCandidate(frame, parent))
            throw new InvalidOperationException("Initial hand marks require their issued opening window.");
        Mark(frame.Id, mark, opening.Where(entity => entity.OwnerSeat == frame.OwnerSeat).Select(entity => entity.CardId).ToArray(), frame.OwnerSeat);
    }

    public void MarkCards(ProgramSkillFrame frame, string bind, string mark) =>
        Mark(frame.Id, mark, frames.ActiveProgram(frame.Id).CardSetBindings.Single(binding => binding.Name == bind).CardIds);

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
        reader.AllowOnly("op", "target", "mark", "bind", "condition");
        if (reader.Has("target") && reader.RequiredEnum<SkillProgramEffectTarget>("target") != SkillProgramEffectTarget.Owner ||
            (op == SkillProgramEffectOp.MarkCards) != reader.Has("bind"))
            throw new InvalidOperationException($"Invalid card mark operation at {reader.Path}.");
        return new(op, SkillProgramEffectTarget.Owner, 0, reader.Condition(),
            sourceBind: op == SkillProgramEffectOp.MarkCards ? reader.RequiredIdentifier("bind") : null)
            { CardMark = reader.RequiredIdentifier("mark") };
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        op == SkillProgramEffectOp.MarkInitialHand ? [new RequireTriggerWindow(SkillProgramTriggerWindow.GameStarting)] : [new ReadCardSet(effect.SourceBind!)];
    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext context) =>
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
        else marks.MarkCards(frame, effect.SourceBind!, effect.CardMark!);
        return SkillProgramStepOutcome.Continue;
    }
}
