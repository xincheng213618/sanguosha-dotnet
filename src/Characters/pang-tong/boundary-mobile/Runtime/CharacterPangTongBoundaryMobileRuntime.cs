#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CharacterPangTongBoundaryMobileRuntime
{
    private readonly ICharacterPangTongBoundaryMobileSkillRuntimeStateOperations _skillRuntimeStateOperations;
    private readonly ICharacterPangTongBoundaryMobileCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    internal CharacterPangTongBoundaryMobileRuntime(ICharacterPangTongBoundaryMobileSkillRuntimeStateOperations skillRuntimeStateOperations, ICharacterPangTongBoundaryMobileCoreProgramEventHistoryOperations coreProgramEventHistoryOperations)
    {
        this._skillRuntimeStateOperations = skillRuntimeStateOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
    }
}
