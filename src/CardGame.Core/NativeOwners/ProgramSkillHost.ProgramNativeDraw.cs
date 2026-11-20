#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed partial class ProgramSkillHost : IResumableProgramDrawHost
{
    public SkillProgramStepOutcome DrawInstruction(ProgramSkillFrame frame, DrawProgramInstruction instruction, int targetSeat, CardMoveReason reason)
    {
        var active = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(frame.Id);
        return getNativeDrawProducerRuntime().ExecuteProgramDrawInstruction(active, instruction, targetSeat, reason);
    }
}
