namespace CardGame.Core;

internal interface IHeQiSelectionQueries : IProgramSelectionQuery
{
    int EquipmentSlotCapacity(int seat, EquipmentSlot slot);
    int EquipmentCountInSlot(int seat, EquipmentSlot slot);
}

internal static class ShanxiRuntime
{
    internal static bool IsActivation(SkillProgramActivation activation) =>
        ProgramInstructionResolver.Default.Features(activation).HasOperation(SkillProgramEffectOp.RevealJointHandsAndDiscardDodges);

    internal static int EmptySlots(int seat, IProgramSelectionQueries queries)
    {
        var equipment = queries.Get<IHeQiSelectionQueries>();
        return Enum.GetValues<EquipmentSlot>().Sum(slot =>
            Math.Max(0, equipment.EquipmentSlotCapacity(seat, slot) - equipment.EquipmentCountInSlot(seat, slot)));
    }

    internal static bool AllowsActivation(int ownerSeat, SkillProgramActivation activation, IProgramSelectionQueries queries) =>
        !IsActivation(activation) || EmptySlots(ownerSeat, queries) > 0;

    internal static bool AllowsActivationTarget(int ownerSeat, int targetSeat, SkillProgramActivation activation, IProgramSelectionQueries queries) =>
        !IsActivation(activation) || AllowsTarget(ownerSeat, targetSeat, queries);

    internal static bool AllowsTarget(int ownerSeat, int targetSeat, IProgramSelectionQueries queries)
    {
        if (targetSeat == ownerSeat || !queries.Get<ISelectionParticipantQueries>().IsAlive(targetSeat)) return false;
        return !queries.Get<IAttackRangeQueries>().IsWithinAttackRange(targetSeat, ownerSeat);
    }
}

[ProgramSelectionModule(ProgramSelectionStage.AfterTargets, 13)]
internal sealed class HeQiSelectionFilter : IProgramSelectionCandidateFilter
{
    public bool Apply(ProgramSelectionContext context, ProgramSelectionCandidates candidates, IProgramSelectionQueries queries)
    {
        candidates.Targets = candidates.Targets.Where(seat => ShanxiRuntime.AllowsActivationTarget(context.OwnerSeat, seat, context.Activation, queries)).ToArray();
        return true;
    }
}
