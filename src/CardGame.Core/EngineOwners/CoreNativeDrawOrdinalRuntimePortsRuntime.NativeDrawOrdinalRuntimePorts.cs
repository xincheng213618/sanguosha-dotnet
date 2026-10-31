#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeDrawOrdinalRuntimePortsRuntime
{
    bool global::CardGame.Core.INativeDrawOrdinalSkillInstances.HasRuntimeSkillInstance(CharacterState owner, string skillId, string skillInstanceId) => _coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, skillId, skillInstanceId);
}
