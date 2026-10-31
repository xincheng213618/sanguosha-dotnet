#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CharacterLiuShanBoundaryMobileRuntime
{
    private readonly ICharacterLiuShanBoundaryMobileCoreLordSkillProjectionsOperations _coreLordSkillProjectionsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberTurnProgressionAccess _coreActualTurnProgressTurnNumberTurnProgressionAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineProgramPersistentStateProgramBooleanStatesAccess _coreGameEngineProgramPersistentStateProgramBooleanStatesAccess;
    internal CharacterLiuShanBoundaryMobileRuntime(ICharacterLiuShanBoundaryMobileCoreLordSkillProjectionsOperations coreLordSkillProjectionsOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberTurnProgressionAccess coreActualTurnProgressTurnNumberTurnProgressionAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineProgramPersistentStateProgramBooleanStatesAccess coreGameEngineProgramPersistentStateProgramBooleanStatesAccess)
    {
        this._coreLordSkillProjectionsOperations = coreLordSkillProjectionsOperations;
        this._coreActualTurnProgressTurnNumberTurnProgressionAccess = coreActualTurnProgressTurnNumberTurnProgressionAccess;
        this._coreGameEngineProgramPersistentStateProgramBooleanStatesAccess = coreGameEngineProgramPersistentStateProgramBooleanStatesAccess;
    }
}
