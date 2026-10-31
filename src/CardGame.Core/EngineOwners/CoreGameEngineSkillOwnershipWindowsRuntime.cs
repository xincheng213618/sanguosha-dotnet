#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineSkillOwnershipWindowsRuntime
{
    private readonly ICoreSkillOwnershipWindowsSkillOwnershipWindowOperations _skillOwnershipWindowOperations;
    internal CoreGameEngineSkillOwnershipWindowsRuntime(ICoreSkillOwnershipWindowsSkillOwnershipWindowOperations skillOwnershipWindowOperations)
    {
        this._skillOwnershipWindowOperations = skillOwnershipWindowOperations;
    }
}
