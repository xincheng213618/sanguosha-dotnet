#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramRecoveryReceiptsEntries
{
    internal void ClearProgramRecoveryApplication(global::CardGame.Core.HpChangeContext change) => _resolve().ClearProgramRecoveryApplication(change);
    internal bool ProgramRecoveryReceiptMatches(global::CardGame.Core.ProgramSkillFrame owner, global::CardGame.Core.HpChangedTriggerWindowFrame hp, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IGameEvent> history) => _resolve().ProgramRecoveryReceiptMatches(owner, hp, history);
    internal bool ProgramRecoveryReplacementReceiptMatches(global::CardGame.Core.ProgramSkillFrame owner, global::CardGame.Core.RecoveryReplacementFrame recovery, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IGameEvent> history) => _resolve().ProgramRecoveryReplacementReceiptMatches(owner, recovery, history);
    internal void RecordProgramRecoveryApplication(global::CardGame.Core.RecoveryFrame recovery, global::CardGame.Core.HpChangeContext change) => _resolve().RecordProgramRecoveryApplication(recovery, change);
    internal void RecordProgramRecoveryRequest(global::CardGame.Core.ResolutionFrame? parent, global::CardGame.Core.RecoveryAttempt attempt) => _resolve().RecordProgramRecoveryRequest(parent, attempt);
}
