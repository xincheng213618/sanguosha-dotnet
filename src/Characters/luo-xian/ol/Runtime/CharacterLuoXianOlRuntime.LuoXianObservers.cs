#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuoXianOlRuntime
{
    internal bool ValidDaili(ProgramSkillFrame frame)
    {
        if (frame.Daili is not { } pending)
            return false;
        var receipt = pending.Receipt;
        if (receipt.Source != DailiSource(frame) || receipt.GameplayHash != frame.GameplayHash || receipt.InstructionIndex != frame.InstructionIndex || receipt.InstructionIndex != 1 || receipt.ActualTurn != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || receipt.TurnOwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || receipt.ShownHandCount < 0 || receipt.ShownHandCount % 2 != 0 || frame.WindowContext is not { Window: SkillProgramTriggerWindow.AfterTurnEnded } context || context.ParentFrameId != receipt.ParentFrameId || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(frame)?.Op != SkillProgramEffectOp.ResolveDailiTurnEnd || !Enum.IsDefined(pending.Stage) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DailiTurnEndIssuedEvent>().Count(e => e.FrameId == frame.Id && e.Receipt == receipt) != 1 || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DailiTurnedOverEvent>().Count(e => e.FrameId == frame.Id && e.WasFaceDown == receipt.WasFaceDown && e.IsFaceDown != receipt.WasFaceDown) != 1)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == frame.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not DeferredTurnEndFrame parent || parent.Id != receipt.ParentFrameId || parent.TurnNumber != receipt.ActualTurn || parent.OwnerSeat != receipt.TurnOwnerSeat || !_coreAfterTurnEndedProgramsOperations.MatchesAfterTurnEndedChild(parent, frame, context))
            return false;
        if (pending.Stage == DailiStage.FlipChildren)
            return frame.PendingMovementContinuation is null && pending.Before == 0 && pending.After == 0 && pending.Count == 0;
        if (frame.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } movement || movement.SubjectSeat != frame.OwnerSeat || pending.Before < 0 || pending.After < pending.Before || pending.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || pending.Count is < 0 or > 3 || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DailiDrawPaidEvent>().Count(e => e.FrameId == frame.Id && e.Before == pending.Before && e.After == pending.After && e.Count == pending.Count) != 1)
            return false;
        var drawn = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DailiCardDrawnEvent>().Where(e => e.FrameId == frame.Id).ToArray();
        return drawn.Length == pending.Count && drawn.Select(e => e.MovementSequence).Distinct().Count() == drawn.Length && drawn.All(e => e.OwnerSeat == frame.OwnerSeat) && DailiDrawnMovements(frame).All(m => m.Sequence > pending.Before && m.Sequence <= pending.After && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(frame.OwnerSeat) && m.Reason.Value == "skill-program.daili.draw") && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > pending.Before && m.Sequence <= pending.After && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(frame.OwnerSeat) && m.Reason.Value == "skill-program.daili.draw") == pending.Count;
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 26420)]
    internal void RequireDaili(ProgramSkillFrame frame)
    {
        if (frame.Daili is not null && !ValidDaili(frame))
            throw new InvalidOperationException("Daili lost its exact ended-turn candidate, paid flip, original drawn entities or native child return.");
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 26420)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 26420)]
    internal bool ContinueIssuedDaili(ProgramSkillFrame frame) => ValidDaili(frame);
    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 110)]
    internal bool IsDailiMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation movement) => effect?.Op == SkillProgramEffectOp.ResolveDailiTurnEnd && frame.Daili is { Stage: DailiStage.DrawChildren } && movement is { BeforeCount: 0, CoverageResultBind: null } && movement.SubjectSeat == frame.OwnerSeat && ValidDaili(frame);
    internal bool DailiFirstChild(ProgramSkillFrame frame, ResolutionFrame child)
    {
        if (!ValidDaili(frame) || frame.Daili is not { } pending)
            return false;
        if (pending.Stage == DailiStage.FlipChildren && child is ProgramLifecycleTriggerWindowFrame flip)
            return flip.Window is SkillProgramTriggerWindow.CharacterTurnedOver or SkillProgramTriggerWindow.CharacterTurnedFaceUp && flip.ResumeProgramFrameId == frame.Id && flip.CharacterStateContinuation == CharacterStateContinuation.Program && flip.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && flip.OwnerSeat == frame.OwnerSeat && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CharacterStateChangedEvent>().Any(e => e.Change.Id == flip.Id && e.Change.ParentFrameId == frame.Id && e.Change.TargetSeat == frame.OwnerSeat);
        return pending.Stage == DailiStage.DrawChildren && child is CardsMovedTriggerWindowFrame moved && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(frame, moved) && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > pending.Before && m.Sequence <= pending.After && m.Reason.Value == "skill-program.daili.draw");
    }

    internal ProgramSkillFrame? DailiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => _characterLuoXianOlOperations.DailiFirstChild(root, child), (index) => _coreObserverSubtreesOperations2.HasMovementObserverSuffix(index));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 420)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 530)]
    internal bool HasDailiDamageObserver(long id) => DailiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 560)]
    internal bool HasDailiBeforeDamageObserver(long id) => DailiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is BeforeDamageProgramWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 390)]
    internal bool IsDailiDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && DailiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 26420)]
    internal bool AdvanceDailiSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && DailiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
