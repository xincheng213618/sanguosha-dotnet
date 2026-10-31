namespace CardGame.Core;

[ProgramRuntimeModule, ProgramCapability]
internal sealed class QuHuangPhaseEndLedger(IProgramContentQueries content,
    IMatchParticipantStateAccess participants, IProgramEventHistoryQueries history) : IQuHuangPhaseEndQueries
{
    public bool HasCapability => content.HasTriggerOperation(SkillProgramEffectOp.ResolveQiejianLastHandLoss) ||
        content.HasTriggerOperation(SkillProgramEffectOp.ResolveNishouEquipmentLossChoice);

    public IReadOnlyList<ActualPhaseEndItem> CollectNishouPhaseEndCandidates(ActualDiscardRecoveryPhaseKey phase)
    {
        if (!HasCapability) return [];
        return Array.AsReadOnly(history.Read<NishouSwapDebtIssuedEvent>()
            .Where(e => e.Phase == phase && participants.At(e.Source.OwnerSeat).IsAlive &&
                !history.Read<NishouSwapDebtConsumedEvent>().Any(d => d.DebtId == e.DebtId))
            .OrderBy(e => e.DebtId).Select(e =>
            {
                var trigger = content.Skill(e.Source.SkillId).Program!.Triggers.Single(t => t.Id == "issued-current-phase-swap");
                return new ActualPhaseEndItem(new(e.Source.OwnerSeat, e.Source.SkillId, trigger.Id,
                    e.Source.SkillInstanceId, e.GameplayHash, trigger.Priority), e.DebtId);
            }).ToArray());
    }
}
