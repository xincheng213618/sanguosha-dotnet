#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunShangxiangSpOlRuntime
{
    internal bool HasSpSunShangxiangLiangzhu => _contentRegistryOperations._contentRegistry.Skills.ContainsKey(SpSunShangxiangOlComposition.Liangzhu);

    internal static CardConversionSource SpSunShangxiangSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal static bool IsSpSunShangxiangLiangzhuTrigger(SkillProgramTrigger t) => t.Id == "actual-own-play-recovery" && t.Window == SkillProgramTriggerWindow.AfterHpRecovered && t.Effects.FirstOrDefault() is { Op: SkillProgramEffectOp.ChooseOption, ResultBind: SpSunShangxiangOlComposition.LiangzhuChoice };
    SkillProgramStepOutcome ISpSunShangxiangOlHost.CompleteFanxiang(ProgramSkillFrame f) => CompleteSpSunShangxiangFanxiang(f);
    internal void CaptureSpSunShangxiangRecovery(HpChangeContext change)
    {
        if (!HasSpSunShangxiangLiangzhu || change.Kind != HpChangeKind.Recovery || change.Amount <= 0 || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != change.TargetSeat || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase))
            return;
        foreach (var owner in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive))
            foreach (var c in _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(owner, SkillProgramTriggerWindow.AfterHpRecovered).Where(c => IsSpSunShangxiangLiangzhuTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(c))))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SpSunShangxiangRecoveryQualifiedEvent(new(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId), c.GameplayHash, change.Id, change.ParentFrameId, change.TargetSeat, change.HpBefore, change.HpAfter, phase));
    }

    internal IReadOnlyList<ProgramTriggerCandidate> CollectSpSunShangxiangRecoveryCandidates(HpChangeContext change) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<SpSunShangxiangRecoveryQualifiedEvent>().Where(q => q.ChangeId == change.Id && _coreMatchParticipantsPlayersAccess._players[q.Source.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[q.Source.OwnerSeat], q.Source.SkillId, q.Source.SkillInstanceId) && _contentRegistryOperations._contentRegistry.GetSkill(q.Source.SkillId).Program?.GameplayHash == q.GameplayHash).Select(q => new ProgramTriggerCandidate(q.Source.OwnerSeat, q.Source.SkillId, q.Source.BindingId, q.Source.SkillInstanceId, q.GameplayHash, _contentRegistryOperations._contentRegistry.GetSkill(q.Source.SkillId).Program!.Triggers.Single(t => t.Id == q.Source.BindingId).Priority)).ToArray();
    internal SpSunShangxiangRecoveryQualifiedEvent? SpSunShangxiangRecoveryQualification(ProgramTriggerCandidate c, HpChangeContext change) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<SpSunShangxiangRecoveryQualifiedEvent>().SingleOrDefault(q => q.Source == new CardConversionSource(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId) && q.GameplayHash == c.GameplayHash && q.ChangeId == change.Id && q.RecoveryParentId == change.ParentFrameId && q.RecoveredSeat == change.TargetSeat && q.HpBefore == change.HpBefore && q.HpAfter == change.HpAfter && change.Kind == HpChangeKind.Recovery && change.Amount == change.HpAfter - change.HpBefore && q.Phase.Kind == ActualDiscardRecoveryPhaseKind.Play && q.Phase.ActorSeat == change.TargetSeat && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(q.Phase));
    internal ProgramSkillWindowContext CreateSpSunShangxiangRecoveryContext(HpChangeContext change, ProgramTriggerCandidate c)
    {
        if (SpSunShangxiangRecoveryQualification(c, change)is null)
            throw new InvalidOperationException("Liangzhu requires its frozen actual own-Play recovery.");
        return new(SkillProgramTriggerWindow.AfterHpRecovered, change.Id, c.OwnerSeat, SourceSeat: change.SourceSeat, TargetSeat: change.TargetSeat, Amount: change.Amount, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat]), HpChange: change);
    }

    internal bool CanRunSpSunShangxiangLiangzhu(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => context.Window == SkillProgramTriggerWindow.AfterHpRecovered && context.OwnerSeat == c.OwnerSeat && context.HpChange is { Kind: HpChangeKind.Recovery, Amount: > 0 } change && context.ParentFrameId == change.Id && context.TargetSeat == change.TargetSeat && context.SourceSeat == change.SourceSeat && context.Amount == change.Amount && c.OccurrenceIndex == 0 && SpSunShangxiangRecoveryQualification(c, change)is not null;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 32300)]
    internal bool CanOfferSpSunShangxiangLiangzhu(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => !IsSpSunShangxiangLiangzhuTrigger(t) || CanRunSpSunShangxiangLiangzhu(c, context);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 32300)]
    internal void RecordSpSunShangxiangHelped(ProgramDrawCompletedEvent draw)
    {
        if (draw.RequestedCount != 2 || _resolutionStackOperations._resolutionStack.LastOrDefault() is not ProgramSkillFrame f ||
            f.Id != draw.FrameId || f.SkillId != SpSunShangxiangOlComposition.Liangzhu || f.TriggerId != "actual-own-play-recovery" ||
            !f.ChoiceBindings.Any(c => c.Name == SpSunShangxiangOlComposition.LiangzhuChoice && c.OptionId == "help") ||
            f.WindowContext?.HpChange is not { } change || draw.RecipientSeat != change.TargetSeat)
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SpSunShangxiangHelpedEvent(f.Id, f.OwnerSeat, change.TargetSeat, change.Id));
    }
    [ProgramAiChoice(SkillProgramEffectOp.ChooseOption, SkillId = SpSunShangxiangOlComposition.Liangzhu)]
    internal PromptChoice AiSpSunShangxiangLiangzhu(PendingDecision p, ProgramSkillFrame f) => p.Choices.FirstOrDefault(c =>
        c.Parameters.GetValueOrDefault("option-id") == "help" && CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(
            _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[f.WindowContext!.TargetSeat!.Value])) ??
        p.Choices.Single(c => c.Parameters.GetValueOrDefault("option-id") == "self");
    [ProgramTriggerProof(ProgramTriggerProofPhase.HpChangeOwner, 90, ProgramTriggerProofMatch.Decision)]
    internal bool? ProveSpLiangzhuHealth(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsSpSunShangxiangLiangzhuTrigger(trigger) ? CanRunSpSunShangxiangLiangzhu(candidate, context) : null;
}
