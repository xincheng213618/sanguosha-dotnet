#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramNativeDrawEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginOperationProgramDraw(global::CardGame.Core.ProgramSkillFrame frame, int recipientSeat, int requestedCount, global::CardGame.Core.CardMoveReason reason) => _resolve().BeginOperationProgramDraw(frame, recipientSeat, requestedCount, reason);
    internal bool ContinueProgramNativeDraw(long id) => _resolve().ContinueProgramNativeDraw(id);
    internal global::CardGame.Core.SkillProgramStepOutcome ExecuteProgramDrawInstruction(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.DrawProgramInstruction instruction, int targetSeat, global::CardGame.Core.CardMoveReason reason) => _resolve().ExecuteProgramDrawInstruction(frame, instruction, targetSeat, reason);
    internal bool ResumeProgramNativeDraw(long id) => _resolve().ResumeProgramNativeDraw(id);
    internal global::CardGame.Core.NativeDrawProducerRuntime NativeDrawProducer { get => _resolve().NativeDrawProducer; }
}
