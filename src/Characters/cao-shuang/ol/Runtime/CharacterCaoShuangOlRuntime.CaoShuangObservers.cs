#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoShuangOlRuntime
{
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 26060)]
    internal void AssertCaoShuangProgram(ProgramSkillFrame f)
    {
        if (f.CaoShuangTuogu is { } t)
        {
            if (f.SkillId != "ol:tuogu" || f.TriggerId is null || t.Source != CaoShuangSource(f) || t.GameplayHash != f.GameplayHash || t.InstructionIndex != f.InstructionIndex || t.InstructionIndex != 1 || f.WindowContext is not { Window: SkillProgramTriggerWindow.CharacterDied } c || c.ParentFrameId != t.KillWindowId || c.TargetSeat != t.VictimSeat || _coreMatchParticipantsPlayersAccess._players[t.VictimSeat].IsAlive || t.VictimSeat == f.OwnerSeat || !t.SkillIds.SequenceEqual(CaoShuangPrintedInheritanceSkills(t.VictimSeat)) || t.SkillIds.Count == 0 || t.SequenceBefore < 0 || t.SequenceAfter < t.SequenceBefore || t.SequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _resolutionStackOperations._resolutionStack.OfType<ProgramKillTriggerWindowFrame>().SingleOrDefault(w => w.Id == t.KillWindowId)is not { } kill || kill.DeathFrameId != t.DeathFrameId || kill.CandidateIndex >= kill.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, kill.Candidates[kill.CandidateIndex]) || kill.Contexts[kill.CandidateIndex] != c || !_resolutionStackOperations._resolutionStack.OfType<DeathFrame>().Any(d => d.Id == t.DeathFrameId && d.VictimSeat == t.VictimSeat && !d.KillerProgramsResolved))
                throw new InvalidOperationException("Tuogu lost its exact deceased printed-skill chooser, native death window or owning source.");
            var facts = _coreProgramEventHistoryOperations.ProgramEventHistory<CaoShuangTuoguInheritedEvent>().Where(e => e.FrameId == f.Id).ToArray();
            if (t.Granted ? facts is not [var fact] || fact.Source != t.Source || fact.DeathFrameId != t.DeathFrameId || fact.VictimSeat != t.VictimSeat || fact.OwnerSeat != f.OwnerSeat || !t.SkillIds.Contains(fact.SelectedSkillId) : facts.Length != 0)
                throw new InvalidOperationException("Tuogu inheritance must commit exactly one selected printed skill with its own grant identity.");
            if (!t.Granted && _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id && (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { IsPrivate: true } p || p.PlayerSeat != t.VictimSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(p.Choices, CaoShuangTuoguChoices(f))))
                throw new InvalidOperationException("Tuogu's private selection belongs to the deceased rather than the receiving skill owner.");
        }

        if (f.CaoShuangShanzhuan is { } r)
        {
            if (f.SkillId != "ol:shanzhuan" || f.TriggerId is null || r.Source != CaoShuangSource(f) || r.GameplayHash != f.GameplayHash || r.InstructionIndex != f.InstructionIndex || r.InstructionIndex != 1 || r.TargetSeat == f.OwnerSeat || f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied } c || c.ParentFrameId != r.DamageWindowId || c.SourceSeat != f.OwnerSeat || c.TargetSeat != r.TargetSeat || _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().SingleOrDefault(w => w.Id == r.DamageWindowId)is not { } window || !_coreDamageObserverCursorQueriesOperations.DamageProgramMatchesWindow(f, window) || r.SequenceBefore < 0 || r.SequenceAfter < r.SequenceBefore || r.SequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
                throw new InvalidOperationException("Shanzhuan lost its exact actual damage and owning physical placement receipt.");
            if (r.Placed && (r.CardId is not { } card || r.From is not { } from || from.OwnerSeat != r.TargetSeat || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || r.EffectiveKind is not { } kind || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsDelayedCard(kind) || _coreProgramEventHistoryOperations.ProgramEventHistory<CaoShuangShanzhuanPlacedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var placed] || placed.Source != r.Source || placed.CardId != card || placed.From != from || placed.TargetSeat != r.TargetSeat || placed.EffectiveKind != kind || placed.SequenceBefore != r.SequenceBefore || placed.SequenceAfter != r.SequenceAfter || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter && m.CardId == card && m.From == from && m.To == CardLocation.Judgment(r.TargetSeat) && m.Reason.Value == CaoShuangPlacementReason) != 1))
                throw new InvalidOperationException("A paid Shanzhuan placement must retain one exact real entity movement into Judgment.");
        }
    }

    internal bool CaoShuangFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (root.CaoShuangTuogu is null && root.CaoShuangShanzhuan is null)
            return false;
        AssertCaoShuangProgram(root);
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skill)
            return skill.ResumeProgramFrameId == root.Id && skill.Continuation == ProgramLifecycleContinuation.ResumeParentProgram && CanContinuePaidCaoShuang(root);
        if (root.CaoShuangTuogu is { Granted: true } t && child is CardsMovedTriggerWindowFrame cleanup)
            return cleanup.ResumeProgramFrameId == root.Id && cleanup.Batch.ParentFrameId == root.Id && cleanup.Batch.OriginOwnerSeat == root.OwnerSeat && cleanup.Batch.OriginSkillId == root.SkillId && cleanup.Batch.OriginSkillInstanceId == root.SkillInstanceId && cleanup.Batch.Movements.Count > 0 && cleanup.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > t.SequenceBefore && m.Sequence <= t.SequenceAfter);
        if (root.CaoShuangShanzhuan is not { Placed: true } r)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.ResumeProgramFrameId == root.Id && moved.Batch.ParentFrameId == root.Id && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter && m.CardId == r.CardId && m.Reason.Value == CaoShuangPlacementReason);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == root.Id && hp.Change.ParentFrameId == root.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.TargetSeat == r.TargetSeat && hp.Change.Amount == 1;
        return child is RecoveryReplacementFrame recovery && recovery.ParentFrameId == root.Id && recovery.Return == new RecoveryReplacementReturn(PostEventContinuation.Program, root.Id) && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason == new CardMoveReason(CaoShuangPlacementReason) && recovery.Attempt.SourceSeat == r.TargetSeat && recovery.Attempt.TargetSeat == r.TargetSeat && recovery.Attempt.Amount == 1;
    }

    internal ProgramSkillFrame? CaoShuangObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => _characterCaoShuangOlOperations.CaoShuangFirstChild(root, child), (child) => _coreObserverSubtreesOperations.HasMovementObserverSuffix(child));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 320)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 430)]
    internal bool HasCaoShuangDamageObserver(long id) => CaoShuangObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame or BeforeDamageProgramWindowFrame) && _resolutionStackOperations._resolutionStack.Any(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 290)]
    internal bool IsCaoShuangDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && CaoShuangObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 26060)]
    internal bool TryAdvanceCaoShuangSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && CaoShuangObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
