using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record BoundaryZhangZhaoPhaseDiscardEvent(ActualDiscardRecoveryPhaseKey Phase, int CardId, long MovementSequence, CardLocation OriginalFrom) : IGameEvent;
public sealed record BoundaryZhangZhaoReturnedEvent(long FrameId, int PhaseOwnerSeat, int CardId, long MovementSequence) : IGameEvent;
public sealed record BoundaryZhangZhaoClaimFinishedEvent(long FrameId, bool Taken, int Count, long Before, long After) : IGameEvent;
public sealed record BoundaryZhangZhaoZhijianPaidEvent(long FrameId, CardConversionSource Source, string Hash, int TargetSeat, int CardId, long MovementSequence) : IGameEvent;
public sealed record BoundaryZhangZhaoPhaseReceipt(ActualDiscardRecoveryPhaseKey Phase, int PhaseOwnerSeat, string Stage,
    IReadOnlyList<DiscardRecoveryEntity> OriginalEntities, int? ReturnedCardId = null, long ReturnMovementSequence = 0)
{
    private readonly IReadOnlyList<DiscardRecoveryEntity> _entities = Array.AsReadOnly(OriginalEntities.ToArray());
    public IReadOnlyList<DiscardRecoveryEntity> OriginalEntities { get => _entities; init => _entities = Array.AsReadOnly(value.ToArray()); }
}
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BoundaryZhangZhaoPhaseReceipt? BoundaryZhangZhaoPhase { get; init; }
}
internal interface IBoundaryZhangZhaoMobileHost { SkillProgramStepOutcome Restore(ProgramSkillFrame f); }
internal sealed class CaptureBoundaryZhangZhaoPhaseDiscardsDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.CaptureBoundaryZhangZhaoPhaseDiscards;
    public override ProgramFactRequirements FactRequirements => new(Triggered: ProgramTrackedFact.ActualDiscardRecoveryPhase);
    public override ISkillProgramEffectHandler Handler { get; } = new BoundaryZhangZhaoPhaseHandler();
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, c) => c.PublicControlValue(2));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.DiscardPhaseEnded)];
}
internal sealed class BoundaryZhangZhaoPhaseHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.CaptureBoundaryZhangZhaoPhaseDiscards;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int target, ISkillProgramEffectHost host) => host.GetCapability<IBoundaryZhangZhaoMobileHost>().Restore(f);
}

public sealed partial class GameEngine
{
    private const string BoundaryZhangZhaoGuzheng = "boundary:guzheng-mobile", BoundaryZhangZhaoZhijian = "boundary:zhijian-mobile";
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 66601)]
    private void ObserveBoundaryZhangZhaoPlacement(CardMovedEvent e)
    {
        if (e.Reason.Value != "skill-program.boundary:zhijian-mobile.SelectAndMoveOwnedCard" ||
            _resolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(f => f.SkillId == BoundaryZhangZhaoZhijian && f.TriggerId is null && f.InstructionIndex == 1) is not { } f ||
            f.SelectedTargetSeats is not [var target] || e.From != CardLocation.Hand(f.OwnerSeat) || e.To != CardLocation.Equipment(target)) return;
        var m = _cardMovements.Last(m => m.CardId == e.CardId && m.From == e.From && m.To == e.To && m.Reason == e.Reason);
        AdvanceEventRulesAndQueueFact(new BoundaryZhangZhaoZhijianPaidEvent(f.Id, new(f.SkillId, f.ActivationId, f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, target, e.CardId, m.Sequence));
    }
    private bool BoundaryZhangZhaoPlacementPaid(ProgramSkillFrame f) => f.SkillId == BoundaryZhangZhaoZhijian && f.TriggerId is null && f.InstructionIndex is 1 or 2 &&
        ProgramEventHistory<BoundaryZhangZhaoZhijianPaidEvent>().Where(e => e.FrameId == f.Id).ToArray() is [var e] &&
        e.Source == new CardConversionSource(f.SkillId, f.ActivationId, f.OwnerSeat, f.SkillInstanceId) && e.Hash == f.GameplayHash && f.SelectedTargetSeats.SequenceEqual([e.TargetSeat]) &&
        _cardMovements.Any(m => m.Sequence == e.MovementSequence && m.CardId == e.CardId && m.From == CardLocation.Hand(f.OwnerSeat) && m.To == CardLocation.Equipment(e.TargetSeat) && m.Reason.Value == "skill-program.boundary:zhijian-mobile.SelectAndMoveOwnedCard");
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 66601)]
    private bool KeepBoundaryZhangZhaoPlacement(ProgramSkillFrame f) => BoundaryZhangZhaoPlacementPaid(f);
    [ProgramContinuation(ProgramContinuationKind.Movement, 66601)]
    private bool ReturnBoundaryZhangZhaoPlacement(ProgramSkillFrame f)
    {
        if (!BoundaryZhangZhaoPlacementPaid(f) || f.InstructionIndex != 1 || f.PendingMovementContinuation is not { SubjectSeat: var seat, BeforeCount: 0, CoverageResultBind: null } || seat != f.OwnerSeat) return false;
        ReplaceRuntimeTop(f with { PendingMovementContinuation = null }); AdvanceRuntimeProgram(f.Id); return true;
    }
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 66600)]
    private void ObserveBoundaryZhangZhaoPhaseDiscard(CardMovedEvent e)
    {
        if (!_contentRegistry.Skills.ContainsKey(BoundaryZhangZhaoGuzheng) || CurrentActualDiscardRecoveryPhase() is not { Kind: ActualDiscardRecoveryPhaseKind.Discard } phase) return;
        var m = _cardMovements.LastOrDefault(m => m.CardId == e.CardId && m.From == e.From && m.To == e.To && m.Reason == e.Reason);
        if (m is null || GetProgramDiscardSource(m) is not { } from) return;
        AdvanceEventRulesAndQueueFact(new BoundaryZhangZhaoPhaseDiscardEvent(phase, m.CardId, m.Sequence, from));
    }
    private IReadOnlyList<DiscardRecoveryEntity> BoundaryZhangZhaoPhaseEntities(ActualDiscardRecoveryPhaseKey phase) =>
        ProgramEventHistory<BoundaryZhangZhaoPhaseDiscardEvent>().Where(e => e.Phase == phase)
            .Select(e => new DiscardRecoveryEntity(e.CardId, e.MovementSequence, e.OriginalFrom)).Where(ActualDiscardEntityStillAvailable)
            .GroupBy(e => e.CardId).Select(g => g.First()).OrderBy(e => e.MovementSequence).ToArray();
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 66600)]
    private bool QualifyBoundaryZhangZhaoMobile(ProgramTriggerCandidate b, ProgramSkillWindowContext c)
    {
        if (b.SkillId == BoundaryZhangZhaoZhijian)
            return c.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized && _phase == TurnPhase.Play && _currentSeat == b.OwnerSeat;
        if (b.SkillId != BoundaryZhangZhaoGuzheng) return true;
        return c.Window == SkillProgramTriggerWindow.DiscardPhaseEnded && CurrentActualDiscardRecoveryPhase() is { Kind: ActualDiscardRecoveryPhaseKind.Discard } p &&
            p.ActorSeat != b.OwnerSeat && BoundaryZhangZhaoPhaseEntities(p).Any(e => e.OriginalFrom == CardLocation.Hand(p.ActorSeat));
    }
    [ProgramCapability]
    private sealed class BoundaryZhangZhaoMobileHost(GameEngine e) : IBoundaryZhangZhaoMobileHost
    { public SkillProgramStepOutcome Restore(ProgramSkillFrame f) => e.BeginBoundaryZhangZhaoPhase(f); }
    private SkillProgramStepOutcome BeginBoundaryZhangZhaoPhase(ProgramSkillFrame f)
    {
        if (f.BoundaryZhangZhaoPhase is not null || f.WindowContext?.Window != SkillProgramTriggerWindow.DiscardPhaseEnded ||
            CurrentActualDiscardRecoveryPhase() is not { Kind: ActualDiscardRecoveryPhaseKind.Discard } p || p.ActorSeat == f.OwnerSeat)
            throw new InvalidOperationException("Mobile Guzheng requires its original other player's completed discard phase.");
        ReplaceRuntimeTop(f = f with { BoundaryZhangZhaoPhase = new(p, p.ActorSeat, "return", BoundaryZhangZhaoPhaseEntities(p)) });
        PublishBoundaryZhangZhaoPhase(f); return SkillProgramStepOutcome.AwaitChoice;
    }
    private IReadOnlyList<PromptChoice> BoundaryZhangZhaoPhaseChoices(ProgramSkillFrame f)
    {
        var r = f.BoundaryZhangZhaoPhase!;
        PromptChoice Choice(string branch, int? card, string label) => new(new($"mobile-guzheng.{f.Id}.{branch}.{card}"), label, card is { } id ? [id] : [], [],
            new Dictionary<string,string> { ["program-action"] = "boundary-zhang-zhao-phase", ["branch"] = branch });
        if (r.Stage == "return") return r.OriginalEntities.Where(e => e.OriginalFrom == CardLocation.Hand(r.PhaseOwnerSeat) && ActualDiscardEntityStillAvailable(e))
            .Select(e => Choice("return", e.CardId, "交还【" + GetAdvancedCard(e.CardId).DisplayName + "】")).ToArray();
        return new[] { Choice("claim", null, "获得本阶段其余仍在弃牌堆的弃牌"), Choice("decline", null, "不获得其余弃牌") };
    }
    private void PublishBoundaryZhangZhaoPhase(ProgramSkillFrame f)
    {
        var choices = BoundaryZhangZhaoPhaseChoices(f); var skill = _contentRegistry.GetSkill(f.SkillId);
        _pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.BoundaryZhangZhaoPhase!.Stage == "return" ? "固政：交还该角色本阶段弃置的一张手牌。" : "固政：是否获得本阶段其余弃牌？", choices.SelectMany(c => c.Cards).ToArray(), [], f.OwnerSeat)
        { PromptId = CreatePromptId(), Choices = choices, SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description) };
        _status = _players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        AdvanceRulesAndPublishState();
    }
    [ProgramAnswer("boundary-zhang-zhao-phase")]
    private void AnswerBoundaryZhangZhaoPhase(PromptChoice choice)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { BoundaryZhangZhaoPhase: { } r } f || _pendingDecision?.PlayerSeat != f.OwnerSeat ||
            !BoundaryZhangZhaoPhaseChoices(f).Any(c => c.Id == choice.Id && c.Cards.SequenceEqual(choice.Cards)))
            throw new InvalidOperationException("Mobile Guzheng rejects an unrelated phase, chooser or discarded entity.");
        AssertBoundaryZhangZhaoPhase(f); ClearPendingDecision();
        if (r.Stage == "return")
        {
            if (!_players[f.OwnerSeat].IsAlive || !_players[r.PhaseOwnerSeat].IsAlive || !HasRuntimeSkillInstance(_players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            { FinishProgramSkill(f, false); return; }
            var id = choice.Cards.Single();
            ReplaceRuntimeTop(f = f with { BoundaryZhangZhaoPhase = r with { Stage = "return-children", ReturnedCardId = id }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            MoveCard(GetAdvancedCard(id), CardLocation.DiscardPile, CardLocation.Hand(r.PhaseOwnerSeat), new("skill-program.boundary:guzheng-mobile.phase-return"), m =>
            {
                var current = GetActiveProgramFrame(f.Id);
                ReplaceRuntimeTop(current with { BoundaryZhangZhaoPhase = current.BoundaryZhangZhaoPhase! with { ReturnMovementSequence = m.Sequence } });
                AdvanceEventRulesAndQueueFact(new BoundaryZhangZhaoReturnedEvent(f.Id, r.PhaseOwnerSeat, id, m.Sequence));
            });
        }
        else if (r.Stage == "claim")
        {
            var take = choice.Parameters["branch"] == "claim" && _players[f.OwnerSeat].IsAlive && _winner == Winner.None;
            var ids = take ? r.OriginalEntities.Where(e => e.CardId != r.ReturnedCardId && ActualDiscardEntityStillAvailable(e)).Select(e => e.CardId).ToArray() : [];
            var before = _movementSequence;
            ReplaceRuntimeTop(f = f with { BoundaryZhangZhaoPhase = r with { Stage = "claim-children" }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            if (ids.Length != 0) MoveCards(ids.Select(GetAdvancedCard).ToArray(), CardLocation.DiscardPile, CardLocation.Hand(f.OwnerSeat), new("skill-program.boundary:guzheng-mobile.phase-claim"));
            AdvanceEventRulesAndQueueFact(new BoundaryZhangZhaoClaimFinishedEvent(f.Id, take, ids.Length, before, _movementSequence));
        }
        else throw new InvalidOperationException("Mobile Guzheng cannot repeat an already completed return or claim.");
        AdvanceRuntimeProgram(f.Id);
    }
    [ProgramContinuation(ProgramContinuationKind.Resume, 66600)]
    private bool ResumeBoundaryZhangZhaoPhase(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { BoundaryZhangZhaoPhase: { } r } f || f.Id != id) return false;
        AssertBoundaryZhangZhaoPhase(f);
        if (r.Stage is "return" or "claim") return true;
        if (TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) ||
            TryBeginAdvancedSkillsChanged(f.Id) || TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || TryBeginCardsMovedProgramWindow(f.Id)) return true;
        f = GetActiveProgramFrame(id); r = f.BoundaryZhangZhaoPhase!;
        ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (r.Stage == "return-children" && _winner == Winner.None && _players[f.OwnerSeat].IsAlive && r.OriginalEntities.Any(e => e.CardId != r.ReturnedCardId && ActualDiscardEntityStillAvailable(e)))
        { ReplaceRuntimeTop(f = f with { BoundaryZhangZhaoPhase = r with { Stage = "claim" } }); PublishBoundaryZhangZhaoPhase(f); return true; }
        ReplaceRuntimeTop(f with { BoundaryZhangZhaoPhase = null }); AdvanceRuntimeProgram(id); return true;
    }
    [ProgramContinuation(ProgramContinuationKind.Movement, 66600)]
    private bool ReturnBoundaryZhangZhaoPhase(ProgramSkillFrame f) => f.BoundaryZhangZhaoPhase is not null && ResumeBoundaryZhangZhaoPhase(f.Id);
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 66600)]
    private bool BoundaryZhangZhaoPhaseMovement(ProgramSkillFrame f, SkillProgramEffect? op, ProgramMovementContinuation p) =>
        op?.Op == SkillProgramEffectOp.CaptureBoundaryZhangZhaoPhaseDiscards && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null &&
        f.BoundaryZhangZhaoPhase is { Stage: "return-children" or "claim-children" } && KeepBoundaryZhangZhaoPaidReturn(f);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 66600)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 66600)]
    private bool KeepBoundaryZhangZhaoPaidReturn(ProgramSkillFrame f) => f.BoundaryZhangZhaoPhase is { ReturnMovementSequence: > 0 } r &&
        ProgramEventHistory<BoundaryZhangZhaoReturnedEvent>().Count(e => e.FrameId == f.Id && e.PhaseOwnerSeat == r.PhaseOwnerSeat && e.CardId == r.ReturnedCardId && e.MovementSequence == r.ReturnMovementSequence) == 1;
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 66600)]
    private void AssertBoundaryZhangZhaoPhase(ProgramSkillFrame f)
    {
        if (f.BoundaryZhangZhaoPhase is not { } r) return;
        if (f.SkillId != BoundaryZhangZhaoGuzheng || f.InstructionIndex != 1 || f.WindowContext?.Window != SkillProgramTriggerWindow.DiscardPhaseEnded ||
            !IsRecordedActualDiscardRecoveryPhase(r.Phase) || r.Phase.Kind != ActualDiscardRecoveryPhaseKind.Discard || r.Phase.ActorSeat != r.PhaseOwnerSeat || r.PhaseOwnerSeat == f.OwnerSeat ||
            r.Stage is not ("return" or "return-children" or "claim" or "claim-children") || r.OriginalEntities.Select(e => e.CardId).Distinct().Count() != r.OriginalEntities.Count ||
            !r.OriginalEntities.Any(e => e.OriginalFrom == CardLocation.Hand(r.PhaseOwnerSeat)) ||
            r.OriginalEntities.Any(e => ProgramEventHistory<BoundaryZhangZhaoPhaseDiscardEvent>().Count(p => p.Phase == r.Phase && p.CardId == e.CardId && p.MovementSequence == e.MovementSequence && p.OriginalFrom == e.OriginalFrom) != 1) ||
            r.ReturnMovementSequence > 0 && !KeepBoundaryZhangZhaoPaidReturn(f))
            throw new InvalidOperationException("Mobile Guzheng lost its exact original phase pool or once-paid return.");
    }
    [ProgramAiChoice(SkillProgramEffectOp.CaptureBoundaryZhangZhaoPhaseDiscards)]
    private PromptChoice BoundaryZhangZhaoPhaseAi(PendingDecision p, ProgramSkillFrame f) => p.Choices.First();
}
