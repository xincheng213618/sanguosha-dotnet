#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramNativeDrawEntries
{
    internal SkillProgramStepOutcome BeginOperationProgramDraw(ProgramSkillFrame frame, int recipientSeat, int requestedCount, CardMoveReason reason) => _resolve().BeginOperationProgramDraw(frame, recipientSeat, requestedCount, reason);
    internal bool ContinueProgramNativeDraw(long id) => _resolve().ContinueProgramNativeDraw(id);
    internal SkillProgramStepOutcome ExecuteProgramDrawInstruction(ProgramSkillFrame frame, DrawProgramInstruction instruction, int targetSeat, CardMoveReason reason) => _resolve().ExecuteProgramDrawInstruction(frame, instruction, targetSeat, reason);
    internal bool ResumeProgramNativeDraw(long id) => _resolve().ResumeProgramNativeDraw(id);
    internal NativeDrawProducerRuntime NativeDrawProducer { get => _resolve().NativeDrawProducer; }
}
