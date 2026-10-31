#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed partial class ProgramSkillHost : IResumableProgramDrawHost
{
    public SkillProgramStepOutcome DrawInstruction(ProgramSkillFrame frame, DrawProgramInstruction instruction, int targetSeat, CardMoveReason reason) => getCoreGameEngineProgramNativeDrawRuntime().ExecuteProgramDrawInstruction(getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(frame.Id), instruction, targetSeat, reason);
}
