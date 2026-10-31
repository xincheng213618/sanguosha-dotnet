#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramSelectionModulesRuntime))]
internal sealed class CharacterBuZhiOlCoreProgramSelectionModulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramSelectionModulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterBuZhiOlCoreProgramSelectionModulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramSelectionModulesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.IProgramSelectionQueries ProgramSelectionQueries { get => this._targetFactory().ProgramSelectionQueries; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime))]
internal sealed class CharacterBuZhiOlCorePlayerViewProjectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterBuZhiOlCorePlayerViewProjectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}
