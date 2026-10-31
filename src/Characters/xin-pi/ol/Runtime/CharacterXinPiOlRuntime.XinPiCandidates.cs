#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXinPiOlRuntime
{
    internal bool ExactYinjuBeforeCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context, out YinjuTurnRelation relation, out BeforeDamageProgramWindowFrame window)
    {
        relation = null !;
        window = null !;
        if (context.Window != SkillProgramTriggerWindow.BeforeDamageApplied || CurrentYinju(c.OwnerSeat)is not { } r || r.Source.SkillId != c.SkillId || r.Source.SkillInstanceId != c.SkillInstanceId || r.GameplayHash != c.GameplayHash || context.SourceSeat != c.OwnerSeat || context.TargetSeat != r.TargetSeat || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId)is not BeforeDamageProgramWindowFrame w || w.Prevented || w.SourceSeat != c.OwnerSeat || w.TargetSeat != r.TargetSeat || w.Amount <= 0 || context.Amount != w.Amount || _coreDamagePreventionProgramsOperations.BeforeDamageHasNoSource(w) || w.CandidateIndex < 0 || w.CandidateIndex >= w.Candidates.Count)
            return false;
        var current = w.Candidates[w.CandidateIndex];
        if (current.Candidate != c || _coreDamagePreventionProgramsOperations.CreateBeforeDamageProgramContext(w, current) != context || _coreProgramLifecycleOperations.GetProgramTrigger2(c).Effects is not [{ Op: SkillProgramEffectOp.PreventYinjuDamageAndRecover }])
            return false;
        relation = r;
        window = w;
        return true;
    }

    internal bool ExactYinjuUseCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context, out YinjuTurnRelation relation, out CardUseFrame use)
    {
        relation = null !;
        use = null !;
        if (CurrentYinju(c.OwnerSeat)is not { } r || r.Source.SkillId != c.SkillId || r.Source.SkillInstanceId != c.SkillInstanceId || r.GameplayHash != c.GameplayHash || ExactXinPiNativeWindow(context, false)is not { Action: { } a } u || a.ActorSeat != c.OwnerSeat || !XinPiMainTargets(u).Contains(r.TargetSeat) || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId)is not ProgramCardTriggerWindowFrame window || !_coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidateMatches(c, context, window) || _coreProgramLifecycleOperations.GetProgramTrigger2(c).Effects is not [{ Op: SkillProgramEffectOp.DrawBothAfterYinjuActualTargetUse }] || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<YinjuUseDrawStartedEvent>().Any(e => e.Relation.ActivationFrameId == r.ActivationFrameId && e.UseId == u.Id))
            return false;
        relation = r;
        use = u;
        return true;
    }

    [ProgramTriggerProof(ProgramTriggerProofPhase.InstanceException, 170)]
    internal bool HasIssuedYinjuCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => ExactYinjuBeforeCandidate(c, w, out _, out _) || ExactYinjuUseCandidate(c, w, out _, out _);
    internal IEnumerable<BeforeDamageProgramCandidate> CollectIssuedYinjuCandidates(int sourceSeat, int targetSeat, bool rangeChainOnly)
    {
        if (!HasXinPiCapability || _coreProgramAttackAttemptOperations.CurrentDamageAttempt?.IsSourceLess == true || CurrentYinju(sourceSeat)is not { } r || r.TargetSeat != targetSeat)
            yield break;
        var p = _contentRegistryOperations._contentRegistry.GetSkill(r.Source.SkillId).Program!;
        foreach (var t in p.Triggers.Where(t => t.Window == SkillProgramTriggerWindow.BeforeDamageApplied && t.Effects is [{ Op: SkillProgramEffectOp.PreventYinjuDamageAndRecover }]))
        {
            var c = new ProgramTriggerCandidate(sourceSeat, r.Source.SkillId, t.Id, r.Source.SkillInstanceId, r.GameplayHash, t.Priority);
            yield return new(c, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[sourceSeat])with { EventTargetHp = _coreMatchParticipantsPlayersAccess._players[targetSeat].Hp });
        }
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CollectIssuedCardTrigger, 17)]
    internal void CollectIssuedYinjuCandidates(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result)
    {
        if (window != SkillProgramTriggerWindow.CardUseTargetsFinalized || action.Type != CardActionType.Use || CurrentYinju(action.ActorSeat)is not { } r || _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault(u => u.Action?.ActionId == action.ActionId)is not { } use || !XinPiMainTargets(use).Contains(r.TargetSeat) || !_coreCompletedCategoryUseProofOperations.HasExactAcceptedActualHandGainUse(use, action))
            return;
        var p = _contentRegistryOperations._contentRegistry.GetSkill(r.Source.SkillId).Program!;
        foreach (var t in p.Triggers.Where(t => t.Window == window && t.Effects is [{ Op: SkillProgramEffectOp.DrawBothAfterYinjuActualTargetUse }]))
        {
            result.RemoveAll(c => c.OwnerSeat == action.ActorSeat && c.SkillId == r.Source.SkillId && c.TriggerId == t.Id && c.SkillInstanceId != r.Source.SkillInstanceId);
            if (result.Any(c => c.OwnerSeat == action.ActorSeat && c.SkillId == r.Source.SkillId && c.TriggerId == t.Id && c.SkillInstanceId == r.Source.SkillInstanceId))
                continue;
            result.Add(new(action.ActorSeat, r.TargetSeat, r.Source.SkillId, t.Id, r.GameplayHash, r.Source.SkillInstanceId, t.Priority, _coreCardActionsOperations.CreateCardActionProgramContext(action, window, 0, action.ActorSeat, r.TargetSeat, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts2(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat], action))));
        }
    }
}
