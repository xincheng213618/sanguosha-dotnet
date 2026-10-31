#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunShangxiangSpOlRuntime
{
    internal bool IsSpSunShangxiangFanxiangTrigger(SkillProgramTrigger t) => t.Effects.Any(e => e.Op == SkillProgramEffectOp.CompleteSpSunShangxiangFanxiang);
    internal IEnumerable<int> SpSunShangxiangHelpedSeats(int owner) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<SpSunShangxiangHelpedEvent>().Where(e => e.OwnerSeat == owner).Select(e => e.HelpedSeat).Distinct();
    internal int? SpSunShangxiangFanxiangWitness(int owner) => SpSunShangxiangHelpedSeats(owner).Where(s => _coreMatchParticipantsPlayersAccess._players[s].IsAlive && _coreMatchParticipantsPlayersAccess._players[s].HasHpValue && _coreMatchParticipantsPlayersAccess._players[s].Hp < _coreMatchParticipantsPlayersAccess._players[s].MaxHp).OrderBy(s => _coreTableOrderOperations.RelativeTableOrder(owner, s)).Select(s => (int? )s).FirstOrDefault();
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.Collection, 32301)]
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 32301)]
    internal bool CanOfferSpSunShangxiangFanxiang(int ownerSeat, SkillProgramTrigger t) => !IsSpSunShangxiangFanxiangTrigger(t) || !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<SpSunShangxiangFanxiangAcceptedEvent>().Any(e => e.Receipt.Source.OwnerSeat == ownerSeat) && (SpSunShangxiangFanxiangWitness(ownerSeat)is not null || _characterGuoJiaMobileGodOperations.HasShenGuoJiaAwakeningCondition(ownerSeat, SpSunShangxiangOlComposition.Fanxiang, t.Id));
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 32301)]
    internal void AcceptSpSunShangxiangFanxiang(ProgramBindingStartedEvent started)
    {
        if (started.SkillId != SpSunShangxiangOlComposition.Fanxiang)
            return;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(started.FrameId);
        if (f.SpSunShangxiangFanxiang is not null || f.InstructionIndex != 0 || f.ActivationId != f.TriggerId || f.WindowContext is not { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow } context || context.SourceSeat != f.OwnerSeat || (SpSunShangxiangFanxiangWitness(f.OwnerSeat) ?? (_characterGuoJiaMobileGodOperations.HasShenGuoJiaAwakeningCondition(f.OwnerSeat, f.SkillId, f.TriggerId) ? -1 : (int? )null))is not { } seat || !CanOfferSpSunShangxiangFanxiang(f.OwnerSeat, _coreProgramLifecycleOperations.GetProgramTrigger(f)))
            throw new InvalidOperationException("Fanxiang requires an unused actual Preparation with a living wounded assisted role.");
        var r = new SpSunShangxiangFanxiangReceipt(SpSunShangxiangSource(f), f.GameplayHash, context.ParentFrameId, seat, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { SpSunShangxiangFanxiang = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SpSunShangxiangFanxiangAcceptedEvent(f.Id, r));
    }

    internal bool ExactSpSunShangxiangFanxiangParent(ProgramSkillFrame f, long id)
    {
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        return n > 0 && _resolutionStackOperations._resolutionStack[n - 1] is ProgramLifecycleTriggerWindowFrame parent && parent.Id == id && parent.OwnerSeat == f.OwnerSeat && parent.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, parent.Candidates[parent.CandidateIndex]);
    }

    internal bool ValidSpSunShangxiangFanxiang(ProgramSkillFrame f) => f.SpSunShangxiangFanxiang is { } r && r.Source == SpSunShangxiangSource(f) && r.GameplayHash == f.GameplayHash && f.SkillId == SpSunShangxiangOlComposition.Fanxiang && f.TriggerId == "assisted-wounded-awakening" && f.ActivationId == f.TriggerId && f.WindowContext is { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow } w && w.ParentFrameId == r.LifecycleId && w.SourceSeat == f.OwnerSeat && _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash == f.GameplayHash && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<SpSunShangxiangFanxiangAcceptedEvent>().Count(e => e == new SpSunShangxiangFanxiangAcceptedEvent(f.Id, r)) == 1 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<SpSunShangxiangFanxiangAcceptedEvent>().Count(e => e.Receipt.Source.OwnerSeat == f.OwnerSeat) == 1 && (SpSunShangxiangHelpedSeats(f.OwnerSeat).Contains(r.QualifiedSeat) || r.QualifiedSeat == -1 && _characterGuoJiaMobileGodOperations.HasShenGuoJiaAwakeningReceipt(f)) && r.MaximumBefore > 0 && ExactSpSunShangxiangFanxiangParent(f, r.LifecycleId);
    internal SkillProgramStepOutcome CompleteSpSunShangxiangFanxiang(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!ValidSpSunShangxiangFanxiang(f) || f.InstructionIndex != 3 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<SpSunShangxiangFanxiangCompletedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Fanxiang can replace skills only after its native maximum/recovery instructions have returned.");
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        foreach (var grant in owner.SkillGrants.Grants.Where(g => g.IsEnabled && g.SkillId == SpSunShangxiangOlComposition.Liangzhu).ToArray())
            owner.SkillGrants.RemoveGrant(grant.GrantId);
        _coreProgramSkillChangesOperations.GrantProgramSkills(f, [SpSunShangxiangOlComposition.Xiaoji]);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramOwnerSkillsReplacedEvent(f.Id, f.SkillId, f.OwnerSeat, [SpSunShangxiangOlComposition.Liangzhu], SpSunShangxiangOlComposition.Xiaoji));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SpSunShangxiangFanxiangCompletedEvent(f.Id, f.OwnerSeat, owner.MaxHp));
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { SpSunShangxiangFanxiang = null });
        return SkillProgramStepOutcome.Continue;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 32301)]
    internal bool ContinueAcceptedSpSunShangxiangFanxiang(ProgramSkillFrame f) => _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && ValidSpSunShangxiangFanxiang(f);
}
