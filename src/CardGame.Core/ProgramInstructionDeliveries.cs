namespace CardGame.Core;

/// <summary>A delivered instruction may finish its own return without authorizing a following step.</summary>
public sealed record ProgramInstructionDelivery(int InstructionIndex, SkillProgramEffectOp Operation);

internal static class ProgramInstructionDeliveries
{
    internal static bool HasSettledOwnedRequest(ProgramSkillFrame frame) =>
        frame.SelectedCardPayment is { Operation: SkillProgramEffectOp.SelectAndMoveOwnedCard,
            MovementCommitted: true, GainRequestSettlement: not null } payment &&
        payment.InstructionIndex == frame.InstructionIndex;

    internal static bool HasPendingOwnedRequest(ProgramSkillFrame frame) =>
        HasSettledOwnedRequest(frame) && frame.SelectedCardPaymentResult is null;

    internal static bool HasDeliveredMovement(ProgramSkillFrame frame) =>
        frame.InstructionDelivery is { } delivered && delivered.InstructionIndex == frame.InstructionIndex &&
        (delivered.Operation == SkillProgramEffectOp.GiveBoundCard ||
         delivered.Operation == SkillProgramEffectOp.SelectAndMoveOwnedCard && HasSettledOwnedRequest(frame));

    internal static bool CanFinish(ProgramSkillFrame frame, SkillProgram program)
    {
        if (frame.InstructionDelivery is not { } delivered || delivered.InstructionIndex != frame.InstructionIndex ||
            delivered.Operation is not (SkillProgramEffectOp.Draw or SkillProgramEffectOp.GiveBoundCard or SkillProgramEffectOp.SelectAndMoveOwnedCard) ||
            delivered.Operation == SkillProgramEffectOp.SelectAndMoveOwnedCard && !HasSettledOwnedRequest(frame))
            return false;
        var plan = ProgramInstructionResolver.Default.Resolve(frame, program);
        return delivered.InstructionIndex > 0 && delivered.InstructionIndex == plan.Instructions.Count &&
            plan.Instructions[delivered.InstructionIndex - 1].Op == delivered.Operation;
    }
}
