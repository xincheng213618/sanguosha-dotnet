namespace CardGame.Core;

// The draft stays on the original instruction; this service joins its physical
// journal and facts without retaining a second pending-discard store.
[ProgramRuntimeModule,ProgramCapability]
internal sealed class NativeLogicalDiscardRuntime(IProgramRuntimeFrames frames, IProgramRuntimeEvents events,
    ICardZoneQueries zones, ICardMovementHistoryQueries movements, IProgramEventHistoryQueries history,
    IProgramContentQueries content, IProgramTurnQueries turn, ISelectionParticipantQueries participants,
    IActualDiscardRecoveryPhaseQueries actualPhases, INativeLogicalDiscardFacts facts,
    INativeLogicalDiscardOrigins origins, INativeLogicalDiscardAdmissions admissions,
    INativeLogicalDiscardMovement movement, IProgramBoundCardMovementOperations programMovement) : INativeLogicalDiscardOperations
{
    public bool TracksInvocations =>
        content.RequiresFact(ProgramTrackedFact.NativeLogicalDiscard);

    public NativeLogicalDiscardInvocationDraft? Capture(int initiatorSeat,
        int executorSeat, IReadOnlyList<int> cardIds, CardMoveReason reason, long? parentFrameId = null)
    {
        if (!TracksInvocations || !facts.SetupComplete || turn.TurnNumber < 1 ||
            !participants.IsValidSeat(turn.TurnOwnerSeat) || !participants.IsValidSeat(initiatorSeat) ||
            !participants.IsValidSeat(executorSeat) || cardIds.Count == 0) return null;
        if (cardIds.Distinct().Count() != cardIds.Count)
            throw new InvalidOperationException("A logical discard cannot repeat a primary material.");
        var materials = cardIds.Select(id =>
        {
            var from = zones.GetLocation(id);
            if (from == CardLocation.Processing)
            {
                var entry = movements.Last(m => m.CardId == id);
                if (entry is not null && entry.To == CardLocation.Processing &&
                    origins.IsOwnedDiscardSource(entry.From) && origins.IsProgramDiscardOriginReason(entry.Reason)) from = entry.From;
            }
            if (!origins.IsOwnedDiscardSource(from))
                throw new InvalidOperationException("A logical discard needs its original owned primary material.");
            return new NativeLogicalDiscardMaterial(id, from);
        }).ToArray();
        return admissions.Admit(new NativeLogicalDiscardInvocationDraft
        {
            InvocationId = frames.NextId(), TurnNumber = turn.TurnNumber,
            ActualTurnOwnerSeat = turn.TurnOwnerSeat, InitiatorSeat = initiatorSeat,
            ExecutorSeat = executorSeat, SequenceBefore = facts.MovementSequence,
            ParentFrameId = parentFrameId ?? frames.Top?.Id, Reason = reason, Materials = materials,
            Phase = actualPhases.Current()
        });
    }

    public void Complete(NativeLogicalDiscardInvocationDraft? draft,
        IReadOnlyList<CardMovementRecord>? prepaidMaterials = null)
    {
        if (draft is null) return;
        if (history.Read<NativeLogicalDiscardInvocationRecordedEvent>().Any(e => e.InvocationId == draft.InvocationId))
            throw new InvalidOperationException("A logical discard must finish exactly once.");
        // Some native conversion costs destroy ephemeral equipment before the
        // skill's discard instruction runs. Only an explicitly supplied, signed
        // discard-origin proof can credit that already completed movement.
        if (prepaidMaterials is not null && (prepaidMaterials.Select(m => m.CardId).Distinct().Count() != prepaidMaterials.Count ||
            prepaidMaterials.Any(move => !movements.Read().Contains(move) || move.Sequence <= draft.SequenceBefore ||
                move.Sequence > facts.MovementSequence || !origins.IsEphemeralCardDestroyedMaterial(move) ||
                !draft.Materials.Any(material => material.CardId == move.CardId && material.From == move.From &&
                    origins.GetProgramDiscardSource(move) == material.From))))
            throw new InvalidOperationException("A prepaid logical discard needs its exact completed native material proof.");
        admissions.Close(draft);
        var actual = draft.Materials.Select(material =>
        {
            var move = prepaidMaterials?.SingleOrDefault(move => move.CardId == material.CardId) ?? movements.Read().FirstOrDefault(move =>
                move.CardId == material.CardId && move.Sequence > draft.SequenceBefore && move.Sequence <= facts.MovementSequence &&
                (move.From == material.From && move.To == CardLocation.DiscardPile && move.Reason == draft.Reason ||
                    origins.GetProgramDiscardSource(move) == material.From &&
                    (move.Reason == draft.Reason || move.Reason.Value == draft.Reason.Value + "-finished" ||
                        move.Reason.Value == draft.Reason.Value + "-discard") ||
                    move.From == material.From && move.To == CardLocation.OutsideGame &&
                    material.From.Zone == CardZoneKind.Equipment && move.Reason == draft.Reason));
            return move is null ? null : new NativeLogicalDiscardMaterialCompletedEvent(draft.InvocationId,
                material.CardId, material.From, move.To, move.Sequence);
        }).OfType<NativeLogicalDiscardMaterialCompletedEvent>().ToArray();
        foreach (var material in actual) events.AdvanceFact(material);
        if (actual.Length == 0) return;
        var owners = actual.Select(m => m.From.OwnerSeat).Distinct().ToArray();
        events.AdvanceFact(new NativeLogicalDiscardInvocationRecordedEvent(draft.InvocationId,
            draft.TurnNumber, draft.ActualTurnOwnerSeat, draft.InitiatorSeat, draft.ExecutorSeat,
            owners.Length == 1 ? owners[0] : null, actual.Length,
            actual.Count(m => m.From.Zone == CardZoneKind.Hand), draft.SequenceBefore, facts.MovementSequence,
            draft.ParentFrameId, draft.Reason) { Phase = draft.Phase });
    }

    public void MoveProgram(ProgramSkillFrame frame, int executorSeat, IReadOnlyList<int> ids,
        CardMoveReason reason, Action<long, IReadOnlyList<CardMovementRecord>>? afterPhysicalRecords = null)
    {
        var draft = Capture(frame.OwnerSeat, executorSeat, ids, reason, frame.Id);
        programMovement.MoveMultipleSources(ids, CardLocation.DiscardPile, reason,
            CardMovementDisclosure.FaceUp, afterPhysicalRecords);
        Complete(draft);
    }

    public void Move(int initiatorSeat, int executorSeat, Card card, CardLocation from,
        CardMoveReason reason, Action<CardMovementRecord>? beforeFact = null, long? parentFrameId = null)
    {
        var draft = Capture(initiatorSeat, executorSeat, [card.Id], reason, parentFrameId);
        movement.Move(card, from, CardLocation.DiscardPile, reason, beforeFact, disclosure: CardMovementDisclosure.FaceUp);
        Complete(draft);
    }

    public void MoveCards(int initiatorSeat, int executorSeat, IReadOnlyList<Card> cards,
        CardLocation from, CardMoveReason reason)
    {
        var draft = Capture(initiatorSeat, executorSeat,
            cards.Select(card => card.Id).ToArray(), reason);
        movement.MoveCards(cards, from, CardLocation.DiscardPile, reason, disclosure: CardMovementDisclosure.FaceUp);
        Complete(draft);
    }

    public ProgramSkillFrame AppendFrame(ProgramSkillFrame frame, int executorSeat,
        IReadOnlyList<int> ids, CardMoveReason reason)
    {
        frame = frames.ActiveProgram(frame.Id);
        if (!TracksInvocations) return frame;
        var draft = frame.LogicalDiscardInvocation;
        if (draft is null)
        {
            draft = Capture(frame.OwnerSeat, executorSeat, ids, reason, frame.Id);
            if (draft is not null) draft = draft with { InstructionIndex = frame.InstructionIndex };
        }
        else
        {
            if (draft.InitiatorSeat != frame.OwnerSeat || draft.ExecutorSeat != executorSeat || draft.Reason != reason ||
                draft.TurnNumber != turn.TurnNumber || draft.ActualTurnOwnerSeat != turn.TurnOwnerSeat ||
                draft.ParentFrameId != frame.Id || draft.InstructionIndex != frame.InstructionIndex || ids.Distinct().Count() != ids.Count ||
                ids.Any(id => draft.Materials.Any(m => m.CardId == id) || !origins.IsOwnedDiscardSource(zones.GetLocation(id))))
                throw new InvalidOperationException("A suspended logical discard changed its instruction or original primary materials.");
            draft = draft with { Materials = draft.Materials.Concat(ids.Select(id =>
                new NativeLogicalDiscardMaterial(id, zones.GetLocation(id)))).ToArray() };
        }
        if (draft is not null)
        {
            admissions.AdmitMaterials(draft);
            frames.ReplaceTop(frame = frame with { LogicalDiscardInvocation = draft });
        }
        return frame;
    }

    public ProgramSkillFrame CompleteFrame(ProgramSkillFrame frame)
    {
        if (!TracksInvocations) return frame;
        frame = frames.ActiveProgram(frame.Id);
        if (frame.LogicalDiscardInvocation is not { } draft) return frame;
        frames.ReplaceTop(frame = frame with { LogicalDiscardInvocation = null });
        Complete(draft);
        return frames.ActiveProgram(frame.Id);
    }
}
