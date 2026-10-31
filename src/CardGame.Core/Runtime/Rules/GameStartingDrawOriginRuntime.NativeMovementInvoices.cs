namespace CardGame.Core;

internal sealed partial class GameStartingDrawOriginRuntime
{

    internal static bool SameGameStartingNativeMovementOrigin(GameStartingNativeMovementOrigin? left, GameStartingNativeMovementOrigin? right) =>
        global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_GameStartingNativeMovementInvoices.SameGameStartingNativeMovementOrigin(left,right);

    internal static bool SameGameStartingMovementDrawOrigin(GameStartingMovementDrawOrigin? left, GameStartingMovementDrawOrigin? right) =>
        global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_GameStartingNativeMovementInvoices.SameGameStartingMovementDrawOrigin(left,right);

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 26705)]
    internal void CaptureGameStartingNativeGift(DingShangWanMovementIssuedEvent issued)
    {
        if (!facts.TracksNativeDrawInvocations || issued.Stage != DingShangWanStage.OpeningGiftChildren || issued.Count == 0 ||
            frames.Top is not ProgramSkillFrame { DingShangWan: { } receipt } program || program.Id != issued.FrameId ||
            !TryGetGameStartingDrawProgram(program, out var window) || receipt.Operation != SkillProgramEffectOp.InitializeFudaoHandRecord ||
            receipt.Stage != issued.Stage || receipt.Instruction != program.InstructionIndex || receipt.Turn != 0 ||
            receipt.Source != new CardConversionSource(program.SkillId, RuntimeSkills.RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(program), program.OwnerSeat, program.SkillInstanceId) || receipt.GameplayHash != program.GameplayHash || receipt.WindowId != window!.Id ||
            receipt.Before != issued.Before || receipt.After != issued.After || receipt.ActualCount != issued.Count ||
            receipt.Payer != program.OwnerSeat || receipt.Payer != issued.Payer || receipt.Recipient != issued.Recipient ||
            receipt.Recipient is not { } recipient || recipient == program.OwnerSeat ||
            program.PendingMovementContinuation != new ProgramMovementContinuation(program.OwnerSeat, 0, null)) return;
        var matchingBatches = Enumerable.Range(0, batches.PendingBatchCount).Select(index => (batch: batches.PendingBatchAt(index), index)).Where(entry =>
            entry.batch.ParentFrameId == program.Id && entry.batch.AwaitingProgramFrameId == program.Id && entry.batch.TurnNumber == 0 &&
            entry.batch.OriginSkillId == program.SkillId && entry.batch.OriginSkillInstanceId == program.SkillInstanceId &&
            entry.batch.OriginOwnerSeat == program.OwnerSeat && entry.batch.NativeDrawInvocation is null &&
            entry.batch.Movements.Count == receipt.Selected.Count && entry.batch.Movements.Count == issued.Count &&
            entry.batch.Movements.All(move => move.Sequence > issued.Before && move.Sequence <= issued.After &&
                move.From == CardLocation.Hand(program.OwnerSeat) && move.To == CardLocation.Hand(recipient) &&
                move.Reason.Value == FudaoGiftReason && receipt.Selected.Contains(move.CardId) && movements.Read().Contains(move))).ToArray();
        if (matchingBatches is not [var selected] || selected.batch.GameStartingMovementOrigin is not null)
            throw new InvalidOperationException("A native startup gift lost its exact once-issued movement batch.");
        var origin = new GameStartingNativeMovementOrigin(window.Id, program.Id, program.InstructionIndex, program.GameplayHash,
            receipt.Source, receipt.Operation, selected.batch.Id, recipient, issued.Before, issued.After)
            { ActualTurnOwnerSeat = receipt.TurnOwner, Materials = selected.batch.Movements };
        batchOrigins.AssignGameStartingMovementOrigin(selected.index, origin);
        events.AdvanceFact(new GameStartingNativeMovementRecordedEvent(origin));
    }

    internal bool ValidGameStartingNativeMovement(GameStartingNativeMovementOrigin root)
    {
        if (root.Kind != GameStartingNativeMovementKind.OpeningHandGift || root.Operation != SkillProgramEffectOp.InitializeFudaoHandRecord ||
            root.WindowFrameId <= 0 || root.ProgramFrameId <= root.WindowFrameId || root.BatchId <= root.ProgramFrameId || root.InstructionIndex != 1 ||
            !participants.IsValidSeat(root.Source.OwnerSeat) || !participants.IsValidSeat(root.RecipientSeat) || root.RecipientSeat == root.Source.OwnerSeat ||
            !participants.IsValidSeat(root.ActualTurnOwnerSeat) || root.SequenceBefore < 0 || root.SequenceAfter <= root.SequenceBefore ||
            root.Materials.Count is < 1 or > 3 || root.Materials.Select(move => move.CardId).Distinct().Count() != root.Materials.Count ||
            root.Materials.Any(move => move.TurnNumber != 0 || move.Sequence <= root.SequenceBefore || move.Sequence > root.SequenceAfter ||
                move.From != CardLocation.Hand(root.Source.OwnerSeat) || move.To != CardLocation.Hand(root.RecipientSeat) ||
                move.Reason.Value != FudaoGiftReason) ||
            !root.Materials.SequenceEqual(movements.Read().Where(move => move.Sequence > root.SequenceBefore && move.Sequence <= root.SequenceAfter)) ||
            content.FindSkill(root.Source.SkillId)?.Program is not { } definition || definition.GameplayHash != root.GameplayHash)
            return false;
        var trigger = definition.Triggers.SingleOrDefault(item => item.Id == root.Source.BindingId);
        if (trigger?.Window != SkillProgramTriggerWindow.GameStarting ||
            ProgramInstructionResolver.Default.Resolve(definition, ProgramInstructionSourceKind.Trigger, trigger.Id).Instructions is not
                [{ Op: SkillProgramEffectOp.InitializeFudaoHandRecord }]) return false;
        var history = journal.Complete().ToArray();
        return history.OfType<GameStartingNativeMovementRecordedEvent>().Count(fact =>
                SameGameStartingNativeMovementOrigin(fact.Origin, root)) == 1 &&
            history.OfType<ProgramBindingStartedEvent>().Count(started => started.FrameId == root.ProgramFrameId &&
                started.SkillId == root.Source.SkillId && started.BindingId == root.Source.BindingId && started.SkillInstanceId == root.Source.SkillInstanceId &&
                started.OwnerSeat == root.Source.OwnerSeat && started.Window == SkillProgramTriggerWindow.GameStarting) == 1 &&
            history.OfType<DingShangWanStartedEvent>().Where(started => started.FrameId == root.ProgramFrameId).ToArray() is [var start] &&
                start.Source == root.Source && start.GameplayHash == root.GameplayHash && start.WindowId == root.WindowFrameId &&
                start.Window == SkillProgramTriggerWindow.GameStarting && start.Operation == root.Operation && start.Turn == 0 &&
                start.TurnOwner == root.ActualTurnOwnerSeat && start.Counterpart is null && start.DamageFrameId is null && start.ActionId is null &&
            history.OfType<DingShangWanMovementIssuedEvent>().Where(payment => payment.FrameId == root.ProgramFrameId &&
                payment.Stage == DingShangWanStage.OpeningGiftChildren).ToArray() is [var payment] &&
                payment == new DingShangWanMovementIssuedEvent(root.ProgramFrameId, DingShangWanStage.OpeningGiftChildren,
                    root.Source.OwnerSeat, root.RecipientSeat, root.SequenceBefore, root.SequenceAfter, root.Materials.Count);
    }

    internal GameStartingMovementDrawOrigin? CaptureGameStartingNativeMovementDrawOrigin(int recipient, int requested,
        CardMoveReason reason, CardConversionSource? directProducer)
    {
        if (turn.TurnNumber != 0 || phase.Phase != TurnPhase.NotStarted || directProducer is not null || requested != 1 || reason.Value != "program.bu-zhi.draw" ||
            frames.Top is not ProgramSkillFrame { BuZhi: { Stage: "movement", ReturnStage: "complete", Actual: 0, PaidIds.Count: 0 } benefit } program ||
            program.WindowContext is not { Window: SkillProgramTriggerWindow.CardsMoved, MovementBatch: { NativeDrawInvocation: null,
                GameStartingMovementOrigin: { } root } batch } context ||
            context.OwnerSeat != program.OwnerSeat || root.RecipientSeat != program.OwnerSeat || root.BatchId != batch.Id ||
            root.ProgramFrameId != batch.ParentFrameId || !root.Materials.SequenceEqual(batch.Movements) || !ValidGameStartingNativeMovement(root) ||
            benefit.Source != new CardConversionSource(program.SkillId, RuntimeSkills.RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(program), program.OwnerSeat, program.SkillInstanceId) ||
            benefit.GameplayHash != program.GameplayHash || benefit.InstructionIndex != program.InstructionIndex ||
            benefit.ParentFrameId != context.ParentFrameId || benefit.BatchId != batch.Id || benefit.Target != recipient ||
            benefit.Before != MovementSequence || benefit.After != benefit.Before ||
            program.PendingMovementContinuation != new ProgramMovementContinuation(program.OwnerSeat, 0, null)) return null;
        var index = FrameIndex(program.Id);
        if (index < 1 || nativeFrames.FrameAt(index - 1) is not CardsMovedTriggerWindowFrame parent || parent.Id != context.ParentFrameId ||
            parent.Batch.Id != batch.Id || !parent.Batch.Movements.SequenceEqual(batch.Movements) ||
            !SameGameStartingNativeMovementOrigin(parent.Batch.GameStartingMovementOrigin, root) ||
            parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count ||
            !RuntimeSkills.RuntimeRules_Core_DamageTargetMountObserver.MountObserverCandidateMatches(program, parent.Candidates[parent.CandidateIndex])) return null;
        var child = new GameStartingOutsideDrawOrigin(program.Id, program.InstructionIndex, program.GameplayHash, benefit.Source,
            parent.Id, batch.Id, 0, recipient, benefit.Before) { Kind = GameStartingDrawChildKind.NativeGainBenefit };
        return GameStartingDrawChildProgramMatches(child, movementRoot: true) && GameStartingDrawChildStartedMatches(child)
            ? new(root, child) : null;
    }

    internal bool ValidGameStartingDrawProvenance(NativeDrawInvocationProof proof) => proof.GameStartingOrigin is not null
        ? proof.GameStartingMovementOrigin is null && ValidGameStartingDrawOrigin(proof)
        : proof.GameStartingMovementOrigin is not null && ValidGameStartingMovementDrawChain(proof);

    internal bool ValidGameStartingMovementDrawChain(NativeDrawInvocationProof proof)
    {
        if (proof.ActualTurnNumber != 0 || proof.Phase is not null || proof.GameStartingMovementOrigin is not { } origin ||
            !ValidGameStartingNativeMovement(origin.MovementRoot)) return false;
        var root = origin.MovementRoot;
        var history = journal.Complete().ToArray();
        var records = history.OfType<NativeDrawInvocationRecordedEvent>().GroupBy(record => record.InvocationId)
            .ToDictionary(group => group.Key, group => group.ToArray());
        var id = proof.InvocationId;
        while (records.TryGetValue(id, out var matching) && matching is [var record])
        {
            if (record.GameStartingOrigin is not null || record.GameStartingMovementOrigin is not { } issued ||
                !SameGameStartingNativeMovementOrigin(issued.MovementRoot, root) || record.TurnNumber != 0 || record.Phase is not null ||
                record.ActualTurnOwnerSeat != root.ActualTurnOwnerSeat || record.Requested < record.ActualCount || record.ActualCount < 1 ||
                !participants.IsValidSeat(record.OwnerSeat) || record.Reason == CardMoveReasons.InitialDeal || record.LastBatchId <= 0 ||
                record.SequenceBefore < root.SequenceAfter || record.SequenceAfter <= record.SequenceBefore || record.SequenceAfter > MovementSequence)
                return false;
            var materials = movements.Read().Where(move => move.Sequence > record.SequenceBefore && move.Sequence <= record.SequenceAfter &&
                move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(record.OwnerSeat) && move.Reason == record.Reason).ToArray();
            var child = issued.DrawChild;
            if (materials.Length != record.ActualCount || materials.Select(move => move.CardId).Distinct().Count() != materials.Length ||
                record.ParentFrameId != child.ProgramFrameId || child.ProgramFrameId >= id || child.ParentInvocationId >= id ||
                record.OwnerSeat != child.ParticipantSeat || record.Requested != child.RequestedCount || record.SequenceBefore != child.SequenceBefore ||
                !GameStartingDrawChildProgramMatches(child, movementRoot: child.ParentInvocationId == 0) || !GameStartingDrawChildStartedMatches(child) ||
                history.OfType<GameStartingDrawChildRequestedEvent>().Count(request => request.InvocationId == id && request.Child == child) != 1 ||
                !GameStartingDrawChildPaymentMatches(record, child, history)) return false;
            if (child.ParentInvocationId == 0)
                return child.Kind == GameStartingDrawChildKind.NativeGainBenefit && child.Source.OwnerSeat == root.RecipientSeat &&
                    child.OriginalBatchId == root.BatchId && child.WindowFrameId == root.BatchId;
            if (!records.TryGetValue(child.ParentInvocationId, out var parents) || parents is not [var parent] ||
                parent.LastBatchId != child.OriginalBatchId || parent.OwnerSeat != child.Source.OwnerSeat ||
                parent.SequenceAfter > child.SequenceBefore || parent.PhaseActorSeat != record.PhaseActorSeat) return false;
            id = child.ParentInvocationId;
        }
        return false;
    }
}
