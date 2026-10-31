#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGanNingHappyGodRuntime
{
    internal const string HappyPoxiReason = "skill-program.happy:poxi.discard", HappyPoxiDrawReason = "skill-program.happy:poxi.draw";
    SkillProgramStepOutcome global::CardGame.Core.IHappyPoxiHost.Resolve(ProgramSkillFrame frame, string bind) => BeginHappyPoxi(frame, bind);
    internal SkillProgramStepOutcome BeginHappyPoxi(ProgramSkillFrame supplied, string bind)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var cards = _coreProgramLifecycleOperations.GetProgramCardSet(frame, bind);
        if (cards.CardIds.Count == 0)
            return SkillProgramStepOutcome.Continue;
        if (frame.HappyPoxi is not null || frame.InstructionIndex != 2 || frame.SelectedTargetSeats is not [var target] || cards.CardIds.Count != 3 || cards.CardIds.Distinct().Count() != 3 || cards.SourceLocations.Count != 3 || cards.SourceLocations.Any(from => from.Zone != CardZoneKind.Hand || from.OwnerSeat != frame.OwnerSeat && from.OwnerSeat != target) || cards.CardIds.Where((id, index) => _cardZonesOperations._cardZones.GetLocation(id) != cards.SourceLocations[index]).Any() || cards.CardIds.Select(id => _coreCardAttackStateOperations.GetAttackCard(id).Suit).Distinct().Count() != 3)
            throw new InvalidOperationException("Happy Poxi lost its three different-suit original hand entities and fixed other participant.");
        if (cards.CardIds.Select((id, index) => (Card: _coreCardAttackStateOperations.GetAttackCard(id), From: cards.SourceLocations[index])).Any(item => _coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(frame.OwnerSeat, item.Card, item.From, OwnedCardMoveIntent.Discard) || _coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(frame.OwnerSeat, item.Card, item.From, OwnedCardMoveIntent.Discard)))
            return SkillProgramStepOutcome.Continue;
        var receipt = new HappyPoxiReceipt(new(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash, target, cards.SourceLocations.Count(from => from.OwnerSeat == frame.OwnerSeat), _coreActualTurnProgressTurnNumberAccess._turnNumber, HappyPoxiStage.DiscardChildren)
        {
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { HappyPoxi = receipt, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(frame, frame.OwnerSeat, cards.CardIds, new(HappyPoxiReason), (batch, moves) =>
        {
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
            var paid = receipt with
            {
                BatchId = batch,
                After = moves[^1].Sequence,
                Cost = moves.Select(m => new ShenLuSuMoveInvoice(m.CardId, m.From, m.To, m.Sequence)).ToArray()
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { HappyPoxi = paid });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyPoxiPaidEvent(frame.Id, paid.Source, paid.GameplayHash, target, paid.OwnCount, paid.ActualTurn, batch, paid.Before, paid.After));
        });
        ContinueHappyPoxi(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void ContinueHappyPoxi(long id)
    {
        while (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { HappyPoxi: { } receipt } frame && frame.Id == id)
        {
            if (receipt.Stage == HappyPoxiStage.Drawing)
                return;
            if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
                return;
            frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            receipt = frame.HappyPoxi!;
            if (frame.PendingMovementContinuation is not null)
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PendingMovementContinuation = null });
            if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || receipt.Stage is HappyPoxiStage.MaximumChildren or HappyPoxiStage.DrawChildren)
            {
                FinishHappyPoxi(frame);
                return;
            }

            if (receipt.Stage == HappyPoxiStage.RecoveryChildren || receipt.Stage == HappyPoxiStage.DiscardChildren && receipt.OwnCount == 3)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { HappyPoxi = receipt with { Stage = HappyPoxiStage.Drawing } });
                _coreProgramNativeDrawOperations.BeginOperationProgramDraw(frame, frame.OwnerSeat, receipt.OwnCount == 2 ? 1 : 3, new(HappyPoxiDrawReason));
                return;
            }

            if (receipt.Stage != HappyPoxiStage.DiscardChildren)
                throw new InvalidOperationException("Happy Poxi cannot repeat a paid outcome stage.");
            var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
            if (receipt.OwnCount == 1)
            {
                FinishHappyPoxi(frame);
                return;
            }

            if (receipt.OwnCount == 0)
            {
                var before = owner.MaxHp;
                var hp = owner.Hp;
                var after = _coreWushuangTacticRulesOperations.HasWushuangTactic(owner.Seat, 2020) ? 7 : Math.Max(0, before - 1);
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { HappyPoxi = receipt with { Stage = HappyPoxiStage.MaximumChildren, MaximumBefore = before, MaximumAfter = after }, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
                owner.MaxHp = after;
                owner.Hp = Math.Min(hp, after);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MaximumHpChangedEvent(owner.Seat, after - before, after, frame.SkillId) { HpBeforeClamp = hp });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyPoxiBenefitIssuedEvent(frame.Id, receipt.OwnCount, before, after));
                if (after == 0 && owner.IsAlive)
                    _coreNativePlayerDeathOperations.BeginPlayerDeath(frame.Id, owner, null, null, null, causingProgramSkillFrameId: frame.Id);
                continue;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { HappyPoxi = receipt with { Stage = HappyPoxiStage.RecoveryChildren }, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyPoxiBenefitIssuedEvent(frame.Id, receipt.OwnCount, owner.MaxHp, owner.MaxHp));
            _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(frame.Id, frame.OwnerSeat, frame.OwnerSeat, 1, null, null);
        }
    }

    internal void FinishHappyPoxi(ProgramSkillFrame frame)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { HappyPoxi = null, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 41680)]
    internal bool ResumeHappyPoxi(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HappyPoxi: not null } frame || frame.Id != id)
            return false;
        ContinueHappyPoxi(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 41680)]
    internal bool ReturnHappyPoxiMovement(ProgramSkillFrame frame) => frame.HappyPoxi is not null && ResumeHappyPoxi(frame.Id);
    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveHappyGanNingPoxi)]
    internal void ReturnHappyPoxiDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var receipt = frame.HappyPoxi ?? throw new InvalidOperationException("Happy Poxi lost its native reward draw parent.");
        if (receipt.Stage != HappyPoxiStage.Drawing || outcome.RecipientSeat != frame.OwnerSeat || outcome.RequestedCount != (receipt.OwnCount == 2 ? 1 : 3) || outcome.SequenceAfter != _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _coreProgramEventHistoryOperations.ProgramEventHistory<HappyPoxiDrawReturnedEvent>().Any(e => e.FrameId == frame.Id))
            throw new InvalidOperationException("Happy Poxi cannot change or repeat its already-issued native draw reward.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { HappyPoxi = receipt with { Stage = HappyPoxiStage.DrawChildren, DrawBefore = outcome.SequenceBefore, DrawAfter = outcome.SequenceAfter, DrawCount = outcome.ActualDrawIds.Count }, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyPoxiDrawReturnedEvent(frame.Id, outcome.RequestedCount, outcome.ActualDrawIds.Count, outcome.SequenceBefore, outcome.SequenceAfter));
        ContinueHappyPoxi(frame.Id);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 41680)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 41680)]
    internal bool KeepHappyPoxiPaid(ProgramSkillFrame frame) => frame.HappyPoxi is not null;
}
