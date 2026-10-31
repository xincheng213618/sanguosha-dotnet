#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineNamedUseConversionsRuntime
{
    private readonly ICoreNamedUseConversionsSkillRuntimeStateOperations _skillRuntimeStateOperations;
    private readonly ICoreNamedUseConversionsCoreDyingCursorRuntimeOperations _coreDyingCursorRuntimeOperations;
    private readonly ICoreNamedUseConversionsCorePlayCardHistoryOperations _corePlayCardHistoryOperations;
    private readonly ICoreNamedUseConversionsCoreProgramPersistentStateOperations _coreProgramPersistentStateOperations;
    private readonly ICoreNamedUseConversionsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineProgramPersistentStateProgramBooleanStatesAccess _coreGameEngineProgramPersistentStateProgramBooleanStatesAccess;
    internal CoreGameEngineNamedUseConversionsRuntime(ICoreNamedUseConversionsSkillRuntimeStateOperations skillRuntimeStateOperations, ICoreNamedUseConversionsCoreDyingCursorRuntimeOperations coreDyingCursorRuntimeOperations, ICoreNamedUseConversionsCorePlayCardHistoryOperations corePlayCardHistoryOperations, ICoreNamedUseConversionsCoreProgramPersistentStateOperations coreProgramPersistentStateOperations, ICoreNamedUseConversionsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineProgramPersistentStateProgramBooleanStatesAccess coreGameEngineProgramPersistentStateProgramBooleanStatesAccess)
    {
        this._skillRuntimeStateOperations = skillRuntimeStateOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._corePlayCardHistoryOperations = corePlayCardHistoryOperations;
        this._coreProgramPersistentStateOperations = coreProgramPersistentStateOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineProgramPersistentStateProgramBooleanStatesAccess = coreGameEngineProgramPersistentStateProgramBooleanStatesAccess;
    }
}
