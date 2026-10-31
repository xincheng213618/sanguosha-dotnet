#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunRuOlRuntime
{
    internal const string SunRuOlChishiDrawReason = "skill-program.sun-ru.ol.chishi.draw";
    internal CardConversionSource SunRuOlSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome global::CardGame.Core.ISunRuOlProgramHost.RewardCurrentTurnLastZoneLoss(ProgramSkillFrame f) => BeginSunRuOlChishi(f);
    SkillProgramStepOutcome global::CardGame.Core.ISunRuOlProgramHost.AbolishEquipmentSlotsForDistinctBenefits(ProgramSkillFrame f) => BeginSunRuOlWeimian(f);
    internal static bool IsSunRuOlChishiTrigger(SkillProgramTrigger t) => global::CardGame.Core.RuntimeSkills.RuntimeRules_sun_ru_ol_SunRuOlChishi.IsSunRuOlChishiTrigger(t);
    internal bool SunRuOlActualTurn(SunRuOlTurnToken t) => t.TurnNumber > 0 && t.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && t.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(t.OwnerSeat) && _coreProgramEventHistoryOperations.ProgramEventHistory<TurnStartedEvent>().Any(e => e.TurnNumber == t.TurnNumber && e.ActorSeat == t.OwnerSeat);
    internal static bool SunRuOlLostLastZone(CardMovementBatchContext batch, int seat) => global::CardGame.Core.RuntimeSkills.RuntimeRules_sun_ru_ol_SunRuOlChishi.SunRuOlLostLastZone(batch, seat);
    internal CardMovementBatchContext CaptureSunRuOlChishiBatch(CardMovementBatchContext batch)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.RewardCurrentTurnLastZoneLoss))
            return batch;
        var turn = new SunRuOlTurnToken(batch.TurnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat);
        return SunRuOlActualTurn(turn) && SunRuOlLostLastZone(batch, turn.OwnerSeat) ? batch with
        {
            SunRuOlLastZoneLoss = turn
        }

        : batch;
    }

    internal bool SunRuOlChishiUsed(int owner, string skill, SunRuOlTurnToken turn) => _coreProgramEventHistoryOperations.ProgramEventHistory<SunRuOlChishiAppliedEvent>().Any(e => e.Source.OwnerSeat == owner && e.Source.SkillId == skill && e.Turn == turn);
    internal IEnumerable<ProgramTriggerCandidate> CollectSunRuOlChishiCandidates(CardMovementBatchContext batch)
    {
        if (batch.SunRuOlLastZoneLoss is not { } turn || !SunRuOlActualTurn(turn) || !_coreMatchParticipantsPlayersAccess._players[turn.OwnerSeat].IsAlive || !SunRuOlLostLastZone(batch, turn.OwnerSeat))
            return[];
        return _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).SelectMany(p => _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(p, SkillProgramTriggerWindow.CardsMoved)).Where(c => IsSunRuOlChishiTrigger(_coreProgramLifecycleOperations.GetProgramTrigger(c)) && !SunRuOlChishiUsed(c.OwnerSeat, c.SkillId, turn)).ToArray();
    }

    internal ProgramSkillWindowContext CreateSunRuOlChishiContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate c)
    {
        var actor = frame.Batch.SunRuOlLastZoneLoss?.OwnerSeat ?? throw new InvalidOperationException("Chishi lost its frozen native actual-turn owner.");
        return new(SkillProgramTriggerWindow.CardsMoved, frame.Id, c.OwnerSeat, SourceSeat: actor, TargetSeat: actor, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat]), MovementBatch: frame.Batch);
    }

    internal bool CanRunSunRuOlChishiCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => context.MovementBatch is { SunRuOlLastZoneLoss: { } turn } batch && SunRuOlActualTurn(turn) && context.Window == SkillProgramTriggerWindow.CardsMoved && context.ParentFrameId == batch.Id && context.OwnerSeat == c.OwnerSeat && context.SourceSeat == turn.OwnerSeat && context.TargetSeat == turn.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[turn.OwnerSeat].IsAlive && SunRuOlLostLastZone(batch, turn.OwnerSeat) && !SunRuOlChishiUsed(c.OwnerSeat, c.SkillId, turn);
    internal SkillProgramStepOutcome BeginSunRuOlChishi(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.SunRuOlChishi is not null || f.InstructionIndex != 1 || f.TriggerId is null || f.WindowContext is not { MovementBatch.SunRuOlLastZoneLoss: { } turn } context || _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().SingleOrDefault(native => native.Id == context.ParentFrameId)is not { } w || w.CandidateIndex >= w.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex]) || !CanRunSunRuOlChishiCandidate(w.Candidates[w.CandidateIndex], context))
            throw new InvalidOperationException("Chishi requires one real last-zone loss of the actual current-turn actor.");
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SunRuOlChishi = new(SunRuOlSource(f), f.GameplayHash, f.InstructionIndex, w.Id, turn, before, before) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SunRuOlChishiAppliedEvent(f.Id, SunRuOlSource(f), w.Id, turn));
        var granted = _turnCardUseEffectsOperations._turnCardUseEffects.GrantRuleModifier(turn.TurnNumber, turn.OwnerSeat, f.Id, f.InstructionIndex - 1, global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(f), SkillRuleQuery.HandLimit, SkillRuleOperation.Add, 2, affectedSeat: turn.OwnerSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TurnRuleModifierGrantedEvent(granted));
        _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[turn.OwnerSeat], 2, true, new(SunRuOlChishiDrawReason));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { SunRuOlChishi = f.SunRuOlChishi!with { SequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        ContinueSunRuOl(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 3200)]
    internal void CollectWindow_CollectSunRuOlChishiCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => result.AddRange(CollectSunRuOlChishiCandidates(batch));
    [ProgramPipelineHook(ProgramPipelinePhase.ExcludeOrdinaryMovementCandidate, 3100)]
    internal bool ExcludeOrdinaryMovement_IsSunRuOlChishiTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => IsSunRuOlChishiTrigger(trigger);
    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 4000)]
    internal ProgramSkillWindowContext? TryCreateSunRuOlChishiContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        var trigger = _coreProgramLifecycleOperations.GetProgramTrigger(candidate);
        if (IsSunRuOlChishiTrigger(trigger))
            return CreateSunRuOlChishiContext(frame, candidate);
        return null;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CompleteMovementBatch, 2400)]
    internal CardMovementBatchContext CompleteMovementBatchCaptureSunRuOlChishiBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => CaptureSunRuOlChishiBatch(completed);
    [ProgramTriggerProof(ProgramTriggerProofPhase.MovementOwner, 90, ProgramTriggerProofMatch.Decision)]
    internal bool? ProveSunRuMovement(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsSunRuOlChishiTrigger(trigger) ? CanRunSunRuOlChishiCandidate(candidate, context) : null;
}
