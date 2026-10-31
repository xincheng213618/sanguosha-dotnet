#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramRecoveryReceiptsEntries
{
    internal void ClearProgramRecoveryApplication(HpChangeContext change) => _resolve().ClearProgramRecoveryApplication(change);
    internal bool ProgramRecoveryReceiptMatches(ProgramSkillFrame owner, HpChangedTriggerWindowFrame hp, IReadOnlyList<IGameEvent> history) => _resolve().ProgramRecoveryReceiptMatches(owner, hp, history);
    internal bool ProgramRecoveryReplacementReceiptMatches(ProgramSkillFrame owner, RecoveryReplacementFrame recovery, IReadOnlyList<IGameEvent> history) => _resolve().ProgramRecoveryReplacementReceiptMatches(owner, recovery, history);
    internal void RecordProgramRecoveryApplication(RecoveryFrame recovery, HpChangeContext change) => _resolve().RecordProgramRecoveryApplication(recovery, change);
    internal void RecordProgramRecoveryRequest(ResolutionFrame? parent, RecoveryAttempt attempt) => _resolve().RecordProgramRecoveryRequest(parent, attempt);
}
