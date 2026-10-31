#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineBoundCardsRecastsRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.IBoundCardsRecastHost.RecastBoundCards(ProgramSkillFrame frame, string sourceBind) => BeginBoundCardsRecast(frame, sourceBind);
    internal SkillProgramStepOutcome BeginBoundCardsRecast(ProgramSkillFrame supplied, string sourceBind)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != frame.Id || frame.BoundCardsRecast is not null)
            throw new InvalidOperationException("A bound group recast requires its original unpaid instruction.");
        var source = _coreProgramLifecycleOperations.GetProgramCardSet(frame, sourceBind);
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreDeckProgramsOperations.DeckProgramSourceValid(frame) || source.CardIds.Count is < 1 or > 2 || source.SourceLocations.Count != source.CardIds.Count || source.CardIds.Distinct().Count() != source.CardIds.Count || source.SelectionActorSeat is { } selector && selector != frame.OwnerSeat || source.SourceLocations.Any(location => location.OwnerSeat != frame.OwnerSeat || location.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment)) || source.CardIds.Where((id, index) => _cardZonesOperations._cardZones.GetLocation(id) != source.SourceLocations[index]).Any())
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "重铸牌或技能来源已失效，未支付成本。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var materials = source.CardIds.Select((id, index) =>
        {
            var from = source.SourceLocations[index];
            var card = _cardZonesOperations._cardZones.CardsAt(from).Single(c => c.Id == id);
            return new NativeRecastMaterial(id, card.Kind, from, card.IsGeneralWeapon);
        }).ToArray();
        var receipt = new ProgramBoundCardsRecastReceipt(frame.InstructionIndex, sourceBind, new(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash, _coreActualTurnProgressPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, _coreActualTurnProgressPhaseTurnNumberTurnProgressionAccess._phase, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
        {
            Materials = materials
        };
        frame = _coreNativeRecastInvocationsOperations.BeginFrameNativeRecastInvocation(frame, frame.OwnerSeat, source.CardIds);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { BoundCardsRecast = receipt, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        // Record the complete original group before any equipment-removal hook can run.
        // Recast is not a discard cost and deliberately bypasses discard prohibitions.
        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
            _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(player.Seat, player.SkillGrants.Revision);
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(source.CardIds, CardLocation.DiscardPile, CardMoveReasons.RecastDiscard, (batch, records) =>
        {
            if (records.Count != materials.Length || !records.Zip(materials).All(pair => IsBoundCardsRecastPayment(pair.First, pair.Second)))
                throw new InvalidOperationException("A bound recast changed its original physical payment group.");
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { BoundCardsRecast = receipt with { CostBatchId = batch, SequenceAfter = records.Max(move => move.Sequence) } });
        });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool IsBoundCardsRecastPayment(CardMovementRecord move, NativeRecastMaterial material) => move.CardId == material.CardId && move.CardKind == material.PrintedKind && move.From == material.From && move.Reason == CardMoveReasons.RecastDiscard && (move.To == CardLocation.DiscardPile || move.To == CardLocation.OutsideGame && (material.IsGeneralWeapon || global::CardGame.Core.EngineOwners.CoreGameEngineGuanduEquipmentRuntime.IsGuanduThunderChariotDeparture(_coreAdvancedLifecycleOperations.GetAdvancedCard(material.CardId), material.From, move.To) || _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial(move)));
    internal bool ValidBoundCardsRecast(ProgramSkillFrame frame)
    {
        if (frame.BoundCardsRecast is not { } receipt || receipt.InstructionIndex != frame.InstructionIndex || receipt.Source != new CardConversionSource(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId) || receipt.GameplayHash != frame.GameplayHash || receipt.TurnNumber != _coreActualTurnProgressPhaseTurnNumberTurnProgressionAccess._turnNumber || receipt.TurnOwnerSeat != _coreActualTurnProgressPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || receipt.Phase != _coreActualTurnProgressPhaseTurnNumberTurnProgressionAccess._phase || receipt.PhaseInstanceId != _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId || !Enum.IsDefined(receipt.Stage) || receipt.Materials is not System.Collections.IList { IsReadOnly: true } || receipt.Materials.Count is < 1 or > 2 || receipt.Materials.Select(m => m.CardId).Distinct().Count() != receipt.Materials.Count || receipt.Materials.Any(m => m.From.OwnerSeat != frame.OwnerSeat || m.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment)) || receipt.CostBatchId <= 0 || receipt.SequenceBefore < 0 || receipt.SequenceAfter <= receipt.SequenceBefore || receipt.SequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || receipt.DrawCount < 0 || receipt.DrawCount > receipt.Materials.Count)
            return false;
        var source = _coreProgramLifecycleOperations.GetProgramCardSet(frame, receipt.SourceBind);
        if (!source.CardIds.SequenceEqual(receipt.Materials.Select(m => m.CardId)) || !source.SourceLocations.SequenceEqual(receipt.Materials.Select(m => m.From)))
            return false;
        var paid = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(move => move.Sequence > receipt.SequenceBefore && move.Sequence <= receipt.SequenceAfter).ToArray();
        if (paid.Length != receipt.Materials.Count || !paid.Zip(receipt.Materials).All(pair => IsBoundCardsRecastPayment(pair.First, pair.Second)))
            return false;
        return receipt.Stage == ProgramBoundCardsRecastStage.CostChildren ? receipt.DrawSequenceBefore == 0 && receipt.DrawSequenceAfter == 0 && receipt.DrawCount == 0 : receipt.DrawSequenceBefore >= receipt.SequenceAfter && receipt.DrawSequenceAfter >= receipt.DrawSequenceBefore && receipt.DrawSequenceAfter <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.BeforeSelectionState, 62020)]
    internal void AssertBoundCardsRecast(ProgramSkillFrame frame, ProgramExecutionPlan plan)
    {
        if (frame.BoundCardsRecast is not { } receipt)
            return;
        if (frame.InstructionIndex < 1 || frame.InstructionIndex > plan.Instructions.Count || plan.Instructions[frame.InstructionIndex - 1] is not { Op: SkillProgramEffectOp.RecastBoundCards } effect || effect.SourceBind != receipt.SourceBind || !ValidBoundCardsRecast(frame))
            throw new InvalidOperationException("A bound recast lost its immutable group, paid movement or owning instruction.");
    }

    internal bool DrainBoundCardsRecastChildren(ProgramSkillFrame frame) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(frame.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(frame.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frame.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frame.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(frame.Id);
    [ProgramContinuation(ProgramContinuationKind.Resume, 62020)]
    internal bool ResumeBoundCardsRecast(long frameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BoundCardsRecast: { } receipt } frame || frame.Id != frameId)
            return false;
        if (!ValidBoundCardsRecast(frame))
            throw new InvalidOperationException("A paid bound recast lost its original receipt.");
        if (receipt.Stage == ProgramBoundCardsRecastStage.Drawing)
            throw new InvalidOperationException("A bound recast resumed before its typed native draw return.");
        if (DrainBoundCardsRecastChildren(frame))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { PendingMovementContinuation = null });
        if (receipt.Stage == ProgramBoundCardsRecastStage.CostChildren)
        {
            receipt = receipt with
            {
                Stage = ProgramBoundCardsRecastStage.Drawing,
                DrawSequenceBefore = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
                DrawSequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundCardsRecast = receipt });
            if (_coreMatchOutcomeWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive)
                _coreProgramNativeDrawOperations.BeginOperationProgramDraw(frame, frame.OwnerSeat, receipt.Materials.Count, CardMoveReasons.RecastDraw);
            else
            {
                CompleteBoundCardsRecastDraw(frame, 0, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence);
                _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            }

            return true;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { BoundCardsRecast = null });
        if (_coreMatchOutcomeWinnerAccess._winner == Winner.None)
            return false;
        _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), "游戏已结束，重铸后续结算已取消。");
        return true;
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.RecastBoundCards)]
    internal void ReturnBoundCardsRecastDraw(ProgramSkillFrame frame, NativeDrawOutcome outcome)
    {
        if (frame.BoundCardsRecast is not { Stage: ProgramBoundCardsRecastStage.Drawing } receipt || frame.NativeDraw is not null || outcome.RecipientSeat != frame.OwnerSeat || outcome.RequestedCount != receipt.Materials.Count || outcome.ActualDrawIds.Count > receipt.Materials.Count)
            throw new InvalidOperationException("A bound recast draw lost its original paid group.");
        _coreProgramLifecycleOperations.CompleteProgramDrawCards(frame.Id, frame.OwnerSeat, null, SkillProgramCardSetVisibility.Private, CardMoveReasons.RecastDraw, 0, outcome.ActualDrawIds);
        CompleteBoundCardsRecastDraw(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), outcome.ActualDrawIds.Count, outcome.SequenceBefore, outcome.SequenceAfter);
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    internal void CompleteBoundCardsRecastDraw(ProgramSkillFrame frame, int actual, long before, long after)
    {
        var receipt = frame.BoundCardsRecast!;
        if (receipt.Stage != ProgramBoundCardsRecastStage.Drawing || actual < 0 || actual > receipt.Materials.Count)
            throw new InvalidOperationException("A bound recast cannot draw for its paid group twice.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { BoundCardsRecast = receipt with { Stage = ProgramBoundCardsRecastStage.DrawChildren, DrawCount = actual, DrawSequenceBefore = before, DrawSequenceAfter = after }, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        for (var index = 0; index < receipt.Materials.Count; index++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRecastEvent(frame.OwnerSeat, receipt.Materials[index].CardId, receipt.Materials[index].PrintedKind, index < actual ? 1 : 0));
        _coreNativeRecastInvocationsOperations.CompleteFrameNativeRecastInvocation(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), actual);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 62020)]
    internal bool ReturnBoundCardsRecastMovement(ProgramSkillFrame frame)
    {
        if (frame.BoundCardsRecast is null)
            return false;
        if (frame.PendingMovementContinuation is null || !ValidBoundCardsRecast(frame))
            throw new InvalidOperationException("A recast child lost its exact paid movement return.");
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return true;
    }

    [ProgramMovementProof(ProgramMovementProofPhase.PrivatePayments, 62020)]
    internal bool IsBoundCardsRecastMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation pending) => effect?.Op == SkillProgramEffectOp.RecastBoundCards && frame.BoundCardsRecast is { Stage: ProgramBoundCardsRecastStage.CostChildren or ProgramBoundCardsRecastStage.DrawChildren } && pending.SubjectSeat == frame.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null && ValidBoundCardsRecast(frame);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 62020)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 62020)]
    internal bool CanKeepPaidBoundCardsRecast(ProgramSkillFrame frame) => frame.BoundCardsRecast is not null && ValidBoundCardsRecast(frame);
}
