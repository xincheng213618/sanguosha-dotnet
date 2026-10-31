#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramRecoveryReceiptsRuntime
{
    internal void RecordProgramRecoveryRequest(ResolutionFrame? parent, RecoveryAttempt attempt) => _coreTypedRecoveryDamageCursorOperations.HealthContinuations.RecordProgramRecoveryRequest(parent, attempt);
    internal bool ProgramRecoveryReplacementReceiptMatches(ProgramSkillFrame owner, RecoveryReplacementFrame recovery, IReadOnlyList<IGameEvent> history) => _coreTypedRecoveryDamageCursorOperations.HealthContinuations.ProgramRecoveryReplacementReceiptMatches(owner, recovery, history);
    internal void RecordProgramRecoveryApplication(RecoveryFrame recovery, HpChangeContext change) => _coreTypedRecoveryDamageCursorOperations.HealthContinuations.RecordProgramRecoveryApplication(recovery, change);
    internal bool ProgramRecoveryReceiptMatches(ProgramSkillFrame owner, HpChangedTriggerWindowFrame hp, IReadOnlyList<IGameEvent> history) => _coreTypedRecoveryDamageCursorOperations.HealthContinuations.ProgramRecoveryReceiptMatches(owner, hp, history);
    internal void ClearProgramRecoveryApplication(HpChangeContext change) => _coreTypedRecoveryDamageCursorOperations.HealthContinuations.ClearProgramRecoveryApplication(change);
}
