#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.SkillOwnershipWindowRuntime))]
internal sealed class CoreSkillOwnershipWindowsSkillOwnershipWindowAdapter(global::System.Func<global::CardGame.Core.SkillOwnershipWindowRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillOwnershipWindowsSkillOwnershipWindowOperations
{
    private readonly global::System.Func<global::CardGame.Core.SkillOwnershipWindowRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillOwnershipWindowRuntime Get() => this._targetFactory();
}
