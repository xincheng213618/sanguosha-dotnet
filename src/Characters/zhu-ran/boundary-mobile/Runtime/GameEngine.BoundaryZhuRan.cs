namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string BoundaryDanshouDiscard = "skill-program.boundary-zhu-ran.danshou.discard";
    private bool HasBoundaryDanshou => _contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.PrepareBoundaryZhuRanDanshou);
    private static CardConversionSource BoundaryDanshouSource(ProgramSkillFrame f) => new(f.SkillId, GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedAfterCategory, 53900)]
    private bool BoundaryDanshouFinalizedTargetWindows(ProgramTargetCardWindowQuery query) => HasBoundaryDanshou;

    // Accepted actions retain their designation even when an effect later becomes
    // ineffective or its active target cursor is cleared. Keep one ever-targeted
    // fact per native whole Use, and update its actual actor after role replacement.
    private void CaptureBoundaryDanshouUse(CardUseFrame use)
    {
        if (!HasBoundaryDanshou || use.Action is { Type: not CardActionType.Use } ||
            ProgramEventHistory<CurrentTurnCardUseKindsRecordedEvent>().SingleOrDefault(e => e.OriginFrameId == use.Id &&
                e.RoundActionType == CardActionType.Use && e.ProgramInstructionIndex is null) is not { } declared) return;
        if (use.Action is { } a)
        {
            if (a.ActorSeat != use.SourceSeat || !ProgramEventHistory<CardActionAcceptedEvent>().Any(e => e.Action.ActionId == a.ActionId && e.Action.Type == CardActionType.Use)) return;
            var actor = declared.ActorSeat;
            foreach (var change in ProgramEventHistory<ProgramCardUseActorReplacedEvent>().Where(e => e.CardUseFrameId == use.Id))
            {
                if (change.PreviousActorSeat != actor || change.ProviderSeat != a.ProviderSeat) return;
                actor = change.ActorSeat;
            }
            // A role mutation updates native state before publishing its typed
            // replacement fact. Wait for that fact rather than recording the
            // transient half of the mutation.
            if (actor != use.SourceSeat) return;
        }
        else if (!IsActualTurnLegacyVirtualUse(use)) return;
        var old = ProgramEventHistory<BoundaryDanshouUseActorEvent>().LastOrDefault(e => e.UseId == use.Id);
        var next = new BoundaryDanshouUseActorEvent(use.Id, use.Action?.ActionId, declared.TurnNumber, declared.TurnOwnerSeat, use.SourceSeat, use.Action?.ProviderSeat ?? use.SourceSeat);
        if (old != next) AdvanceEventRulesAndQueueFact(next);
        var targets = use.Action?.EffectiveDesignatedTargetSeats ?? (use.CardKind == CardKind.BorrowedSword
            ? use.TargetSeats.Where((_, i) => i % 2 == 0).ToArray() : use.TargetSeats);
        foreach (var target in targets.Distinct().Where(IsValidPlayerSeat))
            if (!ProgramEventHistory<BoundaryDanshouUseTargetEvent>().Any(e => e.UseId == use.Id && e.Target == target))
                AdvanceEventRulesAndQueueFact(new BoundaryDanshouUseTargetEvent(use.Id, declared.TurnNumber, declared.TurnOwnerSeat, target));
    }
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 53900)]
    private void ObserveBoundaryDanshouHistory(IGameEvent payload)
    {
        if (!HasBoundaryDanshou || payload is BoundaryDanshouUseActorEvent or BoundaryDanshouUseTargetEvent or BoundaryDanshouDirectDuelTargetEvent) return;
        // The original native legacy Duel owns its attack on the producing
        // program instruction instead of manufacturing a CardUse/Action.
        if (payload is CurrentTurnCardUseKindsRecordedEvent { RoundActionType: CardActionType.Use, ProgramInstructionIndex: { } instruction } direct &&
            _resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == direct.OriginFrameId) is { } producer &&
            producer.InstructionIndex == instruction && FrozenProgramDuelInstructionIndex(producer.Id) == instruction &&
            producer.CardAttack is { Active: true, EffectiveCardKind: CardKind.Duel, CardId: null, PhysicalCardIds.Count: 0 } attack &&
            producer.Continuations.Duel is { Active: true } && attack.CardUserSeat == direct.ActorSeat &&
            !ProgramEventHistory<BoundaryDanshouDirectDuelTargetEvent>().Any(e => e.ProducerFrameId == producer.Id && e.InstructionIndex == instruction))
            AdvanceEventRulesAndQueueFact(new BoundaryDanshouDirectDuelTargetEvent(producer.Id, instruction, direct.TurnNumber, direct.TurnOwnerSeat, direct.ActorSeat, attack.TargetSeat));
        foreach (var use in _resolutionStack.OfType<CardUseFrame>().ToArray()) CaptureBoundaryDanshouUse(use);
    }
    [NativeUsePreparation(53900)]
    private bool SealBoundaryDanshouCompletedUse(CardUseFrame use) { CaptureBoundaryDanshouUse(use); return false; }
    private int BoundaryDanshouCount(int owner, int actor, int turn, int turnOwner, long cutoff)
    {
        var history = CompleteProgramEventHistory().Take(checked((int)cutoff)).ToArray();
        var targeted = history.OfType<BoundaryDanshouUseTargetEvent>().Where(e => e.Target == owner && e.Turn == turn && e.TurnOwner == turnOwner).Select(e => e.UseId).ToHashSet();
        return history.OfType<BoundaryDanshouUseActorEvent>().Where(e => e.Turn == turn && e.TurnOwner == turnOwner).GroupBy(e => e.UseId)
            .Select(g => g.Last()).Count(e => e.Actor == actor && targeted.Contains(e.UseId)) +
            history.OfType<BoundaryDanshouDirectDuelTargetEvent>().Count(e => e.Turn == turn && e.TurnOwner == turnOwner && e.Actor == actor && e.Target == owner);
    }
    private TurnEndingBoundaryFrame? BoundaryDanshouMother(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (c.SkillId != BoundaryZhuRanComposition.Skill || c.BindingId is not (BoundaryZhuRanComposition.Draw or BoundaryZhuRanComposition.Pay) ||
            context is not { Window: SkillProgramTriggerWindow.TurnEnding, SourceSeat: { } actor, TargetSeat: { } target } || actor != target || actor == c.OwnerSeat ||
            context.OwnerSeat != c.OwnerSeat || context.OccurrenceIndex != c.OccurrenceIndex ||
            _resolutionStack.OfType<TurnEndingBoundaryFrame>().SingleOrDefault(p => p.Id == context.ParentFrameId) is not { } p ||
            p.OwnerSeat != actor || p.ItemIndex < 0 || p.ItemIndex >= p.Items.Count || p.Items[p.ItemIndex].Candidate != c ||
            context.Facts != p.Items[p.ItemIndex].Facts) return null;
        return p;
    }
    private Card[] BoundaryDanshouMaterials(int owner) => GetHand(_players[owner]).Concat(GetEquipment(_players[owner]))
        .Where(card => !IsSelfHandCategoryDiscardForbidden(owner, card, _cardZones.GetLocation(card.Id), OwnedCardMoveIntent.Discard) &&
            !IsForeignEquipmentDiscardPrevented(owner, card, _cardZones.GetLocation(card.Id), OwnedCardMoveIntent.Discard)).ToArray();
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 53900, SkillProgramEffectOp.PrepareBoundaryZhuRanDanshou)]
    private bool QualifyBoundaryDanshou(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (BoundaryDanshouMother(c, context) is not { } p || !_players[c.OwnerSeat].IsAlive) return false;
        if (ProgramEventHistory<BoundaryDanshouStartedEvent>().Any(e => e.EndingId == p.Id && e.Source.OwnerSeat == c.OwnerSeat && e.Source.SkillInstanceId == c.SkillInstanceId)) return false;
        var x = BoundaryDanshouCount(c.OwnerSeat, p.OwnerSeat, p.TurnNumber, _turnProgression.OwnerSeat, CompleteProgramEventHistory().Count());
        return c.BindingId == BoundaryZhuRanComposition.Draw ? x == 0 : x > 0 && BoundaryDanshouMaterials(c.OwnerSeat).Length >= x;
    }
    [ProgramCapability]
    private sealed class BoundaryZhuRanHost(GameEngine engine) : IBoundaryZhuRanHost
    { public SkillProgramStepOutcome Prepare(ProgramSkillFrame f) => engine.PrepareBoundaryDanshou(f); }
    private SkillProgramStepOutcome PrepareBoundaryDanshou(ProgramSkillFrame supplied)
    {
        var f = GetActiveProgramFrame(supplied.Id); var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0, f.WindowContext?.OccurrenceIndex ?? 0);
        if (f.BoundaryDanshou is not null || f.InstructionIndex != 1 || f.WindowContext is not { } c || BoundaryDanshouMother(candidate, c) is not { } parent || !QualifyBoundaryDanshou(candidate, c))
            throw new InvalidOperationException("Danshou requires the original other role's Ending and its actual whole-use target history.");
        var cutoff = CompleteProgramEventHistory().Count(); var turnOwner = _turnProgression.OwnerSeat;
        var x = BoundaryDanshouCount(f.OwnerSeat, parent.OwnerSeat, parent.TurnNumber, turnOwner, cutoff);
        var r = new BoundaryDanshouReceipt(BoundaryDanshouSource(f), f.GameplayHash, parent.Id, parent.TurnNumber, turnOwner, parent.OwnerSeat, cutoff, x,
            x == 0 ? BoundaryDanshouStage.Complete : BoundaryDanshouStage.Selecting);
        ReplaceRuntimeTop(f = f with { BoundaryDanshou = r });
        AdvanceEventRulesAndQueueFact(new BoundaryDanshouStartedEvent(f.Id, r.Source, r.Hash, parent.Id, r.Turn, turnOwner, r.Actor, cutoff, x));
        if (x == 0) return SkillProgramStepOutcome.Continue;
        PublishBoundaryDanshou(f); return SkillProgramStepOutcome.AwaitChoice;
    }
    private IReadOnlyList<PromptChoice> BoundaryDanshouChoices(ProgramSkillFrame f)
    {
        var r = f.BoundaryDanshou!; var choices = new List<PromptChoice>();
        PromptChoice Choice(string branch, string label, IReadOnlyList<int>? cards = null) => new(new($"boundary-danshou.{f.Id}.{branch}.{string.Join('-', cards ?? [])}"), label, cards ?? [], [],
            new Dictionary<string, string> { ["program-action"] = "boundary-danshou", ["branch"] = branch });
        var legal = BoundaryDanshouMaterials(f.OwnerSeat);
        foreach (var card in legal.Where(c => r.Selected.Contains(c.Id))) choices.Add(Choice("unpick", $"取消选择【{card.DisplayName}】", [card.Id]));
        if (r.Selected.Count < r.X)
            foreach (var card in legal.Where(c => !r.Selected.Contains(c.Id))) choices.Add(Choice("card", $"选择弃置【{card.DisplayName}】", [card.Id]));
        if (r.Selected.Count == r.X) choices.Add(Choice("pay", $"同时弃置{r.X}张牌，对{_players[r.Actor].Name}造成1点伤害"));
        choices.Add(Choice("decline", "取消弃牌")); return choices;
    }
    private void PublishBoundaryDanshou(ProgramSkillFrame f)
    {
        var r = f.BoundaryDanshou!; var choices = BoundaryDanshouChoices(f); var skill = _contentRegistry.GetSkill(f.SkillId);
        var prompt = new PendingDecision(DecisionKind.ProgramTrigger, f.OwnerSeat, $"胆守：选择{r.X}张手牌或装备牌，确认后同时弃置。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [], r.Actor)
        { PromptId = CreatePromptId(), IsPrivate = true, TargetSeat = f.OwnerSeat, Choices = choices, SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description) };
        var remaining = r.X - r.Selected.Count;
        _pendingDecision = NativeOwnedSelection(prompt, f, "danshou-cost", "branch", ["card"], ["pay"], r.Selected, remaining, remaining);
        _status = _players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }
    [ProgramAnswer("boundary-danshou")]
    private void AnswerBoundaryDanshou(PromptChoice choice)
    {
        var f = _resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Danshou lost its original cost frame.");
        AssertBoundaryDanshou(f); var r = f.BoundaryDanshou!;
        if (r.Stage != BoundaryDanshouStage.Selecting || _pendingDecision?.PlayerSeat != f.OwnerSeat || !BoundaryDanshouChoices(f).Any(c => c.Id == choice.Id && c.Cards.SequenceEqual(choice.Cards) && c.Targets.SequenceEqual(choice.Targets)))
            throw new InvalidOperationException("Danshou rejects a stale or unrelated owned cost entity.");
        ClearPendingDecision();
        switch (choice.Parameters["branch"])
        {
            case "card": ReplaceRuntimeTop(f = f with { BoundaryDanshou = r with { Selected = [.. r.Selected, choice.Cards.Single()] } }); PublishBoundaryDanshou(f); return;
            case "unpick": ReplaceRuntimeTop(f = f with { BoundaryDanshou = r with { Selected = r.Selected.Where(id => id != choice.Cards.Single()).ToArray() } }); PublishBoundaryDanshou(f); return;
            case "decline": CancelProgramBindingAndCleanup(f, "取消胆守付款。"); return;
            case "pay": PayBoundaryDanshou(f); return;
            default: throw new InvalidOperationException("Unknown Danshou cost option.");
        }
    }
    private void PayBoundaryDanshou(ProgramSkillFrame f)
    {
        var r = f.BoundaryDanshou!; var legal = BoundaryDanshouMaterials(f.OwnerSeat).Select(c => c.Id).ToHashSet();
        if (r.Selected.Count != r.X || r.Selected.Any(id => !legal.Contains(id)) || !_players[f.OwnerSeat].IsAlive || !_players[r.Actor].IsAlive ||
            !HasRuntimeSkillInstance(_players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
        { CancelProgramBindingAndCleanup(f, "胆守成本或来源已失效。"); return; }
        foreach (var player in _players) _observedSkillGrantRevisions.TryAdd(player.Seat, player.SkillGrants.Revision);
        ReplaceRuntimeTop(f = f with { BoundaryDanshou = r = r with { Stage = BoundaryDanshouStage.PaidChildren, Paid = true, Before = _movementSequence, After = _movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        MoveProgramLogicalDiscard(f, f.OwnerSeat, r.Selected, new(BoundaryDanshouDiscard));
        var invoice = ProgramEventHistory<NativeLogicalDiscardInvocationRecordedEvent>().SingleOrDefault(e => e.ParentFrameId == f.Id && e.Reason.Value == BoundaryDanshouDiscard && e.SequenceBefore == r.Before);
        f = GetActiveProgramFrame(f.Id); ReplaceRuntimeTop(f = f with { BoundaryDanshou = r with { After = _movementSequence, DiscardInvocationId = invoice?.InvocationId, Actual = invoice?.ActualCount ?? 0 } });
        AdvanceEventRulesAndQueueFact(new BoundaryDanshouPaidEvent(f.Id, r.X, invoice?.InvocationId, invoice?.ActualCount ?? 0, r.Before, _movementSequence));
        ContinueBoundaryDanshou(f.Id);
    }
    private void ContinueBoundaryDanshou(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { BoundaryDanshou: { Stage: BoundaryDanshouStage.PaidChildren } r } f || f.Id != id) return;
        if (TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) ||
            TryBeginAdvancedSkillsChanged(f.Id) || TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || TryBeginCardsMovedProgramWindow(f.Id)) return;
        f = GetActiveProgramFrame(id); var damage = r.Actual == r.X && _players[r.Actor].IsAlive && _winner == Winner.None;
        AdvanceEventRulesAndQueueFact(new BoundaryDanshouCompletedEvent(f.Id, r.Actual, damage));
        ReplaceRuntimeTop(f = f with { BoundaryDanshou = r with { Stage = BoundaryDanshouStage.Complete }, PendingMovementContinuation = null });
        if (damage) AdvanceRuntimeProgram(f.Id); else FinishProgramSkill(f, false);
    }
    [ProgramContinuation(ProgramContinuationKind.Resume, 53900)]
    private bool ResumeBoundaryDanshou(long id)
    { if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { BoundaryDanshou.Stage: BoundaryDanshouStage.PaidChildren } f || f.Id != id) return false; ContinueBoundaryDanshou(id); return true; }
    [ProgramContinuation(ProgramContinuationKind.Movement, 53900)]
    private bool ReturnBoundaryDanshouMovement(ProgramSkillFrame f)
    { if (f.BoundaryDanshou?.Stage != BoundaryDanshouStage.PaidChildren) return false; ContinueBoundaryDanshou(f.Id); return true; }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 53900)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 53900)]
    private bool KeepBoundaryDanshouPaid(ProgramSkillFrame f) => f.BoundaryDanshou is { Paid: true };
    [ProgramAiChoice(SkillProgramEffectOp.PrepareBoundaryZhuRanDanshou)]
    private static PromptChoice ChooseBoundaryDanshou(PendingDecision p) => p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "pay") ??
        p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "card") ?? p.Choices.Last();
}
