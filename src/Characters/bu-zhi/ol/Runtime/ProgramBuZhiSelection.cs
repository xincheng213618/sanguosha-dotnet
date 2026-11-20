namespace CardGame.Core;

internal static class DingpanRuntime
{
    internal static bool IsActivation(SkillProgramActivation activation) =>
        ProgramInstructionResolver.Default.Features(activation).HasOperation(SkillProgramEffectOp.ResolveRebelCountEquipmentChoice);

    internal static bool AllowsTarget(int targetSeat, SkillProgramActivation activation, IProgramSelectionQueries queries) =>
        !IsActivation(activation) || HasEquippedLivingTarget(targetSeat, queries.Get<ISelectionParticipantQueries>());

    internal static bool HasEquippedLivingTarget(int targetSeat, ISelectionParticipantQueries players) =>
        players.IsAlive(targetSeat) && players.EquipmentCount(targetSeat) > 0;
}

[ProgramSelectionModule(ProgramSelectionStage.AfterTargets, 12)]
internal sealed class BuZhiSelectionFilter : IProgramSelectionCandidateFilter
{
    public bool Apply(ProgramSelectionContext context, ProgramSelectionCandidates candidates, IProgramSelectionQueries queries)
    {
        candidates.Targets = candidates.Targets.Where(seat => DingpanRuntime.AllowsTarget(seat, context.Activation, queries)).ToArray();
        return true;
    }
}
