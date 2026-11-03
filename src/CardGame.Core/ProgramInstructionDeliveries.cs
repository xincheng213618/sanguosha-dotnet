namespace CardGame.Core;

/// <summary>A delivered instruction may finish its own return without authorizing a following step.</summary>
public sealed record ProgramInstructionDelivery(int InstructionIndex, SkillProgramEffectOp Operation);

internal static class ProgramInstructionDeliveries
{
    internal static bool HasDeliveredMovement(ProgramSkillFrame frame) =>
        frame.InstructionDelivery is { Operation: SkillProgramEffectOp.GiveBoundCard } delivered &&
        delivered.InstructionIndex == frame.InstructionIndex;

    internal static bool CanFinish(ProgramSkillFrame frame, SkillProgram program)
    {
        if (frame.InstructionDelivery is not { } delivered || delivered.InstructionIndex != frame.InstructionIndex ||
            delivered.Operation is not (SkillProgramEffectOp.Draw or SkillProgramEffectOp.GiveBoundCard))
            return false;
        var plan = ProgramInstructionResolver.Default.Resolve(frame, program);
        return delivered.InstructionIndex > 0 && delivered.InstructionIndex == plan.Instructions.Count &&
            plan.Instructions[delivered.InstructionIndex - 1].Op == delivered.Operation;
    }
}
