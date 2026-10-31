#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineActorHandLimitTargetProgramsRuntime
{
    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedInstantTrickRequirement, 90)]
    internal bool ActorHandLimitTargetFinalizedInstantTrickRequirementTargetWindow(ProgramTargetCardWindowQuery query) => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.GrantActorHandLimitPenalty);
    internal bool HasActorHandLimitTargetPrograms => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.GrantActorHandLimitPenalty);

    internal bool ShouldCollectActorHandLimitPenalty(CardActionContext action, int ownerSeat, SkillProgramTriggerWindow window)
    {
        if (!HasActorHandLimitTargetPrograms || window != SkillProgramTriggerWindow.CardUseTargetsFinalized || action.Type != CardActionType.Use || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat) || !_coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive || ownerSeat == action.ActorSeat || LifecycleActorHandLimitUse(action)is not { } use || use.ActorHandLimitAnnouncedTargets?.Contains(ownerSeat) == true)
            return false;
        if (CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind))
            return action.EffectiveDesignatedTargetSeats.Contains(ownerSeat) && use.TargetSeats.Contains(ownerSeat);
        if (!CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(action.EffectiveKind))
            return false;
        return action.EffectiveKind == CardKind.BorrowedSword ? action.TargetSeats.Where((_, index) => index % 2 == 0).Contains(ownerSeat) : action.EffectiveDesignatedTargetSeats.Contains(ownerSeat);
    }

    internal CardUseFrame? LifecycleActorHandLimitUse(CardActionContext action) => _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault(use => use.Action is { Type: CardActionType.Use } current && current.ActionId == action.ActionId && current.ActorSeat == action.ActorSeat && current.ProviderSeat == action.ProviderSeat && current.EffectiveKind == action.EffectiveKind && use.SourceSeat == action.ActorSeat && use.CardKind == action.EffectiveKind);
    internal IReadOnlyList<int> ActorHandLimitPrimaryTargets(CardActionContext action)
    {
        var targets = (action.EffectiveKind == CardKind.BorrowedSword ? action.TargetSeats.Where((_, index) => index % 2 == 0) : action.EffectiveDesignatedTargetSeats).Distinct().ToArray();
        if (targets.Any(seat => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(seat)))
            throw new InvalidOperationException("An actor hand-limit announcement requires real primary target seats.");
        return Array.AsReadOnly(targets);
    }

    internal void RecordActorHandLimitTargetAnnouncements(CardActionContext action, SkillProgramTriggerWindow window)
    {
        if (!HasActorHandLimitTargetPrograms || window != SkillProgramTriggerWindow.CardUseTargetsFinalized || action.Type != CardActionType.Use || !CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) && !CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(action.EffectiveKind) || LifecycleActorHandLimitUse(action)is not { } use)
            return;
        var announced = (use.ActorHandLimitAnnouncedTargets ?? []).Concat(ActorHandLimitPrimaryTargets(action)).Distinct().ToArray();
        if (use.ActorHandLimitAnnouncedTargets is { } old && old.SequenceEqual(announced))
            return;
        _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { ActorHandLimitAnnouncedTargets = announced });
    }

    internal bool IsActorHandLimitTargetCandidate(ProgramCardTriggerCandidate candidate) => _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(candidate.SkillId)?.Program is { } program && program.GameplayHash == candidate.GameplayHash && ProgramInstructionResolver.Default.FindTrigger(program, candidate.TriggerId)?.Effects.Any(effect => effect.Op == SkillProgramEffectOp.GrantActorHandLimitPenalty) == true;
    internal static CardActionContext ActorHandLimitTargetCollectionAction(CardUseFrame use) => RuntimeSkills.RuntimeRules_Core_ActorHandLimitTargetPrograms.ActorHandLimitTargetCollectionAction(use);
    internal bool HasIssuedActorHandLimitTargetCandidate(CardUseFrame use, ProgramCardTriggerCandidate candidate)
    {
        return _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramActorHandLimitPenaltyGrantedEvent>().Any(fact => fact.Penalty.CardUseFrameId == use.Id && fact.Penalty.ActionId == use.Action!.ActionId && fact.Penalty.TargetSeat == candidate.OwnerSeat && fact.Penalty.Source.OwnerSeat == candidate.OwnerSeat && fact.Penalty.Source.SkillId == candidate.SkillId && fact.Penalty.Source.BindingId == candidate.TriggerId);
    }

    internal IReadOnlyList<ProgramCardTriggerCandidate> CollectActorHandLimitTargetCandidates(CardUseFrame use, long windowId, IReadOnlyList<ProgramCardTriggerCandidate> existing)
    {
        var action = ActorHandLimitTargetCollectionAction(use);
        var candidates = _coreCardActionsOperations.CollectSharedCardActionCandidates(action, SkillProgramTriggerWindow.CardUseTargetsFinalized, use.TargetSeats, cardUseCausedDamage: null, continuation: CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) ? ProgramCardContinuation.Slash : ProgramCardContinuation.FinalizedTrick).Where(IsActorHandLimitTargetCandidate).Where(candidate => ShouldCollectActorHandLimitPenalty(action, candidate.OwnerSeat, SkillProgramTriggerWindow.CardUseTargetsFinalized)).Where(candidate => !existing.Any(old => old.OwnerSeat == candidate.OwnerSeat && old.SkillId == candidate.SkillId && old.TriggerId == candidate.TriggerId && old.OpponentSeat == candidate.OpponentSeat)).Where(candidate => !HasIssuedActorHandLimitTargetCandidate(use, candidate)).OrderByDescending(candidate => candidate.Priority).ThenBy(candidate => _coreTableOrderOperations.RelativeTableOrder(0, candidate.OwnerSeat)).ThenBy(candidate => candidate.SkillId, StringComparer.Ordinal).ThenBy(candidate => candidate.SkillInstanceId, StringComparer.Ordinal).ThenBy(candidate => candidate.TriggerId, StringComparer.Ordinal).Select(candidate => candidate.FrozenContext is { CardUse: { } identity } frozen ? candidate with { FrozenContext = frozen with { ParentFrameId = windowId, CardUse = identity with { ParentCardUseFrameId = use.Id } } } : throw new InvalidOperationException("An actor hand-limit target lost its frozen use identity.")).ToArray();
        return Array.AsReadOnly(candidates);
    }

    internal ProgramCardTriggerWindowFrame AppendActorHandLimitTargetCandidates(ProgramCardTriggerWindowFrame frame)
    {
        if (!HasActorHandLimitTargetPrograms || _coreMatchOutcomeWinnerAccess._winner != Winner.None || CoreGameEngineCardActionsRuntime.GetCardActionWindow(frame) != SkillProgramTriggerWindow.CardUseTargetsFinalized || _coreCardUseLifecycleOperations.LifecycleCardUse(frame.ParentFrameId)is not { Action: { Type: CardActionType.Use } action } use || action.ActionId != frame.Action.ActionId || action.ActorSeat != frame.Action.ActorSeat || action.EffectiveKind != frame.Action.EffectiveKind || !CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) && !CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(use.CardKind))
            return frame;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramCardTriggerWindowFrame current || current.Id != frame.Id)
            throw new InvalidOperationException("Actor hand-limit target collection lost its exact native window.");
        var added = CollectActorHandLimitTargetCandidates(use, frame.Id, frame.Candidates);
        RecordActorHandLimitTargetAnnouncements(ActorHandLimitTargetCollectionAction(use), SkillProgramTriggerWindow.CardUseTargetsFinalized);
        if (added.Count == 0)
            return frame;
        // Keep the completed prefix, live cursor, old Action and every old frozen
        // context. An announced target still owns that frozen notification after
        // removal or effect cancellation. Some older producers do not sync Action.
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Candidates = Array.AsReadOnly(frame.Candidates.Concat(added).ToArray()) });
        return frame;
    }

    internal bool TryBeginAcceptedSlashActorHandLimitTargetPrograms(NativeOwners.CardAttackHandle attack)
    {
        if (!HasActorHandLimitTargetPrograms || _coreMatchOutcomeWinnerAccess._winner != Winner.None || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { ProgramUseAccepted: true, Action: { Type: CardActionType.Use } action } use || !CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || action.EffectiveKind != use.CardKind || action.ActorSeat != use.SourceSeat || use.CardAttack is not { } currentAttack || currentAttack.TargetSeat != attack.TargetSeat || !use.TargetSeats.Contains(attack.TargetSeat))
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != use.Id)
            throw new InvalidOperationException("An accepted Slash target notification lost its owning use.");
        // The first finalized window announced all its then-current targets.
        // A subsequent native tail redirection announces a new target identity;
        // prior identities remain issued, and exact action/target/source facts
        // prevent either an unchanged tail or a redirect onto an old target from
        // issuing that same notification again.
        var windowId = _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1;
        var candidates = CollectActorHandLimitTargetCandidates(use, windowId, []);
        RecordActorHandLimitTargetAnnouncements(ActorHandLimitTargetCollectionAction(use), SkillProgramTriggerWindow.CardUseTargetsFinalized);
        if (candidates.Count == 0)
            return false;
        _coreResolutionIdentityBoundaryOperations._resolutionSequence++;
        _coreRuntimeOperations.PushRuntimeFrame(new ProgramCardTriggerWindowFrame(windowId, use.Id, action, ProgramCardContinuation.Slash, candidates, AttackOwnerFrameId: use.Id));
        _coreRuntimeOperations.AdvanceRuntimeTop<ProgramCardTriggerWindowFrame>();
        return true;
    }
}
