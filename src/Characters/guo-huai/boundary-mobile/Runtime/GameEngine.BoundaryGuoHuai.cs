namespace CardGame.Core;

public sealed partial class GameEngine
{
    private bool HasBoundaryGuoHuaiJingce => _contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.PrepareBoundaryGuoHuaiJingce);
    private void CaptureBoundaryGuoHuaiDiscardBatch(CardMovementBatchContext batch)
    {
        if (!HasBoundaryGuoHuaiJingce || batch.TurnNumber != _turnNumber || batch.ParentFrameId is not { } parent ||
            _currentSeat != _turnProgression.OwnerSeat || _turnNumber <= 0) return;
        foreach (var move in batch.Movements.Where(m => IsNativeTrueUseDiscardMovement(batch, m)))
        {
            var entry = _cardMovements.Last(m => m.CardId == move.CardId && m.Sequence < move.Sequence);
            var actionId = ProgramEventHistory<NativeTrueUseProcessingEntryEvent>().SingleOrDefault(e => e.ParentFrameId == parent &&
                e.CardId == move.CardId && e.EntrySequence == entry.Sequence)?.ParentActionId ?? 0;
            RecordBoundaryGuoHuaiDiscard(move, parent, actionId, batch.TurnNumber, _turnProgression.OwnerSeat);
        }
    }
    private void RecordBoundaryGuoHuaiDiscard(CardMovementRecord move, long nativeParent, long action, int turn, int actualOwner)
    {
        if (move.From != CardLocation.Processing || move.To != CardLocation.DiscardPile || move.TurnNumber != turn ||
            move.Sequence <= 0 || move.Sequence > _cardMovements.Count || _cardMovements[move.Sequence - 1] != move ||
            ProgramEventHistory<BoundaryGuoHuaiDiscardedEntityEvent>().Any(e => e.MovementSequence == move.Sequence)) return;
        AdvanceEventRulesAndQueueFact(new BoundaryGuoHuaiDiscardedEntityEvent(turn, actualOwner, nativeParent, action, move.CardId, move.Sequence));
    }
    private int BoundaryGuoHuaiCount(int turn, int owner) => ProgramEventHistory<BoundaryGuoHuaiDiscardedEntityEvent>()
        .Count(e => e.TurnNumber == turn && e.ActualTurnOwnerSeat == owner);

    private TurnEndingBoundaryFrame? BoundaryGuoHuaiEnding(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (candidate.SkillId != BoundaryGuoHuaiComposition.Skill || candidate.BindingId != BoundaryGuoHuaiComposition.Ending ||
            context is not { Window: SkillProgramTriggerWindow.TurnEnding, SourceSeat: { } actor, TargetSeat: { } target } ||
            actor != candidate.OwnerSeat || target != actor || context.OwnerSeat != actor || actor != _currentSeat ||
            _resolutionStack.OfType<TurnEndingBoundaryFrame>().SingleOrDefault(f => f.Id == context.ParentFrameId) is not { } ending ||
            ending.OwnerSeat != actor || ending.TurnNumber != _turnNumber || ending.ItemIndex < 0 || ending.ItemIndex >= ending.Items.Count ||
            ending.Items[ending.ItemIndex].Candidate != candidate) return null;
        return ending;
    }
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 56400, SkillProgramEffectOp.PrepareBoundaryGuoHuaiJingce)]
    private bool QualifyBoundaryGuoHuaiJingce(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (context.Window == SkillProgramTriggerWindow.CardResponseCompleted) return true;
        return BoundaryGuoHuaiEnding(candidate, context) is not null && _players[candidate.OwnerSeat] is { IsAlive: true, HasHpValue: true } owner &&
            BoundaryGuoHuaiCount(_turnNumber, _turnProgression.OwnerSeat) >= owner.Hp;
    }
    [ProgramCapability]
    private sealed class BoundaryGuoHuaiCapability(GameEngine engine) : IBoundaryGuoHuaiHost
    { public void Prepare(ProgramSkillFrame frame) => engine.PrepareBoundaryGuoHuaiJingce(frame); }
    private void PrepareBoundaryGuoHuaiJingce(ProgramSkillFrame supplied)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (frame.WindowContext is not { } context || frame.InstructionIndex != 1)
            throw new InvalidOperationException("Mobile Jingce lost its original native observation frame.");
        if (context.Window == SkillProgramTriggerWindow.CardResponseCompleted)
        {
            if (_resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(f => f.Id == context.ParentFrameId) is not
                    { ResponseCompletion: { } completion } window || completion.ActionId != window.Action.ActionId ||
                completion.ActualTurnNumber != _turnNumber || completion.ActualTurnOwnerSeat != _turnProgression.OwnerSeat ||
                window.Action.Type != CardActionType.Response)
                throw new InvalidOperationException("Mobile Jingce requires the exact typed response completion and native materials.");
            foreach (var move in completion.NativeCosts)
                RecordBoundaryGuoHuaiDiscard(move, completion.ParentFrameId, completion.ActionId, completion.ActualTurnNumber, completion.ActualTurnOwnerSeat);
            return;
        }
        if (_resolutionStack.OfType<TurnEndingBoundaryFrame>().SingleOrDefault(f => f.Id == context.ParentFrameId) is not { } ending ||
            ending.ItemIndex < 0 || ending.ItemIndex >= ending.Items.Count || ending.Items[ending.ItemIndex].Candidate is not { } candidate ||
            BoundaryGuoHuaiEnding(candidate, context) is null || candidate.SkillInstanceId != frame.SkillInstanceId || candidate.GameplayHash != frame.GameplayHash)
            throw new InvalidOperationException("Mobile Jingce lost its own original actual Ending candidate.");
        var count = BoundaryGuoHuaiCount(_turnNumber, _turnProgression.OwnerSeat);
        var owner = _players[frame.OwnerSeat];
        if (!owner.IsAlive || !owner.HasHpValue || count < owner.Hp)
            throw new InvalidOperationException("Mobile Jingce no longer satisfies its actual discarded-entity threshold.");
        AdvanceEventRulesAndQueueFact(new BoundaryGuoHuaiEndingQualifiedEvent(frame.Id,
            new(frame.SkillId, frame.TriggerId!, frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash,
            ending.Id, ending.TurnNumber, ending.OwnerSeat, count, owner.Hp, _movementSequence));
    }
}
