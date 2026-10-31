#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiahouDunMouMobileRuntime
{
    internal int[] MouXiahouDunGanglieTargets(int seat) => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DamageAppliedEvent>().Any(e => !e.SourceLess && e.Amount > 0 && e.TargetSeat == seat && e.SourceSeat == p.Seat) && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouXiahouDunMobileGanglieSpentEvent>().Any(e => e.OwnerSeat == seat && e.TargetSeat == p.Seat)).Select(p => p.Seat).ToArray();
    internal int MouXiahouDunPhaseUses(int seat, ActualDiscardRecoveryPhaseKey phase) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouXiahouDunMobileStartedEvent>().Count(e => e.Source.OwnerSeat == seat && e.Op == SkillProgramEffectOp.ResolveMouXiahouDunGanglie && e.Phase == phase);
    [ProgramPhaseUsedProjection(60)]
    internal int MouXiahouDunProjectedUses(int seat, string skill, int fallback) => skill == MouXiahouDunGanglie && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } phase ? MouXiahouDunPhaseUses(seat, phase) : fallback;
    [ProgramActivationLegality(ProgramActivationLegalityPhase.AfterCommonStart, 31200, SkillProgramEffectOp.ResolveMouXiahouDunGanglie)]
    internal bool CanStartMouXiahouDunGanglie(CharacterState owner) => _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } phase && phase.ActorSeat == owner.Seat && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase) && MouXiahouDunPhaseUses(owner.Seat, phase) == 0 && MouXiahouDunGanglieTargets(owner.Seat).Length > 0;
    internal void IssueMouXiahouDunDamage(ProgramSkillFrame f)
    {
        var r = f.MouXiahouDunMobile!;
        var index = r.Index;
        while (index < r.Targets.Count && !_coreMatchParticipantsPlayersAccess._players[r.Targets[index]].IsAlive)
            index++;
        if (index == r.Targets.Count || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishMouXiahouDun(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouXiahouDunMobile = r with { Index = index } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouXiahouDunMobileDamageIssuedEvent(f.Id, index, f.OwnerSeat, r.Targets[index]));
        _coreSkillProgramsOperations.BeginProgramSkillDamage(f, r.Targets[index], 2, new(ProgramParticipantRef.Owner));
    }
}
