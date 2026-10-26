namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class BoundCardsRecastCapability(GameEngine engine) : IBoundCardsRecastHost
    {
        public SkillProgramStepOutcome RecastBoundCards(ProgramSkillFrame frame, string sourceBind) =>
            engine.BeginBoundCardsRecast(frame, sourceBind);
    }

    private SkillProgramStepOutcome BeginBoundCardsRecast(ProgramSkillFrame supplied, string sourceBind)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (_resolutionStack.LastOrDefault()?.Id != frame.Id || frame.BoundCardsRecast is not null)
            throw new InvalidOperationException("A bound group recast requires its original unpaid instruction.");
        var source = GetProgramCardSet(frame, sourceBind);
        if (_winner != Winner.None || !DeckProgramSourceValid(frame) || source.CardIds.Count is < 1 or > 2 ||
            source.SourceLocations.Count != source.CardIds.Count || source.CardIds.Distinct().Count() != source.CardIds.Count ||
            source.SelectionActorSeat is { } selector && selector != frame.OwnerSeat || source.SourceLocations.Any(location =>
                location.OwnerSeat != frame.OwnerSeat || location.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment)) ||
            source.CardIds.Where((id, index) => _cardZones.GetLocation(id) != source.SourceLocations[index]).Any())
        {
            CancelProgramBindingAndCleanup(frame, "重铸牌或技能来源已失效，未支付成本。");
            return SkillProgramStepOutcome.AwaitChild;
        }
        var materials = source.CardIds.Select((id, index) =>
        {
            var from = source.SourceLocations[index];
            var card = _cardZones.CardsAt(from).Single(c => c.Id == id);
            return new NativeRecastMaterial(id, card.Kind, from, card.IsGeneralWeapon);
        }).ToArray();
        var receipt = new ProgramBoundCardsRecastReceipt(frame.InstructionIndex, sourceBind,
            new(frame.SkillId, GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash,
            _turnNumber, _turnProgression.OwnerSeat, _phase, _cardUseDebitPhaseInstanceId, _movementSequence)
        { Materials = materials };
        frame = BeginFrameNativeRecastInvocation(frame, frame.OwnerSeat, source.CardIds);
        ReplaceRuntimeTop(frame with { BoundCardsRecast = receipt,
            PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        // Record the complete original group before any equipment-removal hook can run.
        // Recast is not a discard cost and deliberately bypasses discard prohibitions.
        foreach (var player in _players) _observedSkillGrantRevisions.TryAdd(player.Seat, player.SkillGrants.Revision);
        MoveProgramCardsFromMultipleSources(source.CardIds, CardLocation.DiscardPile, CardMoveReasons.RecastDiscard,
            (batch, records) =>
            {
                if (records.Count != materials.Length || !records.Zip(materials).All(pair =>
                        IsBoundCardsRecastPayment(pair.First, pair.Second)))
                    throw new InvalidOperationException("A bound recast changed its original physical payment group.");
                var active = GetActiveProgramFrame(frame.Id);
                ReplaceRuntimeTop(active with { BoundCardsRecast = receipt with
                    { CostBatchId = batch, SequenceAfter = records.Max(move => move.Sequence) } });
            });
        AdvanceRuntimeProgram(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    private bool IsBoundCardsRecastPayment(CardMovementRecord move, NativeRecastMaterial material) =>
        move.CardId == material.CardId && move.CardKind == material.PrintedKind && move.From == material.From &&
        move.Reason == CardMoveReasons.RecastDiscard && (move.To == CardLocation.DiscardPile ||
            move.To == CardLocation.OutsideGame && (material.IsGeneralWeapon ||
                IsGuanduThunderChariotDeparture(GetAdvancedCard(material.CardId), material.From, move.To) ||
                IsEphemeralCardDestroyedMaterial(move)));

    private bool ValidBoundCardsRecast(ProgramSkillFrame frame)
    {
        if (frame.BoundCardsRecast is not { } receipt || receipt.InstructionIndex != frame.InstructionIndex ||
            receipt.Source != new CardConversionSource(frame.SkillId, GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId) ||
            receipt.GameplayHash != frame.GameplayHash || receipt.TurnNumber != _turnNumber ||
            receipt.TurnOwnerSeat != _turnProgression.OwnerSeat || receipt.Phase != _phase ||
            receipt.PhaseInstanceId != _cardUseDebitPhaseInstanceId || !Enum.IsDefined(receipt.Stage) ||
            receipt.Materials is not System.Collections.IList { IsReadOnly: true } ||
            receipt.Materials.Count is < 1 or > 2 || receipt.Materials.Select(m => m.CardId).Distinct().Count() != receipt.Materials.Count ||
            receipt.Materials.Any(m => m.From.OwnerSeat != frame.OwnerSeat ||
                m.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment)) ||
            receipt.CostBatchId <= 0 || receipt.SequenceBefore < 0 || receipt.SequenceAfter <= receipt.SequenceBefore ||
            receipt.SequenceAfter > _movementSequence || receipt.DrawCount < 0 || receipt.DrawCount > receipt.Materials.Count)
            return false;
        var source = GetProgramCardSet(frame, receipt.SourceBind);
        if (!source.CardIds.SequenceEqual(receipt.Materials.Select(m => m.CardId)) ||
            !source.SourceLocations.SequenceEqual(receipt.Materials.Select(m => m.From))) return false;
        var paid = _cardMovements.Where(move => move.Sequence > receipt.SequenceBefore && move.Sequence <= receipt.SequenceAfter).ToArray();
        if (paid.Length != receipt.Materials.Count || !paid.Zip(receipt.Materials).All(pair =>
                IsBoundCardsRecastPayment(pair.First, pair.Second))) return false;
        return receipt.Stage == ProgramBoundCardsRecastStage.CostChildren
            ? receipt.DrawSequenceBefore == 0 && receipt.DrawSequenceAfter == 0 && receipt.DrawCount == 0
            : receipt.DrawSequenceBefore >= receipt.SequenceAfter && receipt.DrawSequenceAfter >= receipt.DrawSequenceBefore &&
                receipt.DrawSequenceAfter <= _movementSequence;
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.BeforeSelectionState, 62020)]
    private void AssertBoundCardsRecast(ProgramSkillFrame frame, ProgramExecutionPlan plan)
    {
        if (frame.BoundCardsRecast is not { } receipt) return;
        if (frame.InstructionIndex < 1 || frame.InstructionIndex > plan.Instructions.Count ||
            plan.Instructions[frame.InstructionIndex - 1] is not { Op: SkillProgramEffectOp.RecastBoundCards } effect ||
            effect.SourceBind != receipt.SourceBind || !ValidBoundCardsRecast(frame))
            throw new InvalidOperationException("A bound recast lost its immutable group, paid movement or owning instruction.");
    }

    private bool DrainBoundCardsRecastChildren(ProgramSkillFrame frame) =>
        TryBeginQueuedRecoveryReplacement(frame.Id, PostEventContinuation.AwaitedProgramMovement) ||
        TryBeginCharacterStateProgramWindow(frame.Id, CharacterStateContinuation.Program) ||
        TryBeginHpChangedProgramWindow(frame.Id, PostEventContinuation.AwaitedProgramMovement) ||
        TryBeginCardsMovedProgramWindow(frame.Id) || TryBeginAdvancedSkillsChanged(frame.Id);

    [ProgramContinuation(ProgramContinuationKind.Resume, 62020)]
    private bool ResumeBoundCardsRecast(long frameId)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { BoundCardsRecast: { } receipt } frame || frame.Id != frameId)
            return false;
        if (!ValidBoundCardsRecast(frame)) throw new InvalidOperationException("A paid bound recast lost its original receipt.");
        if (receipt.Stage == ProgramBoundCardsRecastStage.Drawing)
            throw new InvalidOperationException("A bound recast resumed before its typed native draw return.");
        if (DrainBoundCardsRecastChildren(frame)) return true;
        ReplaceRuntimeTop(frame = GetActiveProgramFrame(frame.Id) with { PendingMovementContinuation = null });
        if (receipt.Stage == ProgramBoundCardsRecastStage.CostChildren)
        {
            receipt = receipt with { Stage = ProgramBoundCardsRecastStage.Drawing,
                DrawSequenceBefore = _movementSequence, DrawSequenceAfter = _movementSequence };
            ReplaceRuntimeTop(frame = frame with { BoundCardsRecast = receipt });
            if (_winner == Winner.None && _players[frame.OwnerSeat].IsAlive)
                BeginOperationProgramDraw(frame, frame.OwnerSeat, receipt.Materials.Count, CardMoveReasons.RecastDraw);
            else
            {
                CompleteBoundCardsRecastDraw(frame, 0, _movementSequence, _movementSequence);
                AdvanceRuntimeProgram(frame.Id);
            }
            return true;
        }
        ReplaceRuntimeTop(frame with { BoundCardsRecast = null });
        if (_winner == Winner.None) return false;
        CancelProgramBindingAndCleanup(GetActiveProgramFrame(frame.Id), "游戏已结束，重铸后续结算已取消。");
        return true;
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.RecastBoundCards)]
    private void ReturnBoundCardsRecastDraw(ProgramSkillFrame frame, NativeDrawOutcome outcome)
    {
        if (frame.BoundCardsRecast is not { Stage: ProgramBoundCardsRecastStage.Drawing } receipt ||
            frame.NativeDraw is not null || outcome.RecipientSeat != frame.OwnerSeat ||
            outcome.RequestedCount != receipt.Materials.Count || outcome.ActualDrawIds.Count > receipt.Materials.Count)
            throw new InvalidOperationException("A bound recast draw lost its original paid group.");
        CompleteProgramDrawCards(frame.Id, frame.OwnerSeat, null, SkillProgramCardSetVisibility.Private,
            CardMoveReasons.RecastDraw, 0, outcome.ActualDrawIds);
        CompleteBoundCardsRecastDraw(GetActiveProgramFrame(frame.Id), outcome.ActualDrawIds.Count,
            outcome.SequenceBefore, outcome.SequenceAfter);
        AdvanceRuntimeProgram(frame.Id);
    }

    private void CompleteBoundCardsRecastDraw(ProgramSkillFrame frame, int actual, long before, long after)
    {
        var receipt = frame.BoundCardsRecast!;
        if (receipt.Stage != ProgramBoundCardsRecastStage.Drawing || actual < 0 || actual > receipt.Materials.Count)
            throw new InvalidOperationException("A bound recast cannot draw for its paid group twice.");
        ReplaceRuntimeTop(frame with { BoundCardsRecast = receipt with { Stage = ProgramBoundCardsRecastStage.DrawChildren,
            DrawCount = actual, DrawSequenceBefore = before, DrawSequenceAfter = after },
            PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        for (var index = 0; index < receipt.Materials.Count; index++)
            AdvanceEventRulesAndQueueFact(new CardRecastEvent(frame.OwnerSeat, receipt.Materials[index].CardId,
                receipt.Materials[index].PrintedKind, index < actual ? 1 : 0));
        CompleteFrameNativeRecastInvocation(GetActiveProgramFrame(frame.Id), actual);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 62020)]
    private bool ReturnBoundCardsRecastMovement(ProgramSkillFrame frame)
    {
        if (frame.BoundCardsRecast is null) return false;
        if (frame.PendingMovementContinuation is null || !ValidBoundCardsRecast(frame))
            throw new InvalidOperationException("A recast child lost its exact paid movement return.");
        AdvanceRuntimeProgram(frame.Id);
        return true;
    }

    [ProgramMovementProof(ProgramMovementProofPhase.PrivatePayments, 62020)]
    private bool IsBoundCardsRecastMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation pending) =>
        effect?.Op == SkillProgramEffectOp.RecastBoundCards && frame.BoundCardsRecast is
            { Stage: ProgramBoundCardsRecastStage.CostChildren or ProgramBoundCardsRecastStage.DrawChildren } &&
        pending.SubjectSeat == frame.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null &&
        ValidBoundCardsRecast(frame);

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 62020)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 62020)]
    private bool CanKeepPaidBoundCardsRecast(ProgramSkillFrame frame) =>
        frame.BoundCardsRecast is not null && ValidBoundCardsRecast(frame);
}
