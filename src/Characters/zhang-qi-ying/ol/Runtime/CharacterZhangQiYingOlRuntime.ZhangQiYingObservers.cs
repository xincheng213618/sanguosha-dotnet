#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangQiYingOlRuntime
{
    internal bool HasZhangQiYingCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.InitializeFaluSuitTokens) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.GainFaluTokensAfterActualDiscard) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ReorderTopByCurrentFaluTokens) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ChooseFinalJudgmentFiveByZiwei) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.IncreaseActualSourceDamageByYuqing) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.GainThreeDeckCategoriesAfterElementalDamage);

    internal void AssertZhangQiYing(ProgramSkillFrame f)
    {
        if (f.ZhangQiYing is not { } r || r.Source != ZhangQiYingSource(f) || r.GameplayHash != f.GameplayHash || r.Instruction != f.InstructionIndex || r.ParentId != f.WindowContext?.ParentFrameId || r.Before > r.After || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            throw new InvalidOperationException("Zhang Qi Ying lost its exact owning source and instruction.");
        if (r.Stage == "gain-children" && (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ZhenyiDeckGainPaidEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.Before == r.Before && e.After == r.After && e.ActualCount == r.Cards.Count) != 1 || r.Cards.Count > 3 || r.Cards.Distinct().Count() != r.Cards.Count || f.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null, SubjectSeat: var seat } || seat != f.OwnerSeat || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= r.After && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == ZhenyiGainReason && r.Cards.Contains(m.CardId)) != r.Cards.Count))
            throw new InvalidOperationException("Gouchen needs one exact already-paid three-category gain invoice.");
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 260)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 260)]
    internal bool CanContinueZhangQiYing(ProgramSkillFrame f)
    {
        if (f.ZhangQiYing is not { Stage: "gain-children" })
            return false;
        AssertZhangQiYing(f);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 24405)]
    internal bool ResumeZhangQiYing(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ZhangQiYing: { Stage: "gain-children" } } f || f.Id != id)
            return false;
        AssertZhangQiYing(f);
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhangQiYing = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 24405)]
    internal bool ReturnZhangQiYingMovement(ProgramSkillFrame f) => CanContinueZhangQiYing(f) && ResumeZhangQiYing(f.Id);
    internal bool ZhangQiYingFirstChild(ProgramSkillFrame f, ResolutionFrame child) => f.ZhangQiYing is { Stage: "gain-children" } r && CanContinueZhangQiYing(f) && child is CardsMovedTriggerWindowFrame moved && moved.Batch.ParentFrameId == f.Id && moved.Batch.AwaitingProgramFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && r.Cards.Contains(m.CardId) && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == ZhenyiGainReason);
    internal ProgramSkillFrame? ZhangQiYingObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((r, c) => _characterZhangQiYingOlOperations.ZhangQiYingFirstChild(r, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 330)]
    internal bool TryAdvanceZhangQiYingSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && ZhangQiYingObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
