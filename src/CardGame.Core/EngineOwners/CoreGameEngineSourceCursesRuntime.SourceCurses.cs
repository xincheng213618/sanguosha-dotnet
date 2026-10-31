#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSourceCursesRuntime
{
    internal bool HasSourceCurseCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.DepositSelectedSourceCurse);
    internal bool HasSourceCurseActionColors => HasSourceCurseCapability && _coreGameEngineSourceCursesSourceCurseLossesSourceCursesAccess._sourceCurses.Count > 0;

    internal SourceCurseDeposit? SourceCurseAt(int seat) => _coreGameEngineSourceCursesSourceCurseLossesSourceCursesAccess._sourceCurses.SingleOrDefault(c => c.TargetSeat == seat && _cardZonesOperations._cardZones.GetLocation(c.CardId) == c.Location);
    internal long SourceCurseMovementSequence => _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? 0;

    internal static bool SameSourceCurse(SourceCurseDeposit a, SourceCurseDeposit b) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_SourceCurses.SameSourceCurse(a, b);
    internal IReadOnlyList<SourceCurseSnapshot>? CreateSourceCurseSnapshots(int seat)
    {
        var cards = _coreGameEngineSourceCursesSourceCurseLossesSourceCursesAccess._sourceCurses.Where(c => c.TargetSeat == seat).Select(c => new SourceCurseSnapshot(c.Source.OwnerSeat, c.ActualDrawCount, _coreSnapshotValueProjectionOperations.ToSnapshot(_cardZonesOperations._cardZones.CardsAt(c.Location).Single(card => card.Id == c.CardId)))).ToArray();
        return cards.Length == 0 ? null : Array.AsReadOnly(cards);
    }

    internal void ObserveSourceCurseMovement(CardMovementRecord movement)
    {
        if (_coreGameEngineSourceCursesSourceCurseLossesSourceCursesAccess._sourceCurses.SingleOrDefault(c => c.CardId == movement.CardId && c.Location == movement.From)is not { } curse || movement.To == movement.From)
            return;
        _coreGameEngineSourceCursesSourceCurseLossesSourceCursesAccess._sourceCurses.Remove(curse);
        var loss = new SourceCurseLoss(curse, movement.TurnNumber, movement.Sequence, movement.To, movement.Reason.Value);
        _coreGameEngineSourceCursesSourceCurseLossesSourceCursesAccess._sourceCurseLosses.Add(loss);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SourceCurseLostEvent(loss));
    }

    internal Card? TakeSourceCurseForJudgment(JudgmentFrame frame)
    {
        var curse = SourceCurseAt(frame.TargetSeat);
        if (curse is null)
            return null;
        var card = _cardZonesOperations._cardZones.CardsAt(curse.Location).Single(c => c.Id == curse.CardId);
        _coreNativeJudgmentOperations.ReplaceJudgmentFrame(frame with { SourceCurseOrigin = curse });
        // The issued foreign entity survives loss of its issuer. It is a real
        // initial judgment card, before any standard replacement opportunity.
        _corePhysicalMovementFacadesOperations.MoveCard(card, curse.Location, CardLocation.Processing, new("program.source-curse.judgment"));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SourceCurseJudgmentIssuedEvent(frame.Id, curse));
        return card;
    }

    internal static bool MatchesSourceCurseCard(SourceCurseDeposit curse, CardActionContext action, SkillProgramEffect effect) => effect.SourceCurseMatchSuit == true ? action.EffectiveSuit is { } suit && suit != Suit.None && suit == curse.PrintedSuit : action.EffectiveIsRed is { } color && global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(curse.PrintedSuit) == color;
    internal SourceCurseDeposit? EligibleSourceCurseUse(int owner, string skill, string instance, CardActionContext action, SkillProgramEffect effect)
    {
        if (action.Type != CardActionType.Use || !_coreMatchParticipantsPlayersAccess._players[owner].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[owner], skill, instance))
            return null;
        var curse = SourceCurseAt(action.ActorSeat);
        if (curse is null || curse.Source.OwnerSeat != owner || curse.Source.SkillId != effect.SkillIds.Single() || curse.BenefitSkillId != skill || curse.BenefitSkillInstanceId != instance || curse.BenefitGameplayHash != _contentRegistryOperations._contentRegistry.GetSkill(skill).Program?.GameplayHash || !MatchesSourceCurseCard(curse, action, effect) || curse.ActualDrawCount >= 2)
            return null;
        // The locked reward has its own captured source. Suppressing nonlocked
        // Zhoufu must not disable this already issued curse's Yingbing reward.
        return curse;
    }

    internal SourceCurseLoss[] EligibleSourceCurseLosses(int owner, string skill, string instance, string hash) => _coreGameEngineSourceCursesSourceCurseLossesSourceCursesAccess._sourceCurseLosses.Where(l => l.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && l.Deposit.Source.OwnerSeat == owner && l.Deposit.Source.SkillId == skill && l.Deposit.Source.SkillInstanceId == instance && l.Deposit.GameplayHash == hash).DistinctBy(l => l.Deposit.TargetSeat).OrderBy(l => _coreTableOrderOperations.RelativeTableOrder(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, l.Deposit.TargetSeat)).ToArray();
    internal bool SourceCurseHasFinalGameEnd()
    {
        if (_coreMatchOutcomeStatusWinnerAccess._status != EngineStatus.Completed)
            return false;
        if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<GameEndedEvent>().Any(e => e.Winner == _coreMatchOutcomeStatusWinnerAccess._winner))
            throw new InvalidOperationException("A terminal curse diagnostic requires the actual final game-end fact.");
        return true;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 90)]
    internal bool CanRunSourceCurseTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        // This shared hook must leave existing non-curse trigger behavior unchanged.
        if (!trigger.Effects.Any(e => e.Op is SkillProgramEffectOp.DrawForSourceCurseUse or SkillProgramEffectOp.LoseHpForLostSourceCurses))
            return true;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _coreMatchOutcomeStatusWinnerAccess._status == EngineStatus.Completed)
            return false;
        if (trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.DrawForSourceCurseUse))
        {
            if (context.Window != SkillProgramTriggerWindow.CardUseCommitted || context.SourceCurseUse is not { } frozen || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().LastOrDefault(w => w.Id == context.ParentFrameId)is not { } parent || parent.Action.ActionId != frozen.ActionId || parent.Action.ActorSeat != frozen.ActorSeat || parent.Action.EffectiveIsRed != frozen.EffectiveIsRed || parent.Action.Type != CardActionType.Use || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(parent.Candidates[parent.CandidateIndex]) != candidate)
                return false;
            var current = EligibleSourceCurseUse(candidate.OwnerSeat, candidate.SkillId, candidate.SkillInstanceId, parent.Action, trigger.Effects.Single());
            return current is not null && SameSourceCurse(current, frozen.Deposit) && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<SourceCurseDrawIssuedEvent>().Any(e => e.Deposit.DepositFrameId == current.DepositFrameId && e.ActionId == frozen.ActionId);
        }

        if (!trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.LoseHpForLostSourceCurses))
            return true;
        return context.Window == SkillProgramTriggerWindow.AfterTurnEnded && _resolutionStackOperations._resolutionStack.OfType<DeferredTurnEndFrame>().LastOrDefault(w => w.Id == context.ParentFrameId)is { } actual && _coreAfterTurnEndedProgramsOperations.IsActualAfterTurnEndedParent(actual) && _coreAfterTurnEndedProgramsOperations.AfterTurnEndedCandidate(actual) == candidate && EligibleSourceCurseLosses(candidate.OwnerSeat, candidate.SkillId, candidate.SkillInstanceId, candidate.GameplayHash).Length > 0 && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<SourceCurseLossRosterIssuedEvent>().Any(e => e.ActualTurn == actual.TurnNumber && e.Losses.Any(l => l.Deposit.Source.OwnerSeat == candidate.OwnerSeat && l.Deposit.Source.SkillId == candidate.SkillId && l.Deposit.Source.SkillInstanceId == candidate.SkillInstanceId));
    }

    internal SkillProgramStepOutcome ExecuteSourceCurse(SkillProgramEffect effect, ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.SourceCurseReceipt is not null)
            throw new InvalidOperationException("A curse cannot issue a second paid receipt.");
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _coreMatchOutcomeStatusWinnerAccess._status == EngineStatus.Completed || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            return SkillProgramStepOutcome.Continue;
        if (effect.Op == SkillProgramEffectOp.DepositSelectedSourceCurse)
            return DepositSourceCurse(f, effect);
        if (effect.Op == SkillProgramEffectOp.DrawForSourceCurseUse)
            return DrawSourceCurse(f, effect);
        if (effect.Op != SkillProgramEffectOp.LoseHpForLostSourceCurses || f.WindowContext is not { } context || _resolutionStackOperations._resolutionStack.OfType<DeferredTurnEndFrame>().LastOrDefault(w => w.Id == context.ParentFrameId)is not { } parent || !CanRunSourceCurseTrigger(_coreAfterTurnEndedProgramsOperations.AfterTurnEndedCandidate(parent), _coreProgramLifecycleOperations.GetProgramTrigger(f), context))
            throw new InvalidOperationException("A curse loss roster requires its exact actual turn-end candidate.");
        var losses = EligibleSourceCurseLosses(f.OwnerSeat, f.SkillId, f.SkillInstanceId, f.GameplayHash);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SourceCurseReceipt = new() { InstructionIndex = f.InstructionIndex, Stage = SourceCurseStage.LossHpChildren, OriginalParentId = parent.Id, ActualTurn = parent.TurnNumber, Losses = losses } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SourceCurseLossRosterIssuedEvent(f.Id, parent.Id, parent.TurnNumber, Array.AsReadOnly(losses)));
        ResumeSourceCurse(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal SkillProgramStepOutcome DepositSourceCurse(ProgramSkillFrame f, SkillProgramEffect effect)
    {
        if (f.TriggerId is not null || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != f.OwnerSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || f.SelectedCardIds is not [var id] || f.SelectedTargetSeats is not [var target] || target == f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || SourceCurseAt(target)is not null)
            throw new InvalidOperationException("A curse deposit lost its exact one-card other-target Play selection.");
        var from = _cardZonesOperations._cardZones.GetLocation(id);
        var card = _cardZonesOperations._cardZones.CardsAt(from).Single(c => c.Id == id);
        if (from.OwnerSeat != f.OwnerSeat || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || !_contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!.Activations.Single(a => a.Id == f.ActivationId).SourceZones.Contains(from.Zone) || card.IsGeneralWeapon)
            throw new InvalidOperationException("A curse requires one transferable owner HE entity.");
        var sourceGrant = _coreNationalSkillsOperations.EnabledRuntimeSkillGrants(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Single(g => g.SkillId == f.SkillId && g.SkillInstanceId == f.SkillInstanceId);
        var benefitSkill = effect.SkillIds.Single();
        var benefit = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].SkillGrants.Grants.Where(g => g.SkillId == benefitSkill && g.SourceId == sourceGrant.SourceId).OrderBy(g => g.SkillInstanceId, StringComparer.Ordinal).FirstOrDefault();
        var deposit = new SourceCurseDeposit(f.Id, new(f.SkillId, f.ActivationId, f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, target, card.Id, card.Kind, card.Suit, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, benefitSkill, benefit?.SkillInstanceId, benefit is null ? null : _contentRegistryOperations._contentRegistry.GetSkill(benefitSkill).Program!.GameplayHash);
        _cardZonesOperations._cardZones.EnsurePublicPersistentPile(deposit.Location);
        _coreGameEngineSourceCursesSourceCurseLossesSourceCursesAccess._sourceCurses.Add(deposit);
        var before = SourceCurseMovementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SourceCurseReceipt = new() { InstructionIndex = f.InstructionIndex, Stage = SourceCurseStage.DepositChildren, Deposit = deposit, PaymentFrom = from, Before = before, After = before, ActualTurn = _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _corePhysicalMovementFacadesOperations.MoveCard(card, from, deposit.Location, new("program.source-curse.deposit"));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SourceCurseReceipt = f.SourceCurseReceipt!with { After = SourceCurseMovementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SourceCurseDepositedEvent(deposit, from, _coreCardMovementJournalCardMovementsAccess._cardMovements.Single(m => m.Sequence > before && m.Sequence <= SourceCurseMovementSequence && m.CardId == id && m.From == from && m.To == deposit.Location).Sequence));
        if (!_coreFireTargetBenefitsOperations.TryDrainFireTargetMovement(f))
            ReturnSourceCurseMovement(f);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal SkillProgramStepOutcome DrawSourceCurse(ProgramSkillFrame f, SkillProgramEffect effect)
    {
        if (f.WindowContext is not { SourceCurseUse: { } identity } context || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().LastOrDefault(w => w.Id == context.ParentFrameId)is not { } parent || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !CanRunSourceCurseTrigger(global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(parent.Candidates[parent.CandidateIndex]), _coreProgramLifecycleOperations.GetProgramTrigger(f), context) || EligibleSourceCurseUse(f.OwnerSeat, f.SkillId, f.SkillInstanceId, parent.Action, effect)is not { } deposit)
            throw new InvalidOperationException("Yingbing lost its original genuine use, exact curse and independent locked source.");
        var before = SourceCurseMovementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SourceCurseReceipt = new() { InstructionIndex = f.InstructionIndex, Stage = SourceCurseStage.DrawChildren, Deposit = deposit, ActionId = identity.ActionId, OriginalParentId = parent.Id, Before = before, After = before, ActualTurn = _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var drawn = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], 1, true, new("program.source-curse.draw"));
        // Count actual physical draws, before draining their paid child subtree.
        var updated = deposit with
        {
            ActualDrawCount = deposit.ActualDrawCount + drawn.Count
        };
        if (_coreGameEngineSourceCursesSourceCurseLossesSourceCursesAccess._sourceCurses.Contains(deposit))
            _coreGameEngineSourceCursesSourceCurseLossesSourceCursesAccess._sourceCurses[_coreGameEngineSourceCursesSourceCurseLossesSourceCursesAccess._sourceCurses.IndexOf(deposit)] = updated;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SourceCurseReceipt = f.SourceCurseReceipt!with { Deposit = updated, After = SourceCurseMovementSequence, DrawCount = drawn.Count } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SourceCurseDrawIssuedEvent(f.Id, parent.Id, updated, identity.ActionId, drawn.Count, before, SourceCurseMovementSequence));
        if (!_coreFireTargetBenefitsOperations.TryDrainFireTargetMovement(f))
            ReturnSourceCurseMovement(f);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 290)]
    internal bool ResumeSourceCurse(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { SourceCurseReceipt: { } r } f || f.Id != id)
            return false;
        // Genuine global termination leaves paid frames as diagnostics. In
        // particular do not push a queued recovery attempt after GameEnded.
        if (SourceCurseHasFinalGameEnd())
            return true;
        if (f.PendingMovementContinuation is not null)
        {
            if (!_coreFireTargetBenefitsOperations.TryDrainFireTargetMovement(f))
                ReturnSourceCurseMovement(f);
            return true;
        }

        if (r.Stage != SourceCurseStage.LossHpChildren)
            throw new InvalidOperationException("A curse movement receipt resumed without its paid movement return.");
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.Program))
            return true;
        while (r.LossIndex < r.Losses.Count && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None)
        {
            var target = r.Losses[r.LossIndex].Deposit.TargetSeat;
            var index = r.LossIndex;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SourceCurseReceipt = r = r with { LossIndex = index + 1, CurrentHpTarget = target } });
            if (!_coreMatchParticipantsPlayersAccess._players[target].IsAlive || _coreMatchParticipantsPlayersAccess._players[target].Hp <= 0)
                continue;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SourceCurseHpLossIssuedEvent(f.Id, r.ActualTurn, target, index));
            if (_kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(f.Id, f.SkillId, target, 1) == SkillProgramStepOutcome.AwaitChild)
                return true;
            if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.Program))
                return true;
        }

        FinishSourceCurse(f);
        return true;
    }

    internal void FinishSourceCurse(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SourceCurseReceipt = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 280)]
    internal bool ReturnSourceCurseMovement(ProgramSkillFrame f)
    {
        if (f.SourceCurseReceipt is not { } r)
            return false;
        if (f.PendingMovementContinuation is not { } pending || !IsSourceCurseMovement(f, SourceCursePausedEffect(f), pending))
            throw new InvalidOperationException("A curse cannot consume a different movement return.");
        if (SourceCurseHasFinalGameEnd())
            return true;
        if (_coreFireTargetBenefitsOperations.TryDrainFireTargetMovement(f))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (r.Stage == SourceCurseStage.DrawChildren && r.DrawCount == 1 && r.Deposit is { ActualDrawCount: 2 } due && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && SourceCurseAt(due.TargetSeat)is { } live && SameSourceCurse(live, due))
        {
            var before = SourceCurseMovementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SourceCurseReceipt = r with { Stage = SourceCurseStage.ObtainChildren, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            var discard = SourceCursePausedEffect(f)!.SourceCurseDiscardAfterSecondDraw == true;
            _corePhysicalMovementFacadesOperations.MoveCard(_cardZonesOperations._cardZones.CardsAt(due.Location).Single(c => c.Id == due.CardId), due.Location, discard ? CardLocation.DiscardPile : CardLocation.Hand(f.OwnerSeat), new(discard ? "program.source-curse.remove" : "program.source-curse.obtain"));
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SourceCurseReceipt = f.SourceCurseReceipt!with { After = SourceCurseMovementSequence } });
            if (discard)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SourceCurseRemovedEvent(f.Id, due, before, SourceCurseMovementSequence));
            else
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SourceCurseObtainedEvent(f.Id, due, before, SourceCurseMovementSequence));
            if (!_coreFireTargetBenefitsOperations.TryDrainFireTargetMovement(f))
                ReturnSourceCurseMovement(f);
            return true;
        }

        FinishSourceCurse(f);
        return true;
    }

    internal SkillProgramEffect? SourceCursePausedEffect(ProgramSkillFrame f) => f.InstructionIndex < 1 ? null : ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).GetPausedInstruction(f.InstructionIndex).Effect;
    internal bool IsSourceCurseMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => f.SourceCurseReceipt is { Stage: not SourceCurseStage.LossHpChildren } r && r.InstructionIndex == f.InstructionIndex && effect == SourceCursePausedEffect(f) && effect?.Op == (r.Stage == SourceCurseStage.DepositChildren ? SkillProgramEffectOp.DepositSelectedSourceCurse : SkillProgramEffectOp.DrawForSourceCurseUse) && pending.SubjectSeat == f.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null;
    SkillProgramStepOutcome global::CardGame.Core.ISourceCurseProgramHost.ExecuteSourceCurse(SkillProgramEffect effect, ProgramSkillFrame frame) => ExecuteSourceCurse(effect, frame);
}
