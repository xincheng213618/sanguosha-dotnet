#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineJudgedRankSlashPoliciesRuntime
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 180)]
    internal bool AllowsRegisteredIsOtherActualBasicDiscardTrigger(ProgramTriggerLegalityContext gate)
    {
        var candidate = gate.Candidate;
        var trigger = gate.Trigger;
        var context = gate.Context;
        return !(IsOtherActualBasicDiscardTrigger(trigger) && !CanRunOtherActualBasicDiscard(candidate, context));
    }

    internal bool HasOtherActualBasicDiscardCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.DrawFromOtherActualBasicDiscard);
    internal bool HasJudgedRankSlashCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.GrantJudgedRankSplitSlashTurnPolicy);

    internal static bool IsOtherActualBasicDiscardTrigger(SkillProgramTrigger trigger) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_JudgedRankSlashPolicies.IsOtherActualBasicDiscardTrigger(trigger);
    internal bool OtherActualBasicDiscardAlreadyIssued(int owner, string skill) => _coreProgramEventHistoryOperations.ProgramEventHistory<OtherActualBasicDiscardDrawIssuedEvent>().Any(e => e.Source.OwnerSeat == owner && e.Source.SkillId == skill && e.ActualTurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.ActualTurnOwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat);
    internal int[] MatchingOtherActualBasicDiscardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate)
    {
        if (batch.Id <= 0 || batch.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || batch.MovementTiming is not { } timing || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(timing.ActualTurnOwnerSeat) || timing.ActualTurnOwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || timing.ActualTurnOwnerSeat == candidate.OwnerSeat || OtherActualBasicDiscardAlreadyIssued(candidate.OwnerSeat, candidate.SkillId) && _coreProgramLifecycleOperations.GetProgramTrigger2(candidate).UsageScope == SkillUsageScope.Turn || _coreProgramEventHistoryOperations.ProgramEventHistory<OtherActualBasicDiscardDrawIssuedEvent>().Any(e => e.Source.OwnerSeat == candidate.OwnerSeat && e.Source.SkillId == candidate.SkillId && e.MovementBatchId == batch.Id))
            return[];
        return batch.Movements.Select((m, index) => (m, index)).Where(p => p.m.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(p.m) && _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(p.m)is { OwnerSeat: var other } && other is { } seat && seat != candidate.OwnerSeat && global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.MatchesSkillProgramCardCategory(p.m.CardKind, SkillProgramCardCategory.Basic)).Select(p => p.index).ToArray();
    }

    internal bool CanRunOtherActualBasicDiscard(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => context.Window == SkillProgramTriggerWindow.DiscardPileReceived && context.MovementBatch is { } batch && context.MovementIndex is { } index && MatchingOtherActualBasicDiscardIndexes(batch, candidate).Contains(index) && context.SourceSeat == _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(batch.Movements[index])?.OwnerSeat;
    internal void DrawFromOriginalOtherBasicDiscard(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var candidate = new ProgramTriggerCandidate(frame.OwnerSeat, frame.SkillId, frame.TriggerId!, frame.SkillInstanceId, frame.GameplayHash, 0);
        if (frame.InstructionIndex != 1 || frame.TriggerId is null || frame.WindowContext is not { } context || !IsOtherActualBasicDiscardTrigger(_coreProgramLifecycleOperations.GetProgramTrigger(frame)) || !CanRunOtherActualBasicDiscard(candidate, context))
            throw new InvalidOperationException("A basic discard benefit lost its original accepted actual-turn movement.");
        var batch = context.MovementBatch!;
        var movement = batch.Movements[context.MovementIndex!.Value];
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OtherActualBasicDiscardDrawIssuedEvent(frame.Id, global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(frame), frame.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, batch.MovementTiming!.ActualTurnOwnerSeat, batch.Id, movement.Sequence, _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(movement)!.Value.OwnerSeat!.Value, movement.CardId));
        // Issuance consumes the quota even if the deck is empty. Generic Draw owns all gain observers and returns.
        _coreProgramLifecycleOperations.DrawProgramCards(frame.Id, frame.OwnerSeat, 1, null, null, SkillProgramCardSetVisibility.Private, new($"skill-program.{(frame.SkillId)}.{(SkillProgramEffectOp.DrawFromOtherActualBasicDiscard)}"));
    }

    internal void GrantOriginalJudgedRankSlashPolicy(ProgramSkillFrame supplied, string bind)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var set = _coreProgramLifecycleOperations.GetProgramCardSet(frame, bind);
        var plan = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!);
        if (frame.InstructionIndex != 2 || frame.WindowContext is not null || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || frame.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || frame.SelectedCardIds.Count != 0 || frame.SelectedTargetSeats.Count != 0 || plan.Instructions is not [{ Op: SkillProgramEffectOp.StartJudgment, Target: SkillProgramEffectTarget.Owner, ResultBind: var result, JudgmentReason: var reason }, { Op: SkillProgramEffectOp.GrantJudgedRankSplitSlashTurnPolicy, SourceBind: var input }] || result != bind || input != bind || set.Visibility != SkillProgramCardSetVisibility.Public)
            throw new InvalidOperationException("A rank-split Slash policy requires its own completed owner judgment.");
        var fact = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<JudgmentResolvedEvent>().Single(e => e.ParentResolutionId == frame.Id && e.TargetSeat == frame.OwnerSeat && e.Reason == reason);
        if (fact.CardId is null && fact.Rank is null && set.CardIds.Count == 0)
            return;
        if (fact.CardId is not { } card || fact.Rank is not (>= 1 and <= 13) || fact.CardKind is null || set.CardIds.Count > 1 || set.CardIds.Count == 1 && set.CardIds[0] != card || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<JudgmentRequestedEvent>().Count(e => e.ResolutionId == fact.ResolutionId && e.ParentResolutionId == frame.Id && e.TargetSeat == frame.OwnerSeat && e.Reason == reason) != 1)
            throw new InvalidOperationException("A rank-split Slash policy replaced its finalized physical result.");
        var grant = _turnCardUseEffectsOperations._turnCardUseEffects.GrantJudgedRankSlashPolicy(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, frame.Id, 1, global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(frame), frame.GameplayHash, fact.ResolutionId, card, fact.Rank.Value, plan.Instructions[1].JudgedRankSlashExpiry == JudgedRankSlashPolicyExpiry.ActualPlayPhase ? _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId : null);
        if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TurnJudgedRankSlashPolicyGrantedEvent>().Any(e => e.Policy.ParentFrameId == frame.Id))
            throw new InvalidOperationException("One completed judgment cannot issue its rank policy twice.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TurnJudgedRankSlashPolicyGrantedEvent(grant));
        // This operation consumes the temporary judgment binding. The mature cleanup moves only
        // its exact cards still in Processing; an earlier legitimate judgment claim keeps its card.
        // Generic post-instruction movement draining owns every resulting observer before completion.
        _coreProgramLifecycleOperations.CleanupProgramBoundCards(frame, completed: true);
    }

    internal TurnJudgedRankSlashPolicy? CurrentJudgedRankSlashPolicy(int actor) => !HasJudgedRankSlashCapability || _coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[actor].IsAlive || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase is TurnPhase.NotStarted or TurnPhase.Finished ? null : _turnCardUseEffectsOperations._turnCardUseEffects.JudgedRankSlashPolicies.Where(g => g.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && g.TurnSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && g.Source.OwnerSeat == actor && (g.PhaseInstanceId is null || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat == actor && g.PhaseInstanceId == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId)).OrderByDescending(g => g.GrantSequence).FirstOrDefault();
    internal bool HasJudgedRankSlashDistance(int actor, CardKind kind, int? rank, long? existingUseFrameId = null) => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && (existingUseFrameId is { } id ? HasIssuedJudgedRankSlashDistance(id, actor) : rank is> 0 && CurrentJudgedRankSlashPolicy(actor)is { } grant && rank < grant.Rank);
    internal bool HasJudgedRankSlashQuota(int actor, CardKind kind, int? rank) => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && rank is> 0 && CurrentJudgedRankSlashPolicy(actor)is { } grant && rank > grant.Rank;
    internal bool HasPotentialJudgedRankSlashQuota(int actor, CardKind kind) => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && CurrentJudgedRankSlashPolicy(actor)is { Rank: < 13 };
    internal JudgedRankSlashThresholdSnapshot? JudgedRankSlashSnapshot(int actor) => CurrentJudgedRankSlashPolicy(actor)is { } grant ? new(grant.TurnNumber, grant.Rank) : null;
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 380)]
    internal void ObserveJudgedRankSlashUse(IGameEvent payload)
    {
        if (!HasJudgedRankSlashCapability || payload is not CardUseDeclaredEvent declared || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(declared.CardKind))
            return;
        var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(f => f.Id == declared.ResolutionId);
        if (use?.Action is not { } action || CurrentJudgedRankSlashPolicy(action.ActorSeat)is not { } policy)
            return;
        if (action.Type != CardActionType.Use || action.EffectiveKind != declared.CardKind || use.SourceSeat != declared.SourceSeat || action.ActorSeat != use.SourceSeat || use.JudgedRankSlashUse is not null)
            throw new InvalidOperationException("A judged-rank Slash declaration lost its actual owning action.");
        var rank = action.EffectiveRank;
        var receipt = new JudgedRankSlashUseReceipt(use.Id, action.ActionId, action.ActorSeat, rank, policy, rank is> 0 && rank < policy.Rank, rank is> 0 && rank > policy.Rank);
        _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { JudgedRankSlashUse = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JudgedRankSlashUsePolicyAppliedEvent(receipt));
    }

    internal bool HasIssuedJudgedRankSlashDistance(long? useId, int actor) => useId is { } id && _coreCardUseLifecycleOperations.LifecycleCardUse(id)?.JudgedRankSlashUse is { ActorSeat: var owner, IgnoresDistance: true } && owner == actor;
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.AfterSlashDebitIdentity, 10)]
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.SlashCountExemption, 10)]
    internal bool HasIssuedJudgedRankSlashQuota(long useId, int actor) => _coreCardUseLifecycleOperations.LifecycleCardUse(useId)?.JudgedRankSlashUse is { ActorSeat: var owner, IgnoresQuota: true } && owner == actor;

    void global::CardGame.Core.IJudgedRankSlashProgramHost.GrantJudgedRankSplitSlashTurnPolicy(ProgramSkillFrame frame, string bind) => GrantOriginalJudgedRankSlashPolicy(frame, bind);
    void global::CardGame.Core.IJudgedRankSlashProgramHost.DrawFromOtherActualBasicDiscard(ProgramSkillFrame frame) => DrawFromOriginalOtherBasicDiscard(frame);
    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 140)]
    internal bool DistancePhysicalSlash140HasIssuedJudgedRankSlashDistance(ProgramSlashDistanceQuery q) => HasIssuedJudgedRankSlashDistance(q.ExistingUseFrameId, q.Actor.Seat);
    [ProgramDistanceRule(ProgramDistanceStage.SpecificSlashExemption, 20)]
    internal bool DistanceSpecificSlashExemption20HasJudgedRankSlashDistance(ProgramSlashDistanceQuery q) => HasJudgedRankSlashDistance(q.Actor.Seat, q.Kind, q.EffectiveRank, q.ExistingUseFrameId);
    [ProgramTargetRule(ProgramTargetRulePoint.SlashQuotaAfterLimit, 60)]
    internal bool JudgedRankSlashQuotaAfterLimit(ProgramSlashQuotaQuery query) => HasJudgedRankSlashQuota(query.Actor.Seat, query.Kind, query.EffectiveRank ?? (query.PhysicalCard is not null ? _coreFinalTargetSlashOperations.SpecificSlashRank(query.Actor, query.PhysicalCard, query.Kind) : null));
    [ProgramPipelineHook(ProgramPipelinePhase.SelectLegacyDiscardIndexes, 500)]
    internal int[]? SelectWindowIndexes_MatchingOtherActualBasicDiscardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger)
    {
        var features = ProgramInstructionResolver.Default.Features(trigger);
        if (IsOtherActualBasicDiscardTrigger(trigger))
            return MatchingOtherActualBasicDiscardIndexes(batch, candidate);
        return null;
    }
}
