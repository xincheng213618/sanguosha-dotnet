namespace CardGame.Core;
internal sealed partial class QuHuangRuntime
{
    private bool NishouSwapAvailable(int owner,ActualDiscardRecoveryPhaseKey? phase)=>phase is{}p&&phases.IsRecorded(p)&&p==phases.Current()&&!ProgramEventHistory<ActualPhaseEndedEvent>().Any(e=>e.Phase==p)&&!ProgramEventHistory<NishouSwapDebtIssuedEvent>().Any(e=>e.Source.OwnerSeat==owner&&e.Phase.TurnNumber==p.TurnNumber);
    [ProgramIssuedTrigger(490)]
    private bool IsIssuedNishouCandidate(ProgramTriggerCandidate c,ProgramSkillWindowContext context)
    {
        if(c.SkillId!="ol:nishou"||c.BindingId!="issued-current-phase-swap"||!_players[c.OwnerSeat].IsAlive||!phaseEnds.IsExactActualPhaseEndCandidate(c,context)||context.ActualPhaseEnd is not{}due)return false;
        return ProgramEventHistory<NishouSwapDebtIssuedEvent>().Any(e=>e.DebtId==due.EntitlementId&&e.Source.OwnerSeat==c.OwnerSeat&&e.Source.SkillId==c.SkillId&&e.Source.SkillInstanceId==c.SkillInstanceId&&e.GameplayHash==c.GameplayHash&&e.Phase==due.Phase&&!ProgramEventHistory<NishouSwapDebtConsumedEvent>().Any(x=>x.DebtId==e.DebtId));
    }
}
