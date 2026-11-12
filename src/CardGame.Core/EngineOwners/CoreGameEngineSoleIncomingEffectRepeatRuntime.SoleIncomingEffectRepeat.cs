#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSoleIncomingEffectRepeatRuntime
{
    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedBeforeActivity, 50)]
    internal bool SoleIncomingEffectFinalizedBeforeActivityTargetWindow(ProgramTargetCardWindowQuery query) => HasSoleIncomingEffectCapability && CoreGameEngineExtraEffectsAndRankGuessingRuntime.ExtraEffectCardKind(query.Kind);
    internal bool HasSoleIncomingEffectCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.RepeatIncomingSoleCardEffectAndDrawByRange);

    internal bool TryGetSoleIncomingEffect(int owner, ProgramSkillWindowContext? context, out CardUseFrame use, out ProgramCardTriggerWindowFrame window)
    {
        use = null !;
        window = null !;
        if (context is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } id } || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId)is not ProgramCardTriggerWindowFrame w || w.ParentFrameId != id.ParentCardUseFrameId || w.CompletedResponseReturn is not null || w.Continuation is not (ProgramCardContinuation.Slash or ProgramCardContinuation.FinalizedTrick or ProgramCardContinuation.FinalizedSimpleCard) || _coreCardUseLifecycleOperations.LifecycleCardUse(w.ParentFrameId)is not { Action: { Type: CardActionType.Use } a } current || a.ActorSeat == owner || a.ActionId != id.CardActionId || a.ActorSeat != current.SourceSeat || a.EffectiveKind != current.CardKind || w.Action.ActionId != a.ActionId || !w.Action.TargetSeats.SequenceEqual(current.TargetSeats) || !CoreGameEngineExtraEffectsAndRankGuessingRuntime.ExtraEffectCardKind(current.CardKind) || current.CardKind is CardKind.Dodge or CardKind.Nullification || !_coreMatchParticipantsPlayersAccess._players[owner].IsAlive)
            return false;
        var actual = a.EffectiveDesignatedTargetSeats.Distinct().ToArray();
        if (actual is not [var target] || target != owner || !current.TargetSeats.Contains(owner) || _effectEpisodes.Ordinal(current.Id) != 0)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == w.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1].Id != current.Id)
            return false;
        use = current;
        window = w;
        return true;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 1243, SkillProgramEffectOp.RepeatIncomingSoleCardEffectAndDrawByRange)]
    internal bool CanOfferSoleIncomingEffect(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => TryGetSoleIncomingEffect(candidate.OwnerSeat, context, out var use, out var window) && _coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidateMatches(candidate, context, window) && use.SoleIncomingEffects?.Grants.Any(g => g.Source.OwnerSeat == candidate.OwnerSeat && g.Source.SkillId == candidate.SkillId && g.Source.SkillInstanceId == candidate.SkillInstanceId) != true;
    internal void IssueSoleIncomingEffect(ProgramSkillFrame supplied, int cap)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { } context || !TryGetSoleIncomingEffect(f.OwnerSeat, context, out var use, out var window) || !CanOfferSoleIncomingEffect(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, 0), _coreProgramLifecycleOperations.GetProgramTrigger(f), context))
            throw new InvalidOperationException("An incoming repeat needs its exact sole target and original finalized native use.");
        var source = new CardConversionSource(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
        var episode = _effectEpisodes.Issue(use.Id,
            new(f.Id, f.InstructionIndex - 1, 0, source, f.GameplayHash, new(50)),
            window.TrickContinuation, window.SimpleContinuation);
        var grant = new SoleIncomingEffectGrant(f.Id, source, f.GameplayHash, window.Id, use.Id,
            use.Action!.ActionId, use.Action.ActorSeat, f.OwnerSeat, use.CardKind, cap, episode.Id);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with
        {
            SoleIncomingEffects = new(grant.ActionId)
            { Grants = (current.SoleIncomingEffects?.Grants ?? []).Append(grant).ToArray() }
        });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SoleIncomingEffectIssuedEvent(grant));
    }

    void ISoleIncomingEffectRepeatHost.IssueSoleIncomingEffect(ProgramSkillFrame frame, int maximumDrawCount) => IssueSoleIncomingEffect(frame, maximumDrawCount);
}
