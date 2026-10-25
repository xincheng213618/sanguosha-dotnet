using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record MobileSpXuShuJianyanMovementReceipt(string Kind, long BatchId, long Before, long After, IReadOnlyList<CardMovementRecord> Movements)
{
    private readonly IReadOnlyList<CardMovementRecord> _movements = Array.AsReadOnly(Movements.ToArray());
    public IReadOnlyList<CardMovementRecord> Movements { get => _movements; init => _movements = Array.AsReadOnly(value.ToArray()); }
}
public sealed record MobileSpXuShuJianyanReceipt(string Stage, string? Criterion, IReadOnlyList<int> RevealedIds,
    int? MatchedCardId = null, int? RecipientSeat = null)
{
    private readonly IReadOnlyList<int> _revealed = Array.AsReadOnly(RevealedIds.ToArray());
    public IReadOnlyList<int> RevealedIds { get => _revealed; init => _revealed = Array.AsReadOnly(value.ToArray()); }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MobileSpXuShuJianyanMovementReceipt? Movement { get; init; }
}
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MobileSpXuShuJianyanReceipt? MobileSpXuShuJianyan { get; init; }
}
public sealed record MobileSpXuShuJianyanRevealedEvent(long FrameId, string Criterion, int CardId, bool Matched, long MovementSequence) : IGameEvent;
public sealed record MobileSpXuShuJianyanGiftPaidEvent(long FrameId, int RecipientSeat, int CardId, long MovementSequence) : IGameEvent;
public sealed record MobileSpXuShuJianyanCleanupPaidEvent(long FrameId, int Count, long Before, long After) : IGameEvent;
public sealed record MobileSpXuShuJianyanNativeBatchIssuedEvent(long FrameId, string Kind, long BatchId, long ParentFrameId,
    long AwaitingProgramFrameId, int OwnerSeat, string SkillId, string SkillInstanceId, long Before, long After, int Count) : IGameEvent;
internal interface IMobileSpXuShuJianyanHost { SkillProgramStepOutcome Begin(ProgramSkillFrame f); }
internal sealed class ResolveMobileSpXuShuJianyanDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveMobileSpXuShuJianyan;
    public override ISkillProgramEffectHandler Handler { get; } = new MobileSpXuShuJianyanHandler();
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, c) => c.PublicControlValue(2));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new CreateCardSet("mobile-jianyan-revealed", int.MaxValue, false)];
}
internal sealed class MobileSpXuShuJianyanHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveMobileSpXuShuJianyan;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int target, ISkillProgramEffectHost host) => host.GetCapability<IMobileSpXuShuJianyanHost>().Begin(f);
}

public sealed partial class GameEngine
{
    private const string MobileSpXuShuJianyanSkill = "mobile:jianyan", MobileSpXuShuJianyanBind = "mobile-jianyan-revealed";
    private const string MobileSpXuShuJianyanRevealReason = "skill-program.mobile:jianyan.reveal", MobileSpXuShuJianyanGiftReason = "skill-program.mobile:jianyan.obtain",
        MobileSpXuShuJianyanCleanupReason = "skill-program.mobile:jianyan.unmatched";
    [ProgramCapability]
    private sealed class MobileSpXuShuJianyanHost(GameEngine engine) : IMobileSpXuShuJianyanHost
    { public SkillProgramStepOutcome Begin(ProgramSkillFrame f) => engine.BeginMobileSpXuShuJianyan(f); }
    private SkillProgramStepOutcome BeginMobileSpXuShuJianyan(ProgramSkillFrame f)
    {
        if (f.SkillId != MobileSpXuShuJianyanSkill || f.TriggerId is not null || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.MobileSpXuShuJianyan is not null)
            throw new InvalidOperationException("Mobile Jianyan starts on its one original zero-input Play activation.");
        ReplaceRuntimeTop(f = f with { MobileSpXuShuJianyan = new("declare", null, []) });
        PublishMobileSpXuShuJianyan(f); return SkillProgramStepOutcome.AwaitChoice;
    }
    private IReadOnlyList<PromptChoice> MobileSpXuShuJianyanChoices(ProgramSkillFrame f)
    {
        var r = f.MobileSpXuShuJianyan!;
        if (r.Stage == "declare") return new[] { "basic", "trick", "equipment", "red", "black" }.Select(criterion =>
            new PromptChoice(new($"mobile-jianyan.{f.Id}.{criterion}"), criterion switch { "basic" => "基本牌", "trick" => "锦囊牌", "equipment" => "装备牌", "red" => "红色牌", _ => "黑色牌" }, [], [],
                new Dictionary<string, string> { ["program-action"] = "mobile-sp-xu-shu-jianyan", ["criterion"] = criterion })).ToArray();
        return _players.Where(p => p.IsAlive && p.Gender == GeneralGender.Male).Select(p =>
            new PromptChoice(new($"mobile-jianyan.{f.Id}.recipient-{p.Seat}"), "令 " + p.Name + " 获得最后亮出的牌", [], [p.Seat],
                new Dictionary<string, string> { ["program-action"] = "mobile-sp-xu-shu-jianyan" })).ToArray();
    }
    private void PublishMobileSpXuShuJianyan(ProgramSkillFrame f)
    {
        var choices = MobileSpXuShuJianyanChoices(f); var skill = _contentRegistry.GetSkill(f.SkillId);
        _pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.MobileSpXuShuJianyan!.Stage == "declare" ? "荐言：声明一种类别或颜色。" : "荐言：选择一名男性角色获得符合条件的牌。", [], choices.SelectMany(c => c.Targets).ToArray(), f.OwnerSeat)
        { PromptId = CreatePromptId(), Choices = choices, SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description) };
        _status = _players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        AdvanceRulesAndPublishState();
    }
    [ProgramAnswer("mobile-sp-xu-shu-jianyan")]
    private void AnswerMobileSpXuShuJianyan(PromptChoice choice)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { MobileSpXuShuJianyan: { } r } f ||
            _pendingDecision?.PlayerSeat != f.OwnerSeat || !MobileSpXuShuJianyanChoices(f).Any(c => c.Id == choice.Id && c.Targets.SequenceEqual(choice.Targets)))
            throw new InvalidOperationException("Mobile Jianyan rejects a different original chooser, criterion or recipient.");
        AssertMobileSpXuShuJianyan(f); ClearPendingDecision();
        if (r.Stage == "declare")
        {
            if (!_players[f.OwnerSeat].IsAlive || !HasRuntimeSkillInstance(_players[f.OwnerSeat], f.SkillId, f.SkillInstanceId)) { FinishProgramSkill(f, false); return; }
            ReplaceRuntimeTop(f with { MobileSpXuShuJianyan = r with { Stage = "search", Criterion = choice.Parameters["criterion"] } });
            AdvanceRuntimeProgram(f.Id); return;
        }
        if (r.Stage != "recipient" || r.MatchedCardId is not { } card || _cardZones.GetLocation(card) != CardLocation.Processing)
            throw new InvalidOperationException("Mobile Jianyan lost its original matching revealed entity.");
        var target = choice.Targets.Single();
        ReplaceRuntimeTop(f = f with { MobileSpXuShuJianyan = r with { Stage = "gift-children", RecipientSeat = target }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var before = _movementSequence;
        MoveProgramCardsFromMultipleSources([card], CardLocation.Hand(target), new(MobileSpXuShuJianyanGiftReason), (batch, records) =>
        {
            RecordMobileSpXuShuJianyanMovement(f.Id, "gift", batch, before, records);
            AdvanceEventRulesAndQueueFact(new MobileSpXuShuJianyanGiftPaidEvent(f.Id, target, card, records.Single().Sequence));
        });
        CaptureMobileSpXuShuJianyanNativeBatch(f.Id);
        AdvanceRuntimeProgram(f.Id);
    }
    [ProgramContinuation(ProgramContinuationKind.Resume, 71700)]
    private bool ResumeMobileSpXuShuJianyan(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { MobileSpXuShuJianyan: { } r } f || f.Id != id) return false;
        AssertMobileSpXuShuJianyan(f);
        if (r.Stage == "declare") return true;
        if (r.Stage == "recipient" && _pendingDecision is not null) return true;
        if (TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) ||
            TryBeginAdvancedSkillsChanged(f.Id) || TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || TryBeginCardsMovedProgramWindow(f.Id)) return true;
        f = GetActiveProgramFrame(id); r = f.MobileSpXuShuJianyan!;
        if (f.PendingMovementContinuation is not null) ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (r.Stage == "recipient") { PublishOrFinishMobileSpXuShuJianyanRecipient(f); return true; }
        if (r.Stage == "cleanup-children")
        { ReplaceRuntimeTop(f = f with { MobileSpXuShuJianyan = null }); FinishProgramSkill(f, true); return true; }
        if (r.Stage == "gift-children" || !_players[f.OwnerSeat].IsAlive || _winner != Winner.None)
        { BeginMobileSpXuShuJianyanCleanup(f); return true; }
        if (r.Stage != "search" || r.Criterion is null) throw new InvalidOperationException("Mobile Jianyan lost its original search cursor.");
        if (!EnsureDrawPile()) { BeginMobileSpXuShuJianyanCleanup(f); return true; }
        var card = _cardZones.CardsAt(CardLocation.DrawPile)[^1]; var matched = MatchesDeclaredDeckCriterion(card, r.Criterion);
        ReplaceRuntimeTop(f = f with { MobileSpXuShuJianyan = r with { Stage = matched ? "recipient" : "search", RevealedIds = r.RevealedIds.Append(card.Id).ToArray(), MatchedCardId = matched ? card.Id : null }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var before = _movementSequence;
        MoveProgramCardsFromMultipleSources([card.Id], CardLocation.Processing, new(MobileSpXuShuJianyanRevealReason), (batch, records) =>
        {
            RecordMobileSpXuShuJianyanMovement(f.Id, "reveal", batch, before, records);
            AdvanceEventRulesAndQueueFact(new MobileSpXuShuJianyanRevealedEvent(f.Id, r.Criterion!, card.Id, matched, records.Single().Sequence));
        }, disclosure: CardMovementDisclosure.FaceUp);
        CaptureMobileSpXuShuJianyanNativeBatch(f.Id);
        f = GetActiveProgramFrame(id); r = f.MobileSpXuShuJianyan!;
        var binding = new ProgramSkillCardSetBinding(MobileSpXuShuJianyanBind, Array.AsReadOnly(r.RevealedIds.ToArray()), SkillProgramCardSetVisibility.Public, Array.AsReadOnly(Enumerable.Repeat(CardLocation.Processing, r.RevealedIds.Count).ToArray()));
        ReplaceRuntimeTop(f = f with { CardSetBindings = Array.AsReadOnly(f.CardSetBindings.Where(b => b.Name != MobileSpXuShuJianyanBind).Append(binding).ToArray()) });
        AdvanceEventRulesAndQueueFact(new ProgramCardsRevealedEvent(f.Id, f.SkillId, GetProgramBindingId(f), f.OwnerSeat, MobileSpXuShuJianyanBind, [ToSnapshot(card)]));
        // The scheduler resumes this same instruction after the actual reveal
        // movement children. Every failed reveal stays in Processing until gift.
        if (!TryBeginCardsMovedProgramWindow(f.Id))
        {
            ReplaceRuntimeTop(f = GetActiveProgramFrame(id) with { PendingMovementContinuation = null });
            if (matched) PublishOrFinishMobileSpXuShuJianyanRecipient(f);
        }
        return true;
    }
    private void PublishOrFinishMobileSpXuShuJianyanRecipient(ProgramSkillFrame f)
    {
        if (!_players[f.OwnerSeat].IsAlive || _winner != Winner.None ||
            f.MobileSpXuShuJianyan!.MatchedCardId is not { } card || _cardZones.GetLocation(card) != CardLocation.Processing ||
            !_players.Any(p => p.IsAlive && p.Gender == GeneralGender.Male))
            BeginMobileSpXuShuJianyanCleanup(f);
        else PublishMobileSpXuShuJianyan(f);
    }
    private void BeginMobileSpXuShuJianyanCleanup(ProgramSkillFrame f)
    {
        var r = f.MobileSpXuShuJianyan!;
        var ids = r.RevealedIds.Where(id => _cardZones.GetLocation(id) == CardLocation.Processing).ToArray(); var before = _movementSequence;
        ReplaceRuntimeTop(f = f with { MobileSpXuShuJianyan = r with { Stage = "cleanup-children" }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (ids.Length > 0)
        {
            MoveProgramCardsFromMultipleSources(ids, CardLocation.DiscardPile, new(MobileSpXuShuJianyanCleanupReason),
                (batch, records) => RecordMobileSpXuShuJianyanMovement(f.Id, "cleanup", batch, before, records), disclosure: CardMovementDisclosure.FaceUp);
            CaptureMobileSpXuShuJianyanNativeBatch(f.Id);
        }
        else RecordMobileSpXuShuJianyanMovement(f.Id, "cleanup", 0, before, []);
        AdvanceEventRulesAndQueueFact(new MobileSpXuShuJianyanCleanupPaidEvent(f.Id, ids.Length, before, _movementSequence));
    }
    private void RecordMobileSpXuShuJianyanMovement(long id, string kind, long batch, long before, IReadOnlyList<CardMovementRecord> records)
    {
        var f = GetActiveProgramFrame(id);
        ReplaceRuntimeTop(f with { MobileSpXuShuJianyan = f.MobileSpXuShuJianyan! with { Movement = new(kind, batch, before, _movementSequence, records) } });
    }
    private void CaptureMobileSpXuShuJianyanNativeBatch(long id)
    {
        var f = GetActiveProgramFrame(id); var m = f.MobileSpXuShuJianyan!.Movement!;
        var batch = _pendingCardsMovedBatches.SingleOrDefault(b => b.Id == m.BatchId);
        if (batch is null || !MobileSpXuShuJianyanBatchMatches(f, m, batch))
            throw new InvalidOperationException("Mobile Jianyan lost its own native physical movement batch and origin.");
        AdvanceEventRulesAndQueueFact(new MobileSpXuShuJianyanNativeBatchIssuedEvent(f.Id, m.Kind, batch.Id, batch.ParentFrameId!.Value,
            batch.AwaitingProgramFrameId!.Value, batch.OriginOwnerSeat!.Value, batch.OriginSkillId!, batch.OriginSkillInstanceId!, m.Before, m.After, m.Movements.Count));
    }
    private static bool MobileSpXuShuJianyanBatchMatches(ProgramSkillFrame f, MobileSpXuShuJianyanMovementReceipt m, CardMovementBatchContext batch) =>
        batch.Id == m.BatchId && batch.Id > f.Id && batch.ParentFrameId == f.Id && batch.AwaitingProgramFrameId == f.Id &&
        batch.OriginOwnerSeat == f.OwnerSeat && batch.OriginSkillId == f.SkillId && batch.OriginSkillInstanceId == f.SkillInstanceId &&
        batch.Movements.SequenceEqual(m.Movements);
    private bool ValidMobileSpXuShuJianyanMovement(ProgramSkillFrame f)
    {
        var r = f.MobileSpXuShuJianyan!;
        if (r.Movement is not { } p || p.Before < 0 || p.After < p.Before || p.After > _movementSequence ||
            p.Movements.Select(m => m.CardId).Distinct().Count() != p.Movements.Count ||
            p.Movements.Any(m => m.Sequence <= p.Before || m.Sequence > p.After || !_cardMovements.Contains(m)) ||
            !p.Movements.SequenceEqual(_cardMovements.Where(m => m.Sequence > p.Before && m.Sequence <= p.After))) return false;
        if (p.Movements.Count == 0)
            return r.Stage == "cleanup-children" && p.Kind == "cleanup" && p.BatchId == 0 && p.Before == p.After &&
                ProgramEventHistory<MobileSpXuShuJianyanCleanupPaidEvent>().Count(e => e == new MobileSpXuShuJianyanCleanupPaidEvent(f.Id, 0, p.Before, p.After)) == 1;
        if (ProgramEventHistory<MobileSpXuShuJianyanNativeBatchIssuedEvent>().Count(e => e == new MobileSpXuShuJianyanNativeBatchIssuedEvent(
                f.Id, p.Kind, p.BatchId, f.Id, f.Id, f.OwnerSeat, f.SkillId, f.SkillInstanceId, p.Before, p.After, p.Movements.Count)) != 1 ||
            _pendingCardsMovedBatches.Concat(_resolutionStack.OfType<CardsMovedTriggerWindowFrame>().Select(w => w.Batch))
                .Where(b => b.Id == p.BatchId).Any(b => !MobileSpXuShuJianyanBatchMatches(f, p, b))) return false;
        return p.Kind switch
        {
            "reveal" => r.Stage is "search" or "recipient" && p.Movements is [var m] && r.RevealedIds.LastOrDefault() == m.CardId &&
                m.From == CardLocation.DrawPile && m.To == CardLocation.Processing && m.Reason.Value == MobileSpXuShuJianyanRevealReason &&
                ProgramEventHistory<MobileSpXuShuJianyanRevealedEvent>().Count(e => e.FrameId == f.Id && e.CardId == m.CardId && e.Criterion == r.Criterion &&
                    e.MovementSequence == m.Sequence && e.Matched == (r.MatchedCardId == m.CardId)) == 1,
            "gift" => r.Stage == "gift-children" && r.RecipientSeat is { } target && p.Movements is [var m] && m.CardId == r.MatchedCardId &&
                m.From == CardLocation.Processing && m.To == NormalizeEphemeralCardDestination(GetAdvancedCard(m.CardId), m.From, CardLocation.Hand(target)) && m.Reason.Value == MobileSpXuShuJianyanGiftReason &&
                ProgramEventHistory<MobileSpXuShuJianyanGiftPaidEvent>().Count(e => e == new MobileSpXuShuJianyanGiftPaidEvent(f.Id, target, m.CardId, m.Sequence)) == 1,
            "cleanup" => r.Stage == "cleanup-children" && p.Movements.All(m => r.RevealedIds.Contains(m.CardId) && (r.RecipientSeat is null || m.CardId != r.MatchedCardId) &&
                m.From == CardLocation.Processing && (m.To == NormalizeEphemeralCardDestination(GetAdvancedCard(m.CardId), m.From, CardLocation.DiscardPile) ||
                    IsRedirectedPaidDiscardMovement(m, CardLocation.Processing, new(MobileSpXuShuJianyanCleanupReason))) && m.Reason.Value == MobileSpXuShuJianyanCleanupReason) &&
                ProgramEventHistory<MobileSpXuShuJianyanCleanupPaidEvent>().Count(e => e == new MobileSpXuShuJianyanCleanupPaidEvent(f.Id, p.Movements.Count, p.Before, p.After)) == 1,
            _ => false
        };
    }
    [ProgramContinuation(ProgramContinuationKind.Movement, 71700)]
    private bool ReturnMobileSpXuShuJianyanMovement(ProgramSkillFrame f)
    {
        if (f.MobileSpXuShuJianyan is null) return false;
        AssertMobileSpXuShuJianyan(f);
        ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (f.MobileSpXuShuJianyan.Stage == "recipient") PublishOrFinishMobileSpXuShuJianyanRecipient(f); else ResumeMobileSpXuShuJianyan(f.Id);
        return true;
    }
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 71700)]
    private bool MobileSpXuShuJianyanMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) =>
        e?.Op == SkillProgramEffectOp.ResolveMobileSpXuShuJianyan && f.SkillId == MobileSpXuShuJianyanSkill && f.TriggerId is null &&
        f.ActivationId == "declare-and-give" && f.InstructionIndex == 1 && f.MobileSpXuShuJianyan is not null &&
        p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && ValidMobileSpXuShuJianyanMovement(f);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 71700)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 71700)]
    private bool KeepMobileSpXuShuJianyanIssued(ProgramSkillFrame f) => f.MobileSpXuShuJianyan is { RevealedIds.Count: > 0 };
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 71700)]
    private void AssertMobileSpXuShuJianyan(ProgramSkillFrame f)
    {
        if (f.MobileSpXuShuJianyan is not { } r) return;
        if (f.SkillId != MobileSpXuShuJianyanSkill || f.TriggerId is not null || f.InstructionIndex != 1 || f.ActivationId != "declare-and-give" ||
            r.Stage is not ("declare" or "search" or "recipient" or "gift-children" or "cleanup-children") ||
            r.Stage != "declare" && r.Criterion is not ("basic" or "trick" or "equipment" or "red" or "black") ||
            r.RevealedIds.Distinct().Count() != r.RevealedIds.Count ||
            r.RevealedIds.Any(id => ProgramEventHistory<MobileSpXuShuJianyanRevealedEvent>().Count(e => e.FrameId == f.Id && e.CardId == id && e.Criterion == r.Criterion &&
                _cardMovements.Any(m => m.Sequence == e.MovementSequence && m.CardId == id && m.From == CardLocation.DrawPile && m.To == CardLocation.Processing && m.Reason.Value == MobileSpXuShuJianyanRevealReason)) != 1) ||
            r.MatchedCardId is { } match && !r.RevealedIds.Contains(match) ||
            r.Movement is not null && !ValidMobileSpXuShuJianyanMovement(f) ||
            f.PendingMovementContinuation is not null && r.Movement is null ||
            r.Stage == "gift-children" && ProgramEventHistory<MobileSpXuShuJianyanGiftPaidEvent>().Count(e => e.FrameId == f.Id && e.CardId == r.MatchedCardId && e.RecipientSeat == r.RecipientSeat) != 1 ||
            r.Stage == "cleanup-children" && ProgramEventHistory<MobileSpXuShuJianyanCleanupPaidEvent>().Count(e => e.FrameId == f.Id) != 1)
            throw new InvalidOperationException("Mobile Jianyan lost its original immutable reveal pool or once-paid native gift/cleanup.");
    }
    [ProgramAiChoice(SkillProgramEffectOp.ResolveMobileSpXuShuJianyan)]
    private PromptChoice MobileSpXuShuJianyanAi(PendingDecision p, ProgramSkillFrame f) =>
        f.MobileSpXuShuJianyan?.Stage == "declare" ? p.Choices.First(c => c.Parameters.GetValueOrDefault("criterion") == "basic") :
        p.Choices.FirstOrDefault(c => c.Targets.SequenceEqual([f.OwnerSeat])) ?? p.Choices.First();
}
