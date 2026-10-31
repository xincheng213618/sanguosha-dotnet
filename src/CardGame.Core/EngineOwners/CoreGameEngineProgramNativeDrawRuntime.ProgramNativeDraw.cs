#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramNativeDrawRuntime
{
    internal NativeDrawProducerRuntime NativeDrawProducer => _nativeDrawProducerOperations.Get();

    internal SkillProgramStepOutcome ExecuteProgramDrawInstruction(ProgramSkillFrame frame, DrawProgramInstruction instruction, int targetSeat, CardMoveReason reason) => NativeDrawProducer.ExecuteProgramDrawInstruction(frame, instruction, targetSeat, reason);
    internal SkillProgramStepOutcome BeginOperationProgramDraw(ProgramSkillFrame frame, int recipientSeat, int requestedCount, CardMoveReason reason) => _closedProgramOperationDrawsSource.Get().BeginOperationProgramDraw(frame, recipientSeat, requestedCount, reason);
    internal bool ResumeProgramNativeDraw(long id) => NativeDrawProducer.ResumeProgramNativeDraw(id);
    internal bool ContinueProgramNativeDraw(long id) => NativeDrawProducer.ContinueProgramNativeDraw(id);
}
