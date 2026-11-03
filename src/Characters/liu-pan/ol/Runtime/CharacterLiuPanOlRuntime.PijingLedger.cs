#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuPanOlRuntime
{
    [ProgramTargetWindow(ProgramTargetWindowStage.NormalizeImplicitDrawTwo, 40)]
    internal bool LiuPanNormalizeImplicitDrawTwoTargetWindow(ProgramTargetCardWindowQuery query) => HasLiuPanCapability;
    internal bool HasLiuPanCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolvePijingTargetTogglesAndRandomGifts);

    internal static CardConversionSource PijingSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_liu_pan_ol_PijingLedger.PijingSource(f);
    internal bool TryGetPijingUse(ProgramSkillWindowContext context, out CardUseFrame use, out ProgramCardTriggerWindowFrame window)
    {
        use = null !;
        window = null !;
        if (context is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } id } || _coreCardUseLifecycleOperations.LifecycleCardUse(id.ParentCardUseFrameId)is not { Action: { Type: CardActionType.Use } a } u || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { CompletedResponseReturn: null } w || w.ParentFrameId != u.Id || w.Continuation is not (ProgramCardContinuation.Slash or ProgramCardContinuation.FinalizedTrick or ProgramCardContinuation.FinalizedSimpleCard) || a.ActionId != id.CardActionId || a.ActorSeat != u.SourceSeat || a.EffectiveKind != u.CardKind || w.Action.ActionId != a.ActionId || !w.Action.TargetSeats.SequenceEqual(u.TargetSeats) || !a.TargetSeats.SequenceEqual(u.TargetSeats) || a.ResponderSeat is not null || a.OpponentSeat is not null || !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseDeclaredEvent>().Any(e => e.ResolutionId == u.Id && (e.CardKind == u.CardKind || _coreCurrentSlashFireOffersOperations.IsCurrentSlashFireChangedUse(u) && u.CurrentSlashFirePolicy!.OriginalKind == e.CardKind)))
            return false;
        var at = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == w.Id);
        if (at < 1 || _resolutionStackOperations._resolutionStack[at - 1].Id != u.Id)
            return false;
        if (u.CardKind == CardKind.BorrowedSword && (u.TargetSeats.Count % 2 != 0 || u.TargetSeats.Where((_, i) => i % 2 == 0).Distinct().Count() != u.TargetSeats.Count / 2))
            return false;
        use = u;
        window = w;
        return true;
    }

    internal CardUseFrame RequirePijingUse(ProgramSkillFrame f, out ProgramCardTriggerWindowFrame window)
    {
        if (f.WindowContext is not { } c || !TryGetPijingUse(c, out var u, out window) || !_coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidateMatches(new(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0), c, window))
            throw new InvalidOperationException("Pijing lost its exact current native designation candidate.");
        var at = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (at < 2 || _resolutionStackOperations._resolutionStack[at - 1].Id != window.Id || _resolutionStackOperations._resolutionStack[at - 2].Id != u.Id)
            throw new InvalidOperationException("Pijing requires its immediate original use and designation window.");
        return u;
    }

    internal bool PijingInitialEligible(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => c.SkillId == "ol:pijing" && c.BindingId == "toggle-targets" && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && TryGetPijingUse(context, out var u, out var w) && u.SourceSeat == c.OwnerSeat && _coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidateMatches(c, context, w) && (CoreNativeCardClassificationsRuntime.IsSlashCard(u.CardKind) || CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(u.CardKind)) && u.CardKind != CardKind.Nullification && u.Action!.EffectiveDesignatedTargetSeats.Count > 0 && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<PijingStartedEvent>().Any(e => !e.Reciprocal && e.Source.OwnerSeat == c.OwnerSeat && e.Source.SkillId == c.SkillId && e.ActualTurn == _coreActualTurnProgressTurnNumberAccess._turnNumber) && PijingInitialOptions(u, []).Count > 0;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27410)]
    internal bool QualifyPijing(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => t.Effects is [{ Op: SkillProgramEffectOp.ResolvePijingTargetTogglesAndRandomGifts }] ? PijingInitialEligible(c, context) : t.Effects is not [{ Op: SkillProgramEffectOp.ResolvePijingNextSoleTargetChoice }] || IsIssuedPijingCandidate(c, context);
    [ProgramPipelineHook(ProgramPipelinePhase.CollectIssuedCardTrigger, 20)]
    internal void CollectIssuedPijingNextUseCandidates(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result)
    {
        if (!HasLiuPanCapability || window != SkillProgramTriggerWindow.CardUseTargetsFinalized || action.Type != CardActionType.Use || CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(action.EffectiveKind) != SkillProgramCardCategory.Basic && !CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(action.EffectiveKind) || action.EffectiveDesignatedTargetSeats.Count != 1 || _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault(u => u.Action?.ActionId == action.ActionId)is not { } use || !_coreMatchParticipantsPlayersAccess._players[action.ActorSeat].IsAlive)
            return;
        if (use.PijingNextUses is null)
        {
            var grants = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<PijingGrantIssuedEvent>().Where(g => g.HolderSeat == action.ActorSeat && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<PijingGrantConsumedEvent>().Any(c => c.GrantId == g.GrantId)).OrderBy(g => g.GrantId).GroupBy(g => (g.Source.OwnerSeat, g.Source.SkillId));
            var groups = new List<PijingNextUseGroup>();
            foreach (var pending in grants)
            {
                var first = pending.First();
                var ids = pending.Select(g => g.GrantId).ToArray();
                var group = new PijingNextUseGroup(first.GrantId, first.Source, first.GameplayHash, action.ActorSeat, ids);
                groups.Add(group);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PijingNextUseIssuedEvent(use.Id, action.ActionId, group.GroupId, group.Source, group.GameplayHash, group.HolderSeat, ids.Length));
                for (var i = 0; i < ids.Length; i++)
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PijingNextUseGrantEvent(use.Id, group.GroupId, i, ids[i]));
            }

            if (groups.Count == 0)
                return;
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { PijingNextUses = groups });
            use = _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)!;
        }

        foreach (var g in use.PijingNextUses!)
        {
            if (result.Any(c => c.SkillId == g.Source.SkillId && c.TriggerId == "next-sole-use" && c.OwnerSeat == g.Source.OwnerSeat))
                continue;
            var trigger = _contentRegistryOperations._contentRegistry.GetSkill(g.Source.SkillId).Program!.Triggers.Single(t => t.Id == "next-sole-use");
            var context = _coreCardActionsOperations.CreateCardActionProgramContext(action, window, 0, g.Source.OwnerSeat, g.HolderSeat, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts2(_coreMatchParticipantsPlayersAccess._players[g.Source.OwnerSeat], action))with
            {
                PijingIssuedGroupId = g.GroupId,
                OptionalChooserSeat = g.HolderSeat
            };
            result.Add(new(g.Source.OwnerSeat, g.HolderSeat, g.Source.SkillId, trigger.Id, g.GameplayHash, g.Source.SkillInstanceId, trigger.Priority, context));
        }
    }

    internal ProgramSkillWindowContext CreatePijingIssuedContext(ProgramCardTriggerWindowFrame frame, ProgramCardTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (candidate.SkillId != "ol:pijing" || candidate.TriggerId != "next-sole-use" || _coreCardUseLifecycleOperations.LifecycleCardUse(frame.ParentFrameId)?.PijingNextUses?.SingleOrDefault(g => g.Source.OwnerSeat == candidate.OwnerSeat && g.Source.SkillInstanceId == candidate.SkillInstanceId)is not { } group)
            return context;
        return context with
        {
            PijingIssuedGroupId = group.GroupId,
            OptionalChooserSeat = group.HolderSeat,
            SourceSeat = group.HolderSeat,
            TargetSeat = group.HolderSeat
        };
    }

    internal PijingNextUseGroup? PijingIssuedGroup(ProgramTriggerCandidate c, ProgramSkillWindowContext context, out CardUseFrame use, out ProgramCardTriggerWindowFrame window)
    {
        use = null !;
        window = null !;
        if (c.SkillId != "ol:pijing" || c.BindingId != "next-sole-use" || context.PijingIssuedGroupId is not { } id || !TryGetPijingUse(context, out use, out window) || !_coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidateMatches(c, context, window) || use.PijingNextUses?.SingleOrDefault(g => g.GroupId == id)is not { } group || group.Source.OwnerSeat != c.OwnerSeat || group.Source.SkillId != c.SkillId || group.Source.SkillInstanceId != c.SkillInstanceId || group.GameplayHash != c.GameplayHash || context.OptionalChooserSeat != group.HolderSeat || _contentRegistryOperations._contentRegistry.GetSkill(c.SkillId).Program?.GameplayHash != c.GameplayHash)
            return null;
        var currentUse = use;
        if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<PijingNextUseIssuedEvent>().Count(e => e == new PijingNextUseIssuedEvent(currentUse.Id, currentUse.Action!.ActionId, group.GroupId, group.Source, group.GameplayHash, group.HolderSeat, group.Grants.Count)) != 1 || !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<PijingNextUseGrantEvent>().Where(e => e.UseId == currentUse.Id && e.GroupId == id).OrderBy(e => e.Index).Select(e => e.GrantId).SequenceEqual(group.Grants) || group.Grants.Any(g => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<PijingGrantIssuedEvent>().SingleOrDefault(e => e.GrantId == g)is not { } issued || issued.HolderSeat != group.HolderSeat || issued.Source.OwnerSeat != group.Source.OwnerSeat || issued.Source.SkillId != group.Source.SkillId))
            return null;
        return group;
    }

    [ProgramIssuedTrigger(500)]
    internal bool IsIssuedPijingCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => PijingIssuedGroup(c, context, out var use, out _)is { } g && _coreMatchParticipantsPlayersAccess._players[g.HolderSeat].IsAlive && use.SourceSeat == g.HolderSeat && g.Grants.All(id => !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<PijingGrantConsumedEvent>().Any(e => e.GrantId == id));
    internal bool HasPijingTargetTail(CardUseFrame use) => use.PijingTail is not null;
    internal bool HasPijingZeroTargetTail(CardUseFrame use) => use.PijingTail is not null && use.TargetSeats.Count == 0;

    [ProgramTriggerProof(ProgramTriggerProofPhase.DesignatedTarget, 10)]
    internal bool RejectPijingDesignatedTarget(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context, SkillProgramTrigger trigger) => context.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized && trigger.OwnerRelation == SkillProgramCardActionOwnerRelation.Target && context.CardUse is { } designatedUse && _coreCardUseLifecycleOperations.LifecycleCardUse(designatedUse.ParentCardUseFrameId)is { Action: { } designatedAction } currentUse && designatedAction.ActionId == designatedUse.CardActionId && HasPijingTargetTail(currentUse) && !(trigger.OnlyDesignatedCardTargets ? designatedAction.EffectiveDesignatedTargetSeats : currentUse.TargetSeats).Contains(candidate.OwnerSeat);
}
