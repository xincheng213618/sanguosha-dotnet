#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMaChengOlRuntime
{
    internal bool HasMaChengCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveChenglieExtraTargetsAndHiddenCards);

    internal const string ChenglieRevealReason = "skill-program.ma-cheng.chenglie.reveal", ChenglieSwapOutReason = "skill-program.ma-cheng.chenglie.swap-out", ChenglieSwapInReason = "skill-program.ma-cheng.chenglie.swap-in", ChenglieAssignReason = "skill-program.ma-cheng.chenglie.assign", ChenglieGiftReason = "skill-program.ma-cheng.chenglie.give", ChenglieCleanupReason = "skill-program.ma-cheng.chenglie.finished";
    internal static CardConversionSource ChenglieSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_ma_cheng_ol_ChenglieLedger.ChenglieSource(f);
    internal bool TryGetChenglieUse(ProgramSkillWindowContext c, out CardUseFrame use, out ProgramCardTriggerWindowFrame window)
    {
        use = null !;
        window = null !;
        if (c.Window != SkillProgramTriggerWindow.CardUseTargetsFinalized || c.CardUse is not { } identity || _coreCardUseLifecycleOperations.LifecycleCardUse(identity.ParentCardUseFrameId)is not { Action: { Type: CardActionType.Use } a } u || !CoreNativeCardClassificationsRuntime.IsSlashCard(u.CardKind) || a.ActionId != identity.CardActionId || a.ActorSeat != u.SourceSeat || a.EffectiveKind != u.CardKind || a.ResponderSeat is not null || a.OpponentSeat is not null || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId)is not { Continuation: ProgramCardContinuation.Slash, CompletedResponseReturn: null } w || w.ParentFrameId != u.Id || w.Action.ActionId != a.ActionId || !w.Action.TargetSeats.SequenceEqual(u.TargetSeats) || !a.TargetSeats.SequenceEqual(u.TargetSeats) || !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseDeclaredEvent>().Any(e => e.ResolutionId == u.Id && (e.CardKind == u.CardKind || _coreCurrentSlashFireOffersOperations.IsCurrentSlashFireChangedUse(u) && u.CurrentSlashFirePolicy!.OriginalKind == e.CardKind)))
            return false;
        var at = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == w.Id);
        if (at < 1 || _resolutionStackOperations._resolutionStack[at - 1].Id != u.Id)
            return false;
        use = u;
        window = w;
        return true;
    }

    internal IReadOnlyList<int> ChenglieLegalTargets(CardUseFrame use, IReadOnlyList<int> added)
    {
        var a = use.Action!;
        var actor = _coreMatchParticipantsPlayersAccess._players[a.ActorSeat];
        var ids = a.PhysicalCards.Select(c => c.CardId).ToArray();
        return Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(t => !use.TargetSeats.Contains(t.Seat) && !added.Contains(t.Seat) && _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, t, new(-1, use.CardKind, a.EffectiveSuit ?? Suit.None, a.EffectiveRank ?? 0), a.ConversionChain.FirstOrDefault(), use.CardKind, existingUseFrameId: use.Id, specificEffectiveRank: a.EffectiveRank, physicalCardIds: ids)).OrderBy(t => t.Seat).Select(t => t.Seat).ToArray());
    }

    internal bool ChenglieInitialEligible(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => candidate.SkillId == "ol:chenglie" && candidate.BindingId == "extra-targets" && _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive && TryGetChenglieUse(context, out var u, out var w) && u.SourceSeat == candidate.OwnerSeat && u.ChenglieLease is null && _coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidateMatches(candidate, context, w) && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ChenglieStartedEvent>().Any(e => e.UseId == u.Id && !e.Outcome) && ChenglieLegalTargets(u, []).Count > 0;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27560)]
    internal bool QualifyChenglie(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => t.Effects is [{ Op: SkillProgramEffectOp.ResolveChenglieExtraTargetsAndHiddenCards }] ? ChenglieInitialEligible(c, context) : t.Effects is not [{ Op: SkillProgramEffectOp.ResolveChenglieRedHolderOutcome }] || IsIssuedChenglieCandidate(c, context);
    internal CardUseFrame RequireChenglieParent(ProgramSkillFrame f)
    {
        if (f.Chenglie is not { } r || _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is not { Action: { } a } u || a.ActionId != r.ActionId || f.WindowContext is not { } c || c.ParentFrameId != r.WindowId || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == r.WindowId)is not { } window || window.ParentFrameId != u.Id || !_coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidateMatches(new(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0), c, window))
            throw new InvalidOperationException("Chenglie lost its original native use, current candidate or exact target/completion mother.");
        var at = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (at < 2 || _resolutionStackOperations._resolutionStack[at - 1].Id != window.Id || _resolutionStackOperations._resolutionStack[at - 2].Id != u.Id)
            throw new InvalidOperationException("Chenglie requires its immediate original card and card-trigger mother.");
        return u;
    }

    internal bool HasChenglieTargetTail(CardUseFrame use) => use.ChenglieLease is not null;

    internal bool HasChenglieResponseObserver(CardActionContext a, ProgramCardContinuation continuation) => HasMaChengCapability && a.Type == CardActionType.Response && a.EffectiveKind == CardKind.Dodge && continuation is (ProgramCardContinuation.Dodge or ProgramCardContinuation.FactionDefenseDodge) && a.ParentActionId is { } parent && _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Any(u => u.Action?.ActionId == parent && u.ChenglieLease is { Ready: true, Settled: false } r && r.Cards.Any(c => c.HolderSeat == a.ActorSeat));
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 27560)]
    internal void RecordChenglieResponse(IGameEvent fact)
    {
        if (!HasMaChengCapability || fact is not CardResponseCompletedEvent { EffectiveKind: CardKind.Dodge } e || e.OriginalContinuation is not (ProgramCardContinuation.Dodge or ProgramCardContinuation.FactionDefenseDodge) || e.AttackOwnerFrameId is not { } id || _coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { ChenglieLease: { Ready: true, Settled: false } r } use || e.ParentFrameId != id || !HasChenglieTargetTail(use) || !r.Cards.Any(c => c.HolderSeat == e.NativeActorSeat) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ChenglieResponseRecordedEvent>().Any(x => x.SetupFrameId == r.SetupFrameId && x.HolderSeat == e.NativeActorSeat))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChenglieResponseRecordedEvent(r.SetupFrameId, use.Id, r.ActionId, e.NativeActorSeat, e.ActionId, e.ProviderSeat, e.OriginalContinuation));
    }

    internal bool ChenglieResponded(ChenglieLease r, int holder) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ChenglieResponseRecordedEvent>().Any(e => e.SetupFrameId == r.SetupFrameId && e.UseId == r.UseId && e.IncomingActionId == r.ActionId && e.HolderSeat == holder);
    internal CardUseFrame? ChenglieOutcomeUse(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (c.SkillId != "ol:chenglie" || c.BindingId != "red-holder-outcomes" || context.Window != SkillProgramTriggerWindow.CardUseCompleted || context.CardUse is not { } id || _coreCardUseLifecycleOperations.LifecycleCardUse(id.ParentCardUseFrameId)is not { Step: ResolutionFrameStep.Completed, ChenglieLease: { Ready: true, Settled: false } r, Action: { } a } u || r.Source.OwnerSeat != c.OwnerSeat || r.Source.SkillInstanceId != c.SkillInstanceId || r.GameplayHash != c.GameplayHash || r.ActionId != id.CardActionId || a.ActionId != id.CardActionId || !HasChenglieTargetTail(u) || _contentRegistryOperations._contentRegistry.GetSkill(c.SkillId).Program?.GameplayHash != c.GameplayHash || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } window || window.ParentFrameId != u.Id || window.Continuation is not (ProgramCardContinuation.CompletedSlash or ProgramCardContinuation.CompletedCard) || !_coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidateMatches(c, context, window) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == u.Id) != 1)
            return null;
        return u;
    }

    [ProgramTriggerProof(ProgramTriggerProofPhase.OwnerException, 30)]
    [ProgramTriggerProof(ProgramTriggerProofPhase.InstanceException, 220)]
    internal bool IsIssuedChenglieCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => ChenglieOutcomeUse(c, context)is not null;
    [ProgramPipelineHook(ProgramPipelinePhase.CollectIssuedCardTrigger, 7)]
    internal void CollectIssuedChenglieCandidates(CardActionContext a, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result)
    {
        if (!HasMaChengCapability || window != SkillProgramTriggerWindow.CardUseCompleted || _resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame { ChenglieLease: { Ready: true, Settled: false } r } u || a.ActionId != r.ActionId || !HasChenglieTargetTail(u))
            return;
        if (result.Any(c => c.SkillId == r.Source.SkillId && c.TriggerId == "red-holder-outcomes" && c.OwnerSeat == r.Source.OwnerSeat))
            return;
        var t = _contentRegistryOperations._contentRegistry.GetSkill(r.Source.SkillId).Program!.Triggers.Single(t => t.Id == "red-holder-outcomes");
        var context = _coreCardActionsOperations.CreateCardActionProgramContext(a, window, 0, r.Source.OwnerSeat, r.Source.OwnerSeat, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts2(_coreMatchParticipantsPlayersAccess._players[r.Source.OwnerSeat], a));
        result.Add(new(r.Source.OwnerSeat, r.Source.OwnerSeat, r.Source.SkillId, t.Id, r.GameplayHash, r.Source.SkillInstanceId, t.Priority, context));
    }
}
