#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMaWanOlRuntime
{
    internal void IssueMaWanHunjiangUse(long id, CardActionContext? action)
    {
        if (!HasMaWanCapability || action is not { Type: CardActionType.Use } || !CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) || action.ActorSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play)
            return;
        var grants = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MaWanHunjiangEntitledEvent>().Where(e => e.Source.OwnerSeat == action.ActorSeat && e.ActualTurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.PhaseInstanceId == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId).ToArray();
        if (grants.Length == 0)
            return;
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, use => use with { MaWanHunjiang = new(id, action.ActionId, action.ActorSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId) { Grants = grants, OriginalTargets = use.TargetSeats } });
        foreach (var grant in grants)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MaWanHunjiangUseIssuedEvent(id, action.ActionId, grant.FrameId, grant.Source, grant.TargetSeat, grant.ActualTurnNumber, grant.PhaseInstanceId));
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CollectIssuedCardTrigger, 9)]
    internal void CollectIssuedMaWanHunjiangCandidates(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result)
    {
        if (window != SkillProgramTriggerWindow.CardUseTargetsFinalized || _resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame { MaWanHunjiang: { Offered: false } receipt } use || use.Action?.ActionId != action.ActionId || action.ActorSeat != receipt.ActorSeat || !_coreMatchParticipantsPlayersAccess._players[action.ActorSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return;
        foreach (var grant in receipt.Grants.DistinctBy(g => (g.Source.SkillId, g.Source.SkillInstanceId)))
        {
            if (_contentRegistryOperations._contentRegistry.GetSkill(grant.Source.SkillId).Program is not { } program || program.GameplayHash != grant.GameplayHash)
                continue;
            var trigger = program.Triggers.Single(t => t.Effects.Any(e => e.Op == SkillProgramEffectOp.OfferHunjiangPhaseSlashTargets));
            if (result.Any(c => c.OwnerSeat == action.ActorSeat && c.SkillId == grant.Source.SkillId && c.SkillInstanceId == grant.Source.SkillInstanceId && c.TriggerId == trigger.Id))
                continue;
            var context = _coreCardActionsOperations.CreateCardActionProgramContext(action, window, 0, action.ActorSeat, action.ActorSeat, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts2(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat], action));
            result.Add(new(action.ActorSeat, action.ActorSeat, grant.Source.SkillId, trigger.Id, program.GameplayHash, grant.Source.SkillInstanceId, trigger.Priority, context));
        }
    }

    [ProgramTriggerProof(ProgramTriggerProofPhase.InstanceException, 110)]
    internal bool HasIssuedMaWanHunjiangCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (candidate.SkillId != "ol:hunjiang" || context is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } card } || card.ActorSeat != candidate.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } window || window.ParentFrameId != card.ParentCardUseFrameId || window.Action.ActionId != card.CardActionId || _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(u => u.Id == card.ParentCardUseFrameId)is not { MaWanHunjiang: { } receipt, Action: { } action } use || action.Type != CardActionType.Use || !CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) || action.ActionId != card.CardActionId || action.ActorSeat != candidate.OwnerSeat || use.SourceSeat != action.ActorSeat || receipt.ActorSeat != action.ActorSeat || receipt.UseId != use.Id || receipt.ActionId != action.ActionId || receipt.ActualTurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || receipt.PhaseInstanceId != _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count)
            return false;
        var current = window.Candidates[window.CandidateIndex];
        if (current.OwnerSeat != candidate.OwnerSeat || current.SkillId != candidate.SkillId || current.TriggerId != candidate.BindingId || current.SkillInstanceId != candidate.SkillInstanceId || current.GameplayHash != candidate.GameplayHash || _coreCardActionsOperations.CreateCardActionProgramContext2(window, current) != context)
            return false;
        return _contentRegistryOperations._contentRegistry.GetSkill(candidate.SkillId).Program is { } p && p.GameplayHash == candidate.GameplayHash && p.Triggers.Any(t => t.Id == candidate.BindingId && t.Effects.Any(e => e.Op == SkillProgramEffectOp.OfferHunjiangPhaseSlashTargets)) && receipt.Grants.Any(g => g.Source.OwnerSeat == candidate.OwnerSeat && g.Source.SkillId == candidate.SkillId && g.Source.SkillInstanceId == candidate.SkillInstanceId && g.GameplayHash == candidate.GameplayHash && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MaWanHunjiangUseIssuedEvent>().Any(e => e.UseId == use.Id && e.ActionId == action.ActionId && e.GrantFrameId == g.FrameId && e.Source == g.Source && e.TargetSeat == g.TargetSeat && e.ActualTurnNumber == g.ActualTurnNumber && e.PhaseInstanceId == g.PhaseInstanceId));
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 26510)]
    internal bool CanOfferMaWan(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => !trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.OfferHunjiangPhaseSlashTargets) || HasIssuedMaWanHunjiangCandidate(candidate, context) && _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(u => u.Id == context.CardUse!.ParentCardUseFrameId).MaWanHunjiang is { Offered: false };
    internal CardUseFrame RequireMaWanUse(ProgramSkillFrame f)
    {
        if (f.WindowContext is not { } context || !HasIssuedMaWanHunjiangCandidate(new(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0), context))
            throw new InvalidOperationException("Hunjiang lost its exact issued native finalized-use candidate.");
        return _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(u => u.Id == context.CardUse!.ParentCardUseFrameId);
    }

    internal int[] MaWanExtraTargets(ProgramSkillFrame f, CardUseFrame use) => use.MaWanHunjiang!.Grants.Where(g => g.Source.SkillId == f.SkillId && g.Source.SkillInstanceId == f.SkillInstanceId && g.GameplayHash == f.GameplayHash && !use.TargetSeats.Contains(g.TargetSeat) && !f.MaWanHunjiang!.SelectedTargets.Contains(g.TargetSeat) && _coreProgramCardUseRolesOperations.CanBeProgramCardUseRoleTarget(use, _coreMatchParticipantsPlayersAccess._players[g.TargetSeat])).Select(g => g.TargetSeat).Distinct().Order().ToArray();
    internal SkillProgramStepOutcome BeginMaWanTargets(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var use = RequireMaWanUse(f);
        var receipt = use.MaWanHunjiang!;
        if (f.MaWanHunjiang is not null || receipt.Offered || f.InstructionIndex != 1)
            throw new InvalidOperationException("Hunjiang can offer its issued target extension only once.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MaWanHunjiang = new(MaWanSource(f), f.GameplayHash, f.InstructionIndex, receipt.ActualTurnNumber, receipt.PhaseInstanceId, MaWanHunjiangStage.AppendTargets, CardUseId: use.Id, ActionId: use.Action!.ActionId) });
        if (MaWanExtraTargets(f, use).Length == 0)
        {
            CompleteMaWanTargets(f);
            return SkillProgramStepOutcome.AwaitChoice;
        }

        PublishMaWan(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void CompleteMaWanTargets(ProgramSkillFrame f)
    {
        var r = f.MaWanHunjiang!;
        var use = RequireMaWanUse(f);
        var issued = use.MaWanHunjiang!;
        if (r.SelectedTargets.Any(s => use.TargetSeats.Contains(s) || !_coreProgramCardUseRolesOperations.CanBeProgramCardUseRoleTarget(use, _coreMatchParticipantsPlayersAccess._players[s])))
            throw new InvalidOperationException("Hunjiang's extra target no longer satisfies the current native Slash target rules.");
        var targets = Array.AsReadOnly(use.TargetSeats.Concat(r.SelectedTargets).ToArray());
        var action = _coreProgramCardUseRolesOperations.CloneRoleAction(use.Action!, use.SourceSeat, targets);
        _coreProgramCardUseRolesOperations.UpdateProgramRoleCardUse(use with { TargetSeats = targets, Action = action, MaWanHunjiang = issued with { Offered = true, OriginalTargets = use.TargetSeats, AddedTargets = r.SelectedTargets }, Enhancements = r.SelectedTargets.Count > 0 ? use.Enhancements | CurrentCardEnhancement.ExtraTarget : use.Enhancements, EnhancementOwnerSeat = r.SelectedTargets.Count > 0 ? f.OwnerSeat : use.EnhancementOwnerSeat }, action);
        for (var i = 0; i < r.SelectedTargets.Count; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MaWanHunjiangTargetAddedEvent(f.Id, use.Id, action.ActionId, r.Source, r.SelectedTargets[i], i));
        FinishMaWan(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }


}
