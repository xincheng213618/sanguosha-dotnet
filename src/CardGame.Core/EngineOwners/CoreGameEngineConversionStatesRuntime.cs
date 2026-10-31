#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineConversionStatesRuntime
{
    private readonly ICoreConversionStatesContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreConversionStatesSkillRuntimeStateOperations _skillRuntimeStateOperations;
    private readonly ICoreConversionStatesCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineConversionStatesRuntime(ICoreConversionStatesContentRegistryOperations contentRegistryOperations, ICoreConversionStatesSkillRuntimeStateOperations skillRuntimeStateOperations, ICoreConversionStatesCoreNationalSkillsOperations coreNationalSkillsOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._skillRuntimeStateOperations = skillRuntimeStateOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
