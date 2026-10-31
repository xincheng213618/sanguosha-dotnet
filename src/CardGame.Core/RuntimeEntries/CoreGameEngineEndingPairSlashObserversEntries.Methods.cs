#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineEndingPairSlashObserversEntries
{
    internal bool EndingPairPaidFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().EndingPairPaidFirstChild(f, child);
    internal ProgramSkillFrame? EndingPairPaidObserverRoot() => _resolve().EndingPairPaidObserverRoot();
    internal bool IsEndingPairMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation pending) => _resolve().IsEndingPairMovement(f, e, pending);
    internal bool IsEndingPairSlashProgramDying() => _resolve().IsEndingPairSlashProgramDying();
    internal bool ReturnEndingPairMovement(ProgramSkillFrame f) => _resolve().ReturnEndingPairMovement(f);
}
