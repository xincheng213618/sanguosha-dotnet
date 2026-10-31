#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhuRanBoundaryMobileRuntime
{
    internal const string BoundaryDanshouDiscard = "skill-program.boundary-zhu-ran.danshou.discard";
    internal bool HasBoundaryDanshou => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.PrepareBoundaryZhuRanDanshou);

    internal static CardConversionSource BoundaryDanshouSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedAfterCategory, 53900)]
    internal bool BoundaryDanshouFinalizedTargetWindows(ProgramTargetCardWindowQuery query) => HasBoundaryDanshou;
    internal void CaptureBoundaryDanshouUse(CardUseFrame use)
    {
        if (!HasBoundaryDanshou || use.Action is { Type: not CardActionType.Use } || _coreProgramEventHistoryOperations.ProgramEventHistory<CurrentTurnCardUseKindsRecordedEvent>().SingleOrDefault(e => e.OriginFrameId == use.Id && e.RoundActionType == CardActionType.Use && e.ProgramInstructionIndex is null)is not { } declared)
            return;
        if (use.Action is { } a)
        {
            if (a.ActorSeat != use.SourceSeat || !_coreProgramEventHistoryOperations.ProgramEventHistory<CardActionAcceptedEvent>().Any(e => e.Action.ActionId == a.ActionId && e.Action.Type == CardActionType.Use))
                return;
            var actor = declared.ActorSeat;
            foreach (var change in _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramCardUseActorReplacedEvent>().Where(e => e.CardUseFrameId == use.Id))
            {
                if (change.PreviousActorSeat != actor || change.ProviderSeat != a.ProviderSeat)
                    return;
                actor = change.ActorSeat;
            }

            // A role mutation updates native state before publishing its typed
            // replacement fact. Wait for that fact rather than recording the
            // transient half of the mutation.
            if (actor != use.SourceSeat)
                return;
        }
        else if (!_coreCurrentTurnUseKindsOperations.IsActualTurnLegacyVirtualUse(use))
            return;
        var old = _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryDanshouUseActorEvent>().LastOrDefault(e => e.UseId == use.Id);
        var next = new BoundaryDanshouUseActorEvent(use.Id, use.Action?.ActionId, declared.TurnNumber, declared.TurnOwnerSeat, use.SourceSeat, use.Action?.ProviderSeat ?? use.SourceSeat);
        if (old != next)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(next);
        var targets = use.Action?.EffectiveDesignatedTargetSeats ?? (use.CardKind == CardKind.BorrowedSword ? use.TargetSeats.Where((_, i) => i % 2 == 0).ToArray() : use.TargetSeats);
        foreach (var target in targets.Distinct().Where(_coreTableSeatQueriesOperations.IsValidPlayerSeat))
            if (!_coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryDanshouUseTargetEvent>().Any(e => e.UseId == use.Id && e.Target == target))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryDanshouUseTargetEvent(use.Id, declared.TurnNumber, declared.TurnOwnerSeat, target));
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 53900)]
    internal void ObserveBoundaryDanshouHistory(IGameEvent payload)
    {
        if (!HasBoundaryDanshou || payload is BoundaryDanshouUseActorEvent or BoundaryDanshouUseTargetEvent or BoundaryDanshouDirectDuelTargetEvent)
            return;
        // The original native legacy Duel owns its attack on the producing
        // program instruction instead of manufacturing a CardUse/Action.
        if (payload is CurrentTurnCardUseKindsRecordedEvent { RoundActionType: CardActionType.Use, ProgramInstructionIndex: { } instruction } direct && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == direct.OriginFrameId)is { } producer && producer.InstructionIndex == instruction && _coreProgramVirtualDuelOperations.FrozenProgramDuelInstructionIndex(producer.Id) == instruction && producer.CardAttack is { Active: true, EffectiveCardKind: CardKind.Duel, CardId: null, PhysicalCardIds.Count: 0 } attack && producer.Continuations.Duel is { Active: true } && attack.CardUserSeat == direct.ActorSeat && !_coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryDanshouDirectDuelTargetEvent>().Any(e => e.ProducerFrameId == producer.Id && e.InstructionIndex == instruction))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryDanshouDirectDuelTargetEvent(producer.Id, instruction, direct.TurnNumber, direct.TurnOwnerSeat, direct.ActorSeat, attack.TargetSeat));
        foreach (var use in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().ToArray())
            CaptureBoundaryDanshouUse(use);
    }

    [NativeUsePreparation(53900)]
    internal bool SealBoundaryDanshouCompletedUse(CardUseFrame use)
    {
        CaptureBoundaryDanshouUse(use);
        return false;
    }

    internal int BoundaryDanshouCount(int owner, int actor, int turn, int turnOwner, long cutoff)
    {
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().Take(checked((int)cutoff)).ToArray();
        var targeted = history.OfType<BoundaryDanshouUseTargetEvent>().Where(e => e.Target == owner && e.Turn == turn && e.TurnOwner == turnOwner).Select(e => e.UseId).ToHashSet();
        return history.OfType<BoundaryDanshouUseActorEvent>().Where(e => e.Turn == turn && e.TurnOwner == turnOwner).GroupBy(e => e.UseId).Select(g => g.Last()).Count(e => e.Actor == actor && targeted.Contains(e.UseId)) + history.OfType<BoundaryDanshouDirectDuelTargetEvent>().Count(e => e.Turn == turn && e.TurnOwner == turnOwner && e.Actor == actor && e.Target == owner);
    }

    internal TurnEndingBoundaryFrame? BoundaryDanshouMother(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (c.SkillId != BoundaryZhuRanComposition.Skill || c.BindingId is not (BoundaryZhuRanComposition.Draw or BoundaryZhuRanComposition.Pay) || context is not { Window: SkillProgramTriggerWindow.TurnEnding, SourceSeat: { } actor, TargetSeat: { } target } || actor != target || actor == c.OwnerSeat || context.OwnerSeat != c.OwnerSeat || context.OccurrenceIndex != c.OccurrenceIndex || _resolutionStackOperations._resolutionStack.OfType<TurnEndingBoundaryFrame>().SingleOrDefault(p => p.Id == context.ParentFrameId)is not { } p || p.OwnerSeat != actor || p.ItemIndex < 0 || p.ItemIndex >= p.Items.Count || p.Items[p.ItemIndex].Candidate != c || context.Facts != p.Items[p.ItemIndex].Facts)
            return null;
        return p;
    }

    internal Card[] BoundaryDanshouMaterials(int owner) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[owner])).Where(card => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(owner, card, _cardZonesOperations._cardZones.GetLocation(card.Id), OwnedCardMoveIntent.Discard) && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(owner, card, _cardZonesOperations._cardZones.GetLocation(card.Id), OwnedCardMoveIntent.Discard)).ToArray();
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 53900, SkillProgramEffectOp.PrepareBoundaryZhuRanDanshou)]
    internal bool QualifyBoundaryDanshou(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (BoundaryDanshouMother(c, context)is not { } p || !_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive)
            return false;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryDanshouStartedEvent>().Any(e => e.EndingId == p.Id && e.Source.OwnerSeat == c.OwnerSeat && e.Source.SkillInstanceId == c.SkillInstanceId))
            return false;
        var x = BoundaryDanshouCount(c.OwnerSeat, p.OwnerSeat, p.TurnNumber, _coreActualTurnProgressTurnProgressionAccess._turnProgression.OwnerSeat, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().Count());
        return c.BindingId == BoundaryZhuRanComposition.Draw ? x == 0 : x > 0 && BoundaryDanshouMaterials(c.OwnerSeat).Length >= x;
    }

    SkillProgramStepOutcome global::CardGame.Core.IBoundaryZhuRanHost.Prepare(ProgramSkillFrame f) => PrepareBoundaryDanshou(f);
    internal SkillProgramStepOutcome PrepareBoundaryDanshou(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0, f.WindowContext?.OccurrenceIndex ?? 0);
        if (f.BoundaryDanshou is not null || f.InstructionIndex != 1 || f.WindowContext is not { } c || BoundaryDanshouMother(candidate, c)is not { } parent || !QualifyBoundaryDanshou(candidate, c))
            throw new InvalidOperationException("Danshou requires the original other role's Ending and its actual whole-use target history.");
        var cutoff = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().Count();
        var turnOwner = _coreActualTurnProgressTurnProgressionAccess._turnProgression.OwnerSeat;
        var x = BoundaryDanshouCount(f.OwnerSeat, parent.OwnerSeat, parent.TurnNumber, turnOwner, cutoff);
        var r = new BoundaryDanshouReceipt(BoundaryDanshouSource(f), f.GameplayHash, parent.Id, parent.TurnNumber, turnOwner, parent.OwnerSeat, cutoff, x, x == 0 ? BoundaryDanshouStage.Complete : BoundaryDanshouStage.Selecting);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryDanshou = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryDanshouStartedEvent(f.Id, r.Source, r.Hash, parent.Id, r.Turn, turnOwner, r.Actor, cutoff, x));
        if (x == 0)
            return SkillProgramStepOutcome.Continue;
        PublishBoundaryDanshou(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> BoundaryDanshouChoices(ProgramSkillFrame f)
    {
        var r = f.BoundaryDanshou!;
        var choices = new List<PromptChoice>();
        PromptChoice Choice(string branch, string label, IReadOnlyList<int>? cards = null) => new(new($"boundary-danshou.{(f.Id)}.{(branch)}.{(string.Join('-', cards ?? []))}"), label, cards ?? [], [], new Dictionary<string, string> { ["program-action"] = "boundary-danshou", ["branch"] = branch });
        var legal = BoundaryDanshouMaterials(f.OwnerSeat);
        foreach (var card in legal.Where(c => r.Selected.Contains(c.Id)))
            choices.Add(Choice("unpick", $"取消选择【{(card.DisplayName)}】", [card.Id]));
        if (r.Selected.Count < r.X)
            foreach (var card in legal.Where(c => !r.Selected.Contains(c.Id)))
                choices.Add(Choice("card", $"选择弃置【{(card.DisplayName)}】", [card.Id]));
        if (r.Selected.Count == r.X)
            choices.Add(Choice("pay", $"同时弃置{(r.X)}张牌，对{(_coreMatchParticipantsPlayersAccess._players[r.Actor].Name)}造成1点伤害"));
        choices.Add(Choice("decline", "取消弃牌"));
        return choices;
    }

    internal void PublishBoundaryDanshou(ProgramSkillFrame f)
    {
        var r = f.BoundaryDanshou!;
        var choices = BoundaryDanshouChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        var prompt = new PendingDecision(DecisionKind.ProgramTrigger, f.OwnerSeat, $"胆守：选择{(r.X)}张手牌或装备牌，确认后同时弃置。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [], r.Actor)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        var remaining = r.X - r.Selected.Count;
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = _corePlayerViewNativeCardSelectionsOperations.NativeOwnedSelection(prompt, f, "danshou-cost", "branch", ["card"], ["pay"], r.Selected, remaining, remaining);
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("boundary-danshou")]
    internal void AnswerBoundaryDanshou(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Danshou lost its original cost frame.");
        AssertBoundaryDanshou(f);
        var r = f.BoundaryDanshou!;
        if (r.Stage != BoundaryDanshouStage.Selecting || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !BoundaryDanshouChoices(f).Any(c => c.Id == choice.Id && c.Cards.SequenceEqual(choice.Cards) && c.Targets.SequenceEqual(choice.Targets)))
            throw new InvalidOperationException("Danshou rejects a stale or unrelated owned cost entity.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        switch (choice.Parameters["branch"])
        {
            case "card":
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryDanshou = r with { Selected = [..r.Selected, choice.Cards.Single()] } });
                PublishBoundaryDanshou(f);
                return;
            case "unpick":
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryDanshou = r with { Selected = r.Selected.Where(id => id != choice.Cards.Single()).ToArray() } });
                PublishBoundaryDanshou(f);
                return;
            case "decline":
                _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "取消胆守付款。");
                return;
            case "pay":
                PayBoundaryDanshou(f);
                return;
            default:
                throw new InvalidOperationException("Unknown Danshou cost option.");
        }
    }

    internal void PayBoundaryDanshou(ProgramSkillFrame f)
    {
        var r = f.BoundaryDanshou!;
        var legal = BoundaryDanshouMaterials(f.OwnerSeat).Select(c => c.Id).ToHashSet();
        if (r.Selected.Count != r.X || r.Selected.Any(id => !legal.Contains(id)) || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[r.Actor].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "胆守成本或来源已失效。");
            return;
        }

        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
            _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(player.Seat, player.SkillGrants.Revision);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryDanshou = r = r with { Stage = BoundaryDanshouStage.PaidChildren, Paid = true, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, r.Selected, new(BoundaryDanshouDiscard));
        var invoice = _coreProgramEventHistoryOperations.ProgramEventHistory<NativeLogicalDiscardInvocationRecordedEvent>().SingleOrDefault(e => e.ParentFrameId == f.Id && e.Reason.Value == BoundaryDanshouDiscard && e.SequenceBefore == r.Before);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryDanshou = r with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, DiscardInvocationId = invoice?.InvocationId, Actual = invoice?.ActualCount ?? 0 } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryDanshouPaidEvent(f.Id, r.X, invoice?.InvocationId, invoice?.ActualCount ?? 0, r.Before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        ContinueBoundaryDanshou(f.Id);
    }

    internal void ContinueBoundaryDanshou(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BoundaryDanshou: { Stage: BoundaryDanshouStage.PaidChildren } r } f || f.Id != id)
            return;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var damage = r.Actual == r.X && _coreMatchParticipantsPlayersAccess._players[r.Actor].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryDanshouCompletedEvent(f.Id, r.Actual, damage));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryDanshou = r with { Stage = BoundaryDanshouStage.Complete }, PendingMovementContinuation = null });
        if (damage)
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        else
            _coreSkillProgramsOperations.FinishProgramSkill(f, false);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 53900)]
    internal bool ResumeBoundaryDanshou(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BoundaryDanshou.Stage: BoundaryDanshouStage.PaidChildren } f || f.Id != id)
            return false;
        ContinueBoundaryDanshou(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 53900)]
    internal bool ReturnBoundaryDanshouMovement(ProgramSkillFrame f)
    {
        if (f.BoundaryDanshou?.Stage != BoundaryDanshouStage.PaidChildren)
            return false;
        ContinueBoundaryDanshou(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 53900)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 53900)]
    internal bool KeepBoundaryDanshouPaid(ProgramSkillFrame f) => f.BoundaryDanshou is { Paid: true };
    [ProgramAiChoice(SkillProgramEffectOp.PrepareBoundaryZhuRanDanshou)]
    internal static PromptChoice ChooseBoundaryDanshou(PendingDecision p) => p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "pay") ?? p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "card") ?? p.Choices.Last();
}
