#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramRecoveryReceiptsRuntime
{
    internal void RecordProgramRecoveryRequest(ResolutionFrame? parent, RecoveryAttempt attempt) => _coreTypedRecoveryDamageCursorOperations.HealthContinuations.RecordProgramRecoveryRequest(parent, attempt);
    internal bool ProgramRecoveryReplacementReceiptMatches(ProgramSkillFrame owner, RecoveryReplacementFrame recovery) => _coreTypedRecoveryDamageCursorOperations.HealthContinuations.ProgramRecoveryReplacementReceiptMatches(owner, recovery);
    internal void RecordProgramRecoveryApplication(RecoveryFrame recovery, HpChangeContext change) => _coreTypedRecoveryDamageCursorOperations.HealthContinuations.RecordProgramRecoveryApplication(recovery, change);
    internal bool ProgramRecoveryReceiptMatches(ProgramSkillFrame owner, HpChangedTriggerWindowFrame hp) => _coreTypedRecoveryDamageCursorOperations.HealthContinuations.ProgramRecoveryReceiptMatches(owner, hp);
    internal void ClearProgramRecoveryApplication(HpChangeContext change) => _coreTypedRecoveryDamageCursorOperations.HealthContinuations.ClearProgramRecoveryApplication(change);
}
