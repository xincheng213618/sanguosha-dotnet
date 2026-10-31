#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramBooleanStateExpirationCoreProgramPersistentStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramBooleanStateDefinition GetProgramBooleanStateDefinition(global::CardGame.Core.ProgramSkillFrame frame, string stateId);
    void SetProgramBooleanState(int ownerSeat, string skillId, string instanceId, string stateId, bool value);
}

internal interface ICoreProgramBooleanStateExpirationCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ValidateProgramTurnEffectGrant(global::CardGame.Core.ProgramSkillFrame frame);
}
