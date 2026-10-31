#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterQiaoRuiAnniversaryRuntime
{
    internal const string QiaoRuiAction = "anniversary-qiao-rui", QiaoRuiDraw = "skill-program.anniversary:aishou.draw", QiaoRuiDiscard = "skill-program.anniversary:aishou.discard", QiaoRuiClaim = "skill-program.anniversary:saowei.obtain";
    internal static CardConversionSource QiaoRuiSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal bool HasAnniversaryQiaoRuiCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveAnniversaryQiaoRuiAishouDraw);

    SkillProgramStepOutcome global::CardGame.Core.IAnniversaryQiaoRuiHost.Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginQiaoRui(f, op);
    internal int[] QiaoRuiMarkedHand(int owner, long cutoff)
    {
        if (!HasAnniversaryQiaoRuiCapability)
            return[];
        var hand = CardLocation.Hand(owner);
        var entries = new HashSet<(long Sequence, int CardId, CardKind Kind)>();
        var lastExits = new Dictionary<int, long>();
        // Share the same cutoff scan across all issued marks. Keeping the exact
        // entry and latest departure preserves residency even after reacquisition.
        foreach (var movement in _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements)
        {
            if (movement.Sequence > cutoff)
                continue;
            if (movement.From == CardLocation.DrawPile && movement.To == hand)
                entries.Add((movement.Sequence, movement.CardId, movement.CardKind));
            if (movement.From == hand && movement.To != movement.From && (!lastExits.TryGetValue(movement.CardId, out var last) || movement.Sequence > last))
                lastExits[movement.CardId] = movement.Sequence;
        }

        return _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryQiaoRuiMarkedEvent>().Where(mark => mark.Source.OwnerSeat == owner && mark.EntrySequence <= cutoff && entries.Contains((mark.EntrySequence, mark.CardId, mark.PrintedKind)) && (!lastExits.TryGetValue(mark.CardId, out var last) || last <= mark.EntrySequence)).Select(mark => mark.CardId).Distinct().Order().ToArray();
    }

    internal bool QiaoRuiLastLoss(int owner, CardMovementBatchContext b)
    {
        if (b.TurnNumber <= 0 || b.MovementTiming is not { } timing || timing.ActualTurnOwnerSeat == owner || b.Movements.Count == 0 || b.Movements.Any(m => !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m)) || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryQiaoRuiMaximumHpEvent>().Any(e => e.LossBatchId == b.Id && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryQiaoRuiStartedEvent>().Any(s => s.FrameId == e.FrameId && s.Source.OwnerSeat == owner)))
            return false;
        var before = b.Movements.Min(m => m.Sequence) - 1;
        var after = b.Movements.Max(m => m.Sequence);
        var marks = QiaoRuiMarkedHand(owner, before);
        return marks.Length > 0 && QiaoRuiMarkedHand(owner, after).Length == 0 && b.Movements.Any(m => marks.Contains(m.CardId) && m.From == CardLocation.Hand(owner) && m.To != m.From);
    }

    internal CardUseFrame? QiaoRuiOriginalSlash(ProgramSkillWindowContext? w) => w is { Window: SkillProgramTriggerWindow.CardUseCompleted, CardUse: { } c } && _coreCardUseLifecycleOperations.LifecycleCardUse(c.ParentCardUseFrameId)is { Step: ResolutionFrameStep.Completed, Action: { Type: CardActionType.Use } a } u && a.ActionId == c.CardActionId && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(a.EffectiveKind) ? u : null;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 49400, SkillProgramEffectOp.ResolveAnniversaryQiaoRuiAishouDraw, SkillProgramEffectOp.ResolveAnniversaryQiaoRuiAishouPrepare, SkillProgramEffectOp.ResolveAnniversaryQiaoRuiAishouLastLoss, SkillProgramEffectOp.ResolveAnniversaryQiaoRuiSaowei)]
    internal bool CanOfferQiaoRui(ProgramTriggerLegalityContext c) => c.Trigger.Effects[0].Op switch
    {
        SkillProgramEffectOp.ResolveAnniversaryQiaoRuiAishouDraw => c.Context.Window == SkillProgramTriggerWindow.TurnEnding && c.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat,
        SkillProgramEffectOp.ResolveAnniversaryQiaoRuiAishouPrepare => c.Context.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && c.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && QiaoRuiMarkedHand(c.OwnerSeat, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence).Length > 0,
        SkillProgramEffectOp.ResolveAnniversaryQiaoRuiAishouLastLoss => c.Context.MovementBatch is { } b && QiaoRuiLastLoss(c.OwnerSeat, b),
        _ => QiaoRuiOriginalSlash(c.WindowContext)is { Action: { } a } && a.ActorSeat != c.OwnerSeat && QiaoRuiPlans(c.OwnerSeat, QiaoRuiSourceForCandidate(c.Candidate), a.TargetSeats).Count > 0
    };
    internal static CardConversionSource QiaoRuiSourceForCandidate(ProgramTriggerCandidate c) => new(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId);
    internal SkillProgramStepOutcome BeginQiaoRui(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.AnniversaryQiaoRui is not null || f.InstructionIndex != 1 || !QiaoRuiOriginal(f, op))
            throw new InvalidOperationException("Qiao Rui needs his original preparation, ending, actual loss or completed Slash candidate.");
        var stage = op == SkillProgramEffectOp.ResolveAnniversaryQiaoRuiAishouDraw ? AnniversaryQiaoRuiStage.Drawing : op == SkillProgramEffectOp.ResolveAnniversaryQiaoRuiAishouPrepare ? AnniversaryQiaoRuiStage.DiscardChildren : op == SkillProgramEffectOp.ResolveAnniversaryQiaoRuiAishouLastLoss ? AnniversaryQiaoRuiStage.HealthChildren : AnniversaryQiaoRuiStage.SlashChoice;
        var r = new AnniversaryQiaoRuiReceipt(QiaoRuiSource(f), f.GameplayHash, op, f.WindowContext!.ParentFrameId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, stage)
        {
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        if (op == SkillProgramEffectOp.ResolveAnniversaryQiaoRuiAishouDraw)
            r = r with
            {
                Requested = Math.Max(0, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp)
            };
        if (op == SkillProgramEffectOp.ResolveAnniversaryQiaoRuiAishouPrepare)
            r = r with
            {
                Cards = QiaoRuiMarkedHand(f.OwnerSeat, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence).Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(f.OwnerSeat) && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(f.OwnerSeat, _coreAdvancedLifecycleOperations.GetAdvancedCard(id), CardLocation.Hand(f.OwnerSeat), OwnedCardMoveIntent.Discard)).ToArray()
            };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryQiaoRui = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryQiaoRuiStartedEvent(f.Id, r.Source, r.Hash, op, r.ParentId, r.Turn));
        if (stage == AnniversaryQiaoRuiStage.Drawing)
        {
            if (r.Requested == 0)
                FinishQiaoRui(f);
            else
                _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, r.Requested, new(QiaoRuiDraw));
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (stage == AnniversaryQiaoRuiStage.DiscardChildren)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, r.Cards, new(QiaoRuiDiscard), (_, _) =>
            {
                var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                _coreRuntimeOperations.ReplaceRuntimeTop(current with { AnniversaryQiaoRui = current.AnniversaryQiaoRui!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            });
            ContinueQiaoRui(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (stage == AnniversaryQiaoRuiStage.HealthChildren)
        {
            if (!QiaoRuiLastLoss(f.OwnerSeat, f.WindowContext.MovementBatch!))
                throw new InvalidOperationException("Aishou lost the exact final outside-turn marked Hand departure.");
            ChangeQiaoRuiMaximum(f, -1, f.WindowContext.MovementBatch!.Id);
            ContinueQiaoRui(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        PublishQiaoRui(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void ChangeQiaoRuiMaximum(ProgramSkillFrame f, int amount, long lossBatch)
    {
        var before = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryQiaoRui = f.AnniversaryQiaoRui!with { Stage = AnniversaryQiaoRuiStage.HealthChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreProgramLifecycleOperations.ChangeProgramMaximumHp(f, amount);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryQiaoRuiMaximumHpEvent(f.Id, lossBatch, before, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp));
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveAnniversaryQiaoRuiAishouDraw)]
    internal void ReturnQiaoRuiDraw(ProgramSkillFrame supplied, NativeDrawOutcome d)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.AnniversaryQiaoRui!;
        if (!ValidQiaoRui(f) || r.Stage != AnniversaryQiaoRuiStage.Drawing || d.RecipientSeat != f.OwnerSeat || d.RequestedCount != r.Requested || d.SequenceBefore != r.Before || f.NativeDraw is not null)
            throw new InvalidOperationException("Aishou requires its exact once-paid original Draw return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryQiaoRui = r with { Stage = AnniversaryQiaoRuiStage.DrawChildren, Draw = d, After = d.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryQiaoRuiDrawReturnedEvent(f.Id, d.RequestedCount, d.ActualDrawIds.Count, d.SequenceBefore, d.SequenceAfter));
        foreach (var id in d.ActualDrawIds)
        {
            var m = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.CardId == id && m.Sequence > d.SequenceBefore && m.Sequence <= d.SequenceAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat)).ToArray();
            if (m is not [var entry])
                throw new InvalidOperationException("Aishou may mark only this paid draw's actual Hand entities.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryQiaoRuiMarkedEvent(f.Id, r.Source, r.Hash, id, entry.CardKind, entry.Sequence));
        }

        ContinueQiaoRui(f.Id);
    }

    internal void ContinueQiaoRui(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryQiaoRui: { } r } f || f.Id != id)
            return;
        if (!ValidQiaoRui(f))
            throw new InvalidOperationException("Qiao Rui lost his original candidate, exact payment or native return.");
        if (r.Stage == AnniversaryQiaoRuiStage.SlashChoice)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishQiaoRui(f);
            return;
        }

        if (r.Stage is not (AnniversaryQiaoRuiStage.DrawChildren or AnniversaryQiaoRuiStage.DiscardChildren or AnniversaryQiaoRuiStage.HealthChildren or AnniversaryQiaoRuiStage.NativeTail or AnniversaryQiaoRuiStage.ClaimChildren))
            return;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.AnniversaryQiaoRui!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (r.Stage == AnniversaryQiaoRuiStage.DiscardChildren)
        {
            var actual = _coreProgramEventHistoryOperations.ProgramEventHistory<NativeLogicalDiscardMaterialCompletedEvent>().Count(m => m.From == CardLocation.Hand(f.OwnerSeat) && r.Cards.Contains(m.CardId) && _coreProgramEventHistoryOperations.ProgramEventHistory<NativeLogicalDiscardInvocationRecordedEvent>().Any(i => i.InvocationId == m.InvocationId && i.ParentFrameId == f.Id && i.Reason.Value == QiaoRuiDiscard && i.SequenceBefore >= r.Before && i.SequenceAfter <= r.After));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryQiaoRuiDiscardedEvent(f.Id, actual, r.Before, r.After));
            if (actual > _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            {
                ChangeQiaoRuiMaximum(f, 1, 0);
                ContinueQiaoRui(id);
                return;
            }
        }

        if (r.Stage == AnniversaryQiaoRuiStage.NativeTail && r.Damaged && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && r.Plan is { } p && _cardZonesOperations._cardZones.GetLocation(p.CardId)is var from && from is { Zone: CardZoneKind.DiscardPile or CardZoneKind.Processing })
        {
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryQiaoRui = r with { Stage = AnniversaryQiaoRuiStage.ClaimChildren, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([p.CardId], CardLocation.Hand(f.OwnerSeat), new(QiaoRuiClaim), (_, _) =>
            {
                var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                _coreRuntimeOperations.ReplaceRuntimeTop(current with { AnniversaryQiaoRui = current.AnniversaryQiaoRui!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryQiaoRuiClaimedEvent(id, r.UseId, p.CardId, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
            });
            ContinueQiaoRui(id);
            return;
        }

        FinishQiaoRui(f);
    }

    internal void FinishQiaoRui(ProgramSkillFrame f)
    {
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryQiaoRui = f.AnniversaryQiaoRui!with { Stage = AnniversaryQiaoRuiStage.Completed }, PendingMovementContinuation = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryQiaoRuiCompletedEvent(f.Id));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 49400)]
    internal bool ResumeQiaoRui(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryQiaoRui: { Stage: not AnniversaryQiaoRuiStage.Completed } } f || f.Id != id)
            return false;
        ContinueQiaoRui(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 49400)]
    internal bool ReturnQiaoRuiMovement(ProgramSkillFrame f) => f.AnniversaryQiaoRui is not null && ResumeQiaoRui(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 49400)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 49400)]
    internal bool KeepQiaoRuiPaid(ProgramSkillFrame f) => f.AnniversaryQiaoRui is { Stage: not AnniversaryQiaoRuiStage.SlashChoice } && ValidQiaoRui(f);
}
