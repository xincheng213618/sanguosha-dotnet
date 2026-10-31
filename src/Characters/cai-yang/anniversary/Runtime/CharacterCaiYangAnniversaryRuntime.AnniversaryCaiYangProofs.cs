#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaiYangAnniversaryRuntime
{
    internal bool ValidCaiYangDuelUse(CardUseFrame use, ProgramSelectedActorDuelOrigin native)
    {
        if (native.CaiYangXunji is not { } o || use.SelectedActorDuelOrigin != native || use.CardId != 0 || use.CardKind != CardKind.Duel || use.PhysicalCardIds is not { Count: 0 } || use.Action is not { Type: CardActionType.Use, EffectiveKind: CardKind.Duel, PhysicalCards.Count: 0, ConversionChain.Count: 0, EffectiveSuit: Suit.None, EffectiveRank: 0 } a || use.Id != o.UseId || a.ActionId != o.ActionId || a.ProviderSeat != o.Source.OwnerSeat || native.OrderedPairPayment is not null || native.ParticipantMode is not null || native.MouDiaoChanRing is not null || native.XiahouEnShiwu is not null || native.ParentProgramFrameId != o.FrameId || native.InstructionIndex != o.Instruction || native.OwnerSeat != o.Source.OwnerSeat || native.InitialActorSeat != o.Source.OwnerSeat || native.InitialTargetSeat != o.Target || native.SkillId != o.Source.SkillId || native.ActivationId != o.Source.BindingId || native.SkillInstanceId != o.Source.SkillInstanceId || native.GameplayHash != o.Hash || native.TurnNumber != o.ActualTurn || o.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || o.Source.SkillId != AnniversaryCaiYangComposition.Xunji || o.Source.BindingId != AnniversaryCaiYangComposition.Ending || _coreProgramEventHistoryOperations.ProgramEventHistory<CaiYangXunjiIssuedEvent>().Count(e => e.Origin == o) != 1)
            return false;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == use.Id);
        return i > 0 && _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame { AnniversaryCaiYang: { Stage: AnniversaryCaiYangStage.Duel, Duel: { } bound } } p && p.Id == o.FrameId && bound == o && p.InstructionIndex == o.Instruction && p.OwnerSeat == o.Source.OwnerSeat && p.SkillId == o.Source.SkillId && p.SkillInstanceId == o.Source.SkillInstanceId && p.GameplayHash == o.Hash && p.WindowContext is { } w && CaiYangEndingWitness(CaiYangCandidate(p), w, true)is { FrameId: var grant } && grant == o.GrantId;
    }

    internal CaiYangXunjiOrigin? CaiYangDuelDamage(int owner, ProgramSkillWindowContext w)
    {
        if (w.Window != SkillProgramTriggerWindow.AfterDamageApplied || w.Amount <= 0 || w.SourceSeat != owner || w.TargetSeat is not { } target || w.DamageFrameId is not { } damageId || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { IsSourceLess: false, DamageWasApplied: true, EffectiveCardKind: CardKind.Duel } a || a.SourceSeat != owner || a.TargetSeat != target || _coreCardUseLifecycleOperations.LifecycleCardUse(a.ResolutionId)is not { SelectedActorDuelOrigin: { CaiYangXunji: { } o } native } use || o.Source.OwnerSeat != owner || o.Target != target || !ValidCaiYangDuelUse(use, native) || _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().SingleOrDefault(x => x.Id == damageId)is not { } d || d.ParentFrameId != use.Id || d.SourceSeat != owner || d.TargetSeat != target || d.Amount != w.Amount || _coreProgramEventHistoryOperations.ProgramEventHistory<CaiYangXunjiDamageBoundEvent>().Any(e => e.UseId == o.UseId && e.DamageId == damageId))
            return null;
        return o;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 45600)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 45600)]
    internal bool KeepCaiYangIssued(ProgramSkillFrame f)
    {
        if (f.AnniversaryCaiYang is not null)
        {
            return true;
        }

        return f.InstructionIndex is 0 or 1 && f.WindowContext is { } w && AdmitCaiYangXunji(CaiYangCandidate(f), w);
    }

    internal bool CaiYangFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.AnniversaryCaiYang is not { } r)
            return false;
        if (r.Stage == AnniversaryCaiYangStage.Drawing && global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return true;
        if (child is CardsMovedTriggerWindowFrame moved)
            return r.Stage == AnniversaryCaiYangStage.DrawChildren && r.Draw is { } d && moved.Batch.ParentFrameId == f.Id && (moved.ResumeProgramFrameId == f.Id || moved.Batch.AwaitingProgramFrameId == f.Id) && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > d.SequenceBefore && m.Sequence <= d.SequenceAfter);
        if (child is HpChangedTriggerWindowFrame hp)
            return r.Stage == AnniversaryCaiYangStage.RecoveryChildren && r.RecoveryIssued && hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.TargetSeat == f.OwnerSeat;
        if (child is RecoveryReplacementFrame recovery)
            return r.Stage == AnniversaryCaiYangStage.RecoveryChildren && r.RecoveryIssued && recovery.ParentFrameId == f.Id;
        if (child is DamageFrame damage)
            return r.Stage == AnniversaryCaiYangStage.Damage && f.AttackAttempt is { } a && damage.ParentFrameId == f.Id && damage.SourceSeat == r.DamageTarget && damage.TargetSeat == f.OwnerSeat && a.SourceSeat == r.DamageTarget && a.TargetSeat == f.OwnerSeat;
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? CaiYangObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((r, c) => _characterCaiYangAnniversaryOperations.CaiYangFirstChild(r, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 45600)]
    internal bool HasCaiYangBeforeDamageObserver(long id) => CaiYangObserverRoot()is { AnniversaryCaiYang.FirstDamage: not null } r && r.WindowContext?.ParentFrameId == id;
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 45600)]
    internal bool HasCaiYangPaidDamageObserver(long id) => CaiYangObserverRoot()is { AnniversaryCaiYang.Stage: AnniversaryCaiYangStage.Damage } r && r.WindowContext?.ParentFrameId == id;
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 45600)]
    internal bool HasCaiYangObserverDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && CaiYangObserverRoot()is { } r && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == r.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 45600)]
    internal bool AdvanceCaiYangObserver() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && CaiYangObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
