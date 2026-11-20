#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramNativeDrawEntries
{
    internal SkillProgramStepOutcome BeginOperationProgramDraw(ProgramSkillFrame frame, int recipientSeat, int requestedCount, CardMoveReason reason) => _resolve().BeginOperationProgramDraw(frame, recipientSeat, requestedCount, reason);
}
