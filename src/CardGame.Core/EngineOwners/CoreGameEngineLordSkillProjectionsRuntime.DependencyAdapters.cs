#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.BoundaryYuanShuRuntime))]
internal sealed class CoreLordSkillProjectionsBoundaryYuanShuAdapter(global::System.Func<global::CardGame.Core.BoundaryYuanShuRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreLordSkillProjectionsBoundaryYuanShuOperations
{
    private readonly global::System.Func<global::CardGame.Core.BoundaryYuanShuRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.BoundaryYuanShuRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineModeAllegianceRuntime))]
internal sealed class CoreLordSkillProjectionsCoreModeAllegianceAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModeAllegianceRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreLordSkillProjectionsCoreModeAllegianceOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModeAllegianceRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.IMatchModeAllegiancePolicy ModeAllegiancePolicy { get => this._targetFactory().ModeAllegiancePolicy; }
}
