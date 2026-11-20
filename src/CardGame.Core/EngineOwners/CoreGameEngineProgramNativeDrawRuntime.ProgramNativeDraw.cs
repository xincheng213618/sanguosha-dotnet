#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramNativeDrawRuntime
{
    internal SkillProgramStepOutcome BeginOperationProgramDraw(ProgramSkillFrame frame, int recipientSeat, int requestedCount, CardMoveReason reason) => _closedProgramOperationDrawsSource.Get().BeginOperationProgramDraw(frame, recipientSeat, requestedCount, reason);
}
