#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhuRongMouMobileRuntime
{
    internal MouZhuRongLierenWonEvent? MouZhuRongWin(int owner, ProgramSkillWindowContext c) => c.CardUse is { } a ? _coreProgramEventHistoryOperations.ProgramEventHistory<MouZhuRongLierenWonEvent>().SingleOrDefault(e => e.Source.OwnerSeat == owner && e.UseId == a.ParentCardUseFrameId && e.ActionId == a.CardActionId) : null;
    internal void RecordMouZhuRongLieren(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != MouZhuRongComposition.Lieren || f.TriggerId != "unique-slash-contest" || f.InstructionIndex != 2 || f.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } a } c || f.PindianResultBindings.SingleOrDefault(b => b.Name == "contest")is not { } result || result.SourceSeat != f.OwnerSeat || result.OpponentSeat != c.TargetSeat)
            throw new InvalidOperationException("Lieren lost its original single-target Slash Pindian return.");
        if (result.SourceWon)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhuRongLierenWonEvent(f.Id, MouZhuRongSource(f), f.GameplayHash, a.ParentCardUseFrameId, a.CardActionId, result.OpponentSeat));
    }

    internal IReadOnlyList<int> MouZhuRongLierenOtherTargets(int owner)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.OwnerSeat != owner || f.SkillId != MouZhuRongComposition.Lieren || f.TriggerId != "won-slash-damage" || f.InstructionIndex != 1 || f.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseCompleted } c || MouZhuRongWin(owner, c)is not { } won || won.Source.SkillInstanceId != f.SkillInstanceId || won.GameplayHash != f.GameplayHash)
            return[];
        return Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != owner && p.Seat != won.OpponentSeat).Select(p => p.Seat).ToArray());
    }

    internal SkillProgramStepOutcome SelectMouZhuRongLierenTarget(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != MouZhuRongComposition.Lieren || f.TriggerId != "won-slash-damage" || f.InstructionIndex != 2 || f.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseCompleted } c || MouZhuRongWin(f.OwnerSeat, c)is not { } won || won.Source.SkillInstanceId != f.SkillInstanceId || won.GameplayHash != f.GameplayHash || f.SelectedTargetSeats is not [var target] || target == f.OwnerSeat || target == won.OpponentSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || _coreProgramEventHistoryOperations.ProgramEventHistory<MouZhuRongLierenDamageChosenEvent>().Any(e => e.UseId == won.UseId && e.OwnerSeat == f.OwnerSeat))
            throw new InvalidOperationException("Lieren requires one native-selected other target for its original won Slash.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhuRongLierenDamageChosenEvent(f.Id, won.UseId, won.ActionId, f.OwnerSeat, target));
        return SkillProgramStepOutcome.Continue;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CollectIssuedCardTrigger, 25)]
    internal void CollectIssuedMouZhuRongLieren(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result)
    {
        if (window != SkillProgramTriggerWindow.CardUseCompleted || action.Type != CardActionType.Use || !CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind))
            return;
        var trigger = _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(MouZhuRongComposition.Lieren)?.Program?.Triggers.SingleOrDefault(t => t.Id == "won-slash-damage");
        if (trigger is null)
            return;
        foreach (var won in _coreProgramEventHistoryOperations.ProgramEventHistory<MouZhuRongLierenWonEvent>().Where(e => e.ActionId == action.ActionId && _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(use => use.Action?.ActionId == action.ActionId)?.Id == e.UseId && _coreMatchParticipantsPlayersAccess._players[e.Source.OwnerSeat].IsAlive && !_coreProgramEventHistoryOperations.ProgramEventHistory<MouZhuRongLierenDamageChosenEvent>().Any(d => d.UseId == e.UseId && d.OwnerSeat == e.Source.OwnerSeat)))
        {
            if (!_coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != won.Source.OwnerSeat && p.Seat != won.OpponentSeat))
                continue;
            var context = _coreCardActionsOperations.CreateCardActionProgramContext(action, window, 0, won.Source.OwnerSeat, won.OpponentSeat, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts2(_coreMatchParticipantsPlayersAccess._players[won.Source.OwnerSeat], action));
            result.Add(new(won.Source.OwnerSeat, won.OpponentSeat, MouZhuRongComposition.Lieren, trigger.Id, won.GameplayHash, won.Source.SkillInstanceId, trigger.Priority, context));
        }
    }

    [ProgramIssuedTrigger(320)]
    internal bool IsIssuedMouZhuRongLieren(ProgramTriggerCandidate candidate, ProgramSkillWindowContext c)
    {
        if (candidate.SkillId != MouZhuRongComposition.Lieren || candidate.BindingId != "won-slash-damage" || c.Window != SkillProgramTriggerWindow.CardUseCompleted || !_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None || _contentRegistryOperations._contentRegistry.GetSkill(candidate.SkillId).Program?.GameplayHash != candidate.GameplayHash || MouZhuRongWin(candidate.OwnerSeat, c)is not { } won || won.Source.SkillInstanceId != candidate.SkillInstanceId || won.GameplayHash != candidate.GameplayHash || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId)is not { } parent || parent.ParentFrameId != won.UseId || parent.Action.ActionId != won.ActionId || CoreGameEngineCardActionsRuntime.GetCardActionWindow(parent) != c.Window || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || parent.Candidates[parent.CandidateIndex] is not { } pending || pending.OwnerSeat != candidate.OwnerSeat || pending.SkillInstanceId != candidate.SkillInstanceId || pending.GameplayHash != candidate.GameplayHash || pending.TriggerId != candidate.BindingId)
            return false;
        return !_coreProgramEventHistoryOperations.ProgramEventHistory<MouZhuRongLierenDamageChosenEvent>().Any(e => e.UseId == won.UseId && e.OwnerSeat == candidate.OwnerSeat) && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != candidate.OwnerSeat && p.Seat != won.OpponentSeat);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 30400)]
    internal bool ContinueMouZhuRongPaidLieren(ProgramSkillFrame f) => f.SkillId == MouZhuRongComposition.Lieren && f.WindowContext is { CardUse: { } use } c && (f.TriggerId == "unique-slash-contest" && f.PindianResultBindings.Any(r => r.Name == "contest" && r.SourceSeat == f.OwnerSeat && r.OpponentSeat == c.TargetSeat) || f.TriggerId == "won-slash-damage" && MouZhuRongWin(f.OwnerSeat, c)is { } won && won.Source.SkillInstanceId == f.SkillInstanceId && won.GameplayHash == f.GameplayHash);
}
