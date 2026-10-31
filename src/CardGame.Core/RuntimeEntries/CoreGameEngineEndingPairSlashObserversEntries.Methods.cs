#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineEndingPairSlashObserversEntries
{
    internal bool EndingPairPaidFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().EndingPairPaidFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? EndingPairPaidObserverRoot() => _resolve().EndingPairPaidObserverRoot();
    internal bool IsEndingPairMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsEndingPairMovement(f, e, pending);
    internal bool IsEndingPairSlashProgramDying() => _resolve().IsEndingPairSlashProgramDying();
    internal bool ReturnEndingPairMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnEndingPairMovement(f);
}
