#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuPanOlRuntime
{
    internal const string PijingGiftReason = "skill-program.liu-pan.pijing.random-gift", PijingDrawReason = "skill-program.liu-pan.pijing.next-use-draw";
    SkillProgramStepOutcome global::CardGame.Core.ILiuPanHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginPijing(f, op);
    internal SkillProgramStepOutcome BeginPijing(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var use = RequirePijingUse(f, out var window);
        var next = op == SkillProgramEffectOp.ResolvePijingNextSoleTargetChoice;
        var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0);
        if (f.Pijing is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { } context || (!next ? !PijingInitialEligible(candidate, context) : !IsIssuedPijingCandidate(candidate, context)))
            throw new InvalidOperationException("Pijing cannot replace or issue an ineligible original target/lease resolution.");
        var group = next ? PijingIssuedGroup(candidate, context, out _, out _) : null;
        var x = next ? 1 : Math.Max(1, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp - _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp);
        var actor = group?.HolderSeat ?? use.SourceSeat;
        var r = new PijingReceipt(PijingSource(f), f.GameplayHash, use.Id, use.Action!.ActionId, window.Id, _coreActualTurnProgressTurnNumberAccess._turnNumber, actor, use.Action.ProviderSeat, x, next, group?.GroupId ?? 0, next ? PijingStage.Reciprocal : PijingStage.Toggles, use.TargetSeats, [], group?.Grants ?? []);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Pijing = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PijingStartedEvent(f.Id, r.Source, r.GameplayHash, r.UseId, r.ActionId, r.WindowId, r.ActualTurn, r.ActorSeat, r.ProviderSeat, r.X, r.Reciprocal, r.GroupId, r.Original.Count));
        for (var i = 0; i < r.Original.Count; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PijingTargetSlotEvent(f.Id, false, i, r.Original[i]));
        if (next)
            foreach (var id in r.Grants)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PijingGrantConsumedEvent(f.Id, r.UseId, r.GroupId, id));
        PublishPijing(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<Card> PijingGiftCards(int donor, int recipient) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[donor]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment2(donor)).Where(c => !c.IsGeneralWeapon && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(recipient, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Transfer)).OrderBy(c => _cardZonesOperations._cardZones.GetLocation(c.Id).Zone).ThenBy(c => c.Id).ToArray());
    internal void IssuePijingGift(ProgramSkillFrame f)
    {
        var r = f.Pijing!;
        var donor = r.Toggles[r.Index].Seat;
        var cards = _coreMatchParticipantsPlayersAccess._players[donor].IsAlive && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive ? PijingGiftCards(donor, f.OwnerSeat) : [];
        if (cards.Count == 0)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PijingGiftSkippedEvent(f.Id, r.Index, donor));
            CompletePijingGift(f);
            return;
        }

        var card = cards[_randomOperations._random.Next(cards.Count)];
        var from = _cardZonesOperations._cardZones.GetLocation(card.Id);
        var selected = new PijingGiftSelectedEvent(f.Id, r.Index, donor, card.Id, card.Kind, from);
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Pijing = r with { Stage = PijingStage.GiftChildren, Gift = selected, Before = before, After = before, Count = 1 }, PendingMovementContinuation = new(donor, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(selected);
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([card.Id], CardLocation.Hand(f.OwnerSeat), new(PijingGiftReason), _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, donor, f.OwnerSeat, [card.Id], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, (batch, moves) =>
        {
            if (moves is not [var m] || m.CardId != card.Id || m.From != from || m.To != CardLocation.Hand(f.OwnerSeat))
                throw new InvalidOperationException("Pijing lost its one actual random transferable HE entity.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PijingGiftPaidEvent(f.Id, r.Index, donor, card.Id, from, f.OwnerSeat, batch, m.Sequence, before, m.Sequence));
        }));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Pijing = f.Pijing!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PijingMovementPaidEvent(f.Id, PijingStage.GiftChildren, r.Index, donor, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, 1));
        ContinuePijing(f.Id);
    }

    internal void CompletePijingGift(ProgramSkillFrame f)
    {
        var r = f.Pijing!;
        var grantId = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var donor = r.Toggles[r.Index].Seat;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PijingGrantIssuedEvent(grantId, r.Source, r.GameplayHash, donor, r.UseId, r.ActionId, r.ActualTurn));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PijingGiftCompletedEvent(f.Id, r.Index, grantId));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Pijing = r with { Index = r.Index + 1, Stage = PijingStage.Gifts, Gift = null, Before = 0, After = 0, Count = 0 }, PendingMovementContinuation = null });
        ContinuePijing(f.Id);
    }

    internal void DrawPijingReciprocal(ProgramSkillFrame f)
    {
        var r = f.Pijing!;
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Pijing = r with { Stage = PijingStage.DrawChildren, Before = before, After = before, Count = 0 }, PendingMovementContinuation = new(r.ActorSeat, 0, null) });
        var count = _coreMatchParticipantsPlayersAccess._players[r.ActorSeat].IsAlive ? _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[r.ActorSeat], 1, true, new(PijingDrawReason)).Count : 0;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Pijing = f.Pijing!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Count = count } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PijingMovementPaidEvent(f.Id, PijingStage.DrawChildren, 0, r.ActorSeat, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, count));
        ContinuePijing(f.Id);
    }

    internal bool DrainPijing(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinuePijing(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Pijing: { } r } f || f.Id != id)
            return;
        if (r.Stage is PijingStage.Toggles or PijingStage.Reciprocal)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishPijing(f);
            return;
        }

        if (r.Stage == PijingStage.Gifts)
        {
            if (r.Index >= r.Toggles.Count)
            {
                FinishPijing(f);
                return;
            }

            IssuePijingGift(f);
            return;
        }

        if (DrainPijing(f))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        if (r.Stage == PijingStage.GiftChildren)
            CompletePijingGift(f);
        else
            FinishPijing(f);
    }

    internal void FinishPijing(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PijingCompletedEvent(f.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Pijing = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27410)]
    internal bool ResumePijing(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Pijing: not null } f || f.Id != id)
            return false;
        ContinuePijing(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 27410)]
    internal bool ReturnPijingMovement(ProgramSkillFrame f)
    {
        if (f.Pijing is null)
            return false;
        ContinuePijing(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27410)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27410)]
    internal bool ContinuePaidPijing(ProgramSkillFrame f)
    {
        if (f.Pijing is null)
            return false;
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27411)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27411)]
    internal bool ContinueIssuedPijing(ProgramSkillFrame f) => f.Pijing is null && f.InstructionIndex is 0 or 1 && f.TriggerId == "next-sole-use" && f.WindowContext is { } c && IsIssuedPijingCandidate(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, 0), c);
    [ProgramAiChoice(SkillProgramEffectOp.ResolvePijingTargetTogglesAndRandomGifts, SkillProgramEffectOp.ResolvePijingNextSoleTargetChoice)]
    internal PromptChoice ChoosePijing(PendingDecision p, ProgramSkillFrame f)
    {
        var r = f.Pijing!;
        if (r.Reciprocal)
            return p.Choices.First(c => c.Parameters.GetValueOrDefault("branch") == "draw");
        var use = RequirePijingUse(f, out _);
        var helpful = use.CardKind is CardKind.DrawTwo or CardKind.PeachGarden or CardKind.FiveGrains;
        return p.Choices.FirstOrDefault(c => c.Targets.Count > 0 && (c.Parameters.GetValueOrDefault("branch") == "remove" ? !helpful && global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[c.Targets[0]]) : helpful == global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[c.Targets[0]]))) ?? p.Choices.Last();
    }
}
