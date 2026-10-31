#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineOwnTrickDrawTargetsRuntime
{
    internal const string OwnTrickDrawReason = "program.own-multi-target-trick.draw";
    internal static int ActualTrickTargetCount(CardUseFrame use) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_OwnTrickDrawTargets.ActualTrickTargetCount(use);
    internal bool HasOwnMultiTargetTrickDraw => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.DrawThenNullifyOwnMultiTargetTrick);

    internal bool OwnMultiTargetTrickCandidate(ProgramTriggerCandidate c) => _coreProgramLifecycleOperations.GetProgramTrigger2(c).Effects.Any(e => e.Op == SkillProgramEffectOp.DrawThenNullifyOwnMultiTargetTrick);
    internal bool HasOwnMultiTargetTrickSelfTarget(CardUseFrame use, int seat) => HasOwnMultiTargetTrickDraw && use.SourceSeat == seat && ActualTrickTargetCount(use) > 1 && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(use.CardKind) && _coreSkillProgramsOperations.EnabledSkillPrograms(_coreMatchParticipantsPlayersAccess._players[seat]).Any(p => p.Triggers.Any(t => t.Effects.Any(e => e.Op == SkillProgramEffectOp.DrawThenNullifyOwnMultiTargetTrick)));
    internal bool HasOwnTrickDrawOfferProcessing(IReadOnlyList<Card> processing)
    {
        if (processing.Count == 0 || !HasOwnMultiTargetTrickDraw || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^1] is not ActualUseTargetWindowFrame { ReturnKind: ActualUseTargetReturnKind.OrdinaryTrick, Step: ResolutionFrameStep.AwaitingResponse, TrickReturn: { } ret } parent || _resolutionStackOperations._resolutionStack[^2] is not CardUseFrame { Action.Type: CardActionType.Use } use || use.Id != parent.ParentFrameId || parent.Candidates.Count != parent.Contexts.Count || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count)
            return false;
        var candidate = parent.Candidates[parent.CandidateIndex];
        var context = parent.Contexts[parent.CandidateIndex];
        var action = use.Action!;
        if (!OwnMultiTargetTrickCandidate(candidate) || !_coreActualUseTargetProgramsOperations.CanRunActualUseTarget(candidate, context) || !CanOfferOwnTrickOrHandCategory(candidate, _coreProgramLifecycleOperations.GetProgramTrigger2(candidate), context) || action.ActorSeat != use.SourceSeat || action.EffectiveKind != use.CardKind || !action.TargetSeats.SequenceEqual(use.TargetSeats) || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } decision || decision.PlayerSeat != candidate.OwnerSeat || decision.SkillPrompt?.SkillId != candidate.SkillId || decision.Choices.Count != 2 || !decision.Choices.All(choice => choice.Parameters.GetValueOrDefault("skill-id") == candidate.SkillId && choice.Parameters.GetValueOrDefault("binding-id") == candidate.BindingId && choice.Parameters.GetValueOrDefault("skill-instance-id") == candidate.SkillInstanceId) || !decision.Choices.Select(choice => choice.Parameters.GetValueOrDefault("program-action")).Order().SequenceEqual(new[] { "activate", "skip" }))
            return false;
        // The offer has not pushed a program child yet. Its directly owning use must
        // account for every Processing entity, rather than admitting an arbitrary use frame.
        var physicalIds = action.PhysicalCards.Select(cost => cost.CardId).ToArray();
        return physicalIds.Length > 0 && physicalIds.All(id => id > 0) && physicalIds.Distinct().Count() == physicalIds.Length && physicalIds.Contains(ret.EffectCardId) && physicalIds.Order().SequenceEqual((use.PhysicalCardIds ?? [use.CardId]).Order()) && physicalIds.Order().SequenceEqual(processing.Select(card => card.Id).Order());
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 800)]
    internal bool CanOfferOwnTrickOrHandCategory(ProgramTriggerCandidate c, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.DrawThenNullifyOwnMultiTargetTrick))
            return context.ActualUseTarget is { } identity && identity.TargetSeat == c.OwnerSeat && _coreCardUseLifecycleOperations.LifecycleCardUse(identity.CardUseFrameId)is { Action.Type: CardActionType.Use } use && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(use.CardKind) && ActualTrickTargetCount(use) > 1 && use.TargetSeats.Contains(c.OwnerSeat);
        if (trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.RestrictDamageSourceHandCategory))
            return context.Window == SkillProgramTriggerWindow.AfterDamageApplied && context.TargetSeat == c.OwnerSeat && context.SourceSeat is { } source && _coreTableSeatQueriesOperations.IsValidPlayerSeat(source) && _coreMatchParticipantsPlayersAccess._players[source].IsAlive && _coreDamageCursorRuntimeOperations.ActiveDamageTrigger is { } damage && damage.Id == context.ParentFrameId && damage.ParentFrameId == context.DamageFrameId && _coreNativeDamageHandlesOperations.GetDamageTriggerAttack(damage)is { IsSourceLess: false };
        return true;
    }

    internal bool OwnTrickDrawMatches(ProgramSkillFrame root, ActualUseTargetWindowFrame parent)
    {
        if (root.OwnTrickDraw is not { } r || root.InstructionIndex != 1 || r.InstructionIndex != 0 || root.WindowContext is not { Window: SkillProgramTriggerWindow.OtherActualUseTargeted, ActualUseTarget: { } identity } c || c.ParentFrameId != parent.Id || parent.ParentFrameId != r.Use.CardUseFrameId || identity != r.Use || identity.TargetSeat != root.OwnerSeat || !_coreActualUseTargetProgramsOperations.MatchesActualUseTarget(identity) || r.Source != new CardConversionSource(root.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(root), root.OwnerSeat, root.SkillInstanceId) || r.GameplayHash != root.GameplayHash || r.TargetCount < 2 || r.ActualDrawCount is < 0 or > 1 || r.SequenceBefore < 0 || r.SequenceAfter < r.SequenceBefore || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(root, parent.Candidates[parent.CandidateIndex]) || !OwnMultiTargetTrickCandidate(parent.Candidates[parent.CandidateIndex]) || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(identity.EffectiveKind))
            return false;
        var records = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter).ToArray();
        bool Draw(CardMovementRecord m) => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(root.OwnerSeat) && m.Reason.Value == OwnTrickDrawReason;
        return records.Count(Draw) == r.ActualDrawCount && records.All(m => Draw(m) || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<OwnTrickDrawIssuedEvent>().Count(e => e.ProgramFrameId == root.Id && e.Receipt == (r with { Applied = false })) == 1 && (!r.Applied || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<OwnTrickTargetNullifiedEvent>().Count(e => e.ProgramFrameId == root.Id && e.CardUseFrameId == r.Use.CardUseFrameId && e.TargetSeat == root.OwnerSeat && e.Source == r.Source) == 1);
    }

    internal SkillProgramStepOutcome BeginOwnTrickDraw(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.OwnTrickDraw is not null || f.InstructionIndex != 1 || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not ActualUseTargetWindowFrame parent || f.WindowContext?.ActualUseTarget is not { } identity || !_coreActualUseTargetProgramsOperations.CanRunActualUseTarget(parent.Candidates[parent.CandidateIndex], f.WindowContext) || !CanOfferOwnTrickOrHandCategory(parent.Candidates[parent.CandidateIndex], _coreProgramLifecycleOperations.GetProgramTrigger2(parent.Candidates[parent.CandidateIndex]), f.WindowContext))
            throw new InvalidOperationException("An own trick draw requires its exact live multi-target use/candidate.");
        var before = _coreLostHpOwnedGiftsOperations.PaidTargetMovementSequence;
        _coreProgramLifecycleOperations.DrawProgramCards(f.Id, f.OwnerSeat, 1, null, null, SkillProgramCardSetVisibility.Private, new(OwnTrickDrawReason));
        var after = _coreLostHpOwnedGiftsOperations.PaidTargetMovementSequence;
        var actual = _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence > before && m.Sequence <= after && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == OwnTrickDrawReason);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        var r = new ProgramOwnTrickDrawReceipt(0, identity, new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, ActualTrickTargetCount(_coreCardUseLifecycleOperations.LifecycleCardUse(identity.CardUseFrameId)!), before, after, actual);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { OwnTrickDraw = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OwnTrickDrawIssuedEvent(f.Id, r));
        if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(f.Id, f.OwnerSeat) == SkillProgramStepOutcome.Continue)
            ResumeOwnTrickDraw(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 490)]
    internal bool ResumeOwnTrickDraw(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        if (f.OwnTrickDraw is not { } r)
            return false;
        if (_resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not ActualUseTargetWindowFrame parent || !OwnTrickDrawMatches(f, parent))
            throw new InvalidOperationException("Own-trick draw lost its exact original use and committed reward ledger.");
        if (f.PendingMovementContinuation is not null && _coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(id, f.OwnerSeat) != SkillProgramStepOutcome.Continue)
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        if (!r.Applied)
        {
            _coreProgramCardTargetEffectsOperations.MarkCardEffectIneffective(r.Use.CardUseFrameId, r.Use.TargetSeat);
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { OwnTrickDraw = r with { Applied = true } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OwnTrickTargetNullifiedEvent(id, r.Use.CardUseFrameId, r.Use.TargetSeat, r.Source));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardEffectNullifiedEvent(id, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, r.Use.ActorSeat, r.Use.CardUseFrameId, r.Use.EffectiveKind));
        }

        _coreSkillProgramsOperations.FinishProgramSkill(_coreProgramLifecycleOperations.GetActiveProgramFrame(id), completed: true);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 470)]
    internal bool ReturnOwnTrickDrawMovement(ProgramSkillFrame frame)
    {
        if (frame.OwnTrickDraw is null)
            return false;
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        if (frame.PendingMovementContinuation is not { } pending || !IsOwnTrickDrawMovement(frame, effect, pending))
            throw new InvalidOperationException("The issued own-trick draw lost its exact paid movement return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return true;
    }

    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 560)]
    internal bool IsOwnTrickDrawMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation m) => e?.Op == SkillProgramEffectOp.DrawThenNullifyOwnMultiTargetTrick && f.OwnTrickDraw is not null && m.SubjectSeat == f.OwnerSeat && m.BeforeCount == 0 && m.CoverageResultBind is null && _resolutionStackOperations._resolutionStack.OfType<ActualUseTargetWindowFrame>().Any(p => OwnTrickDrawMatches(f, p));
    internal bool OwnTrickDrawFirstChild(ProgramSkillFrame f, ResolutionFrame child) => f.OwnTrickDraw is { } r && f.PendingMovementContinuation is { BeforeCount: 0, CoverageResultBind: null } m && m.SubjectSeat == f.OwnerSeat && child is CardsMovedTriggerWindowFrame first && first.Batch.ParentFrameId == f.Id && (first.Batch.AwaitingProgramFrameId is null || first.Batch.AwaitingProgramFrameId == f.Id) && first.Batch.OriginSkillId == f.SkillId && first.Batch.OriginSkillInstanceId == f.SkillInstanceId && first.Batch.OriginOwnerSeat == f.OwnerSeat && first.Batch.Movements.Count > 0 && first.Batch.Movements.All(x => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(x) && x.Sequence > r.SequenceBefore && x.Sequence <= r.SequenceAfter && x.From == CardLocation.DrawPile && x.To == CardLocation.Hand(f.OwnerSeat) && x.Reason.Value == OwnTrickDrawReason);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 230, group: 10)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 490)]
    internal bool IsOwnTrickDrawProgramDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && _resolutionStackOperations._resolutionStack.OfType<ActualUseTargetWindowFrame>().Any(w => _corePaidOwnTargetObserversOperations.ActualUseTargetObserverRoot(w.ParentFrameId)is { OwnTrickDraw: not null } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id));
    SkillProgramStepOutcome global::CardGame.Core.IOwnTrickAndHandCategoryProgramHost.DrawThenNullifyOwnMultiTargetTrick(ProgramSkillFrame f) => BeginOwnTrickDraw(f);
    void global::CardGame.Core.IOwnTrickAndHandCategoryProgramHost.RestrictDamageSourceHandCategory(ProgramSkillFrame f, SkillProgramCardCategory category) => _coreTurnHandCategoryRestrictionsOperations.GrantDamageSourceHandCategory(f, category);
}
