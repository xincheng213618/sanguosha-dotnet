#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime))]
internal sealed class CharacterHuanShuJunOlCoreEquipmentContentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterHuanShuJunOlCoreEquipmentContentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get => this._targetFactory().EquipmentRules; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreCommandSessionQueriesRuntime))]
internal sealed class CharacterHuanShuJunOlCoreCommandSessionQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreCommandSessionQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterHuanShuJunOlCoreCommandSessionQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreCommandSessionQueriesRuntime> _targetFactory = targetFactory;
    public long Revision { get => this._targetFactory().Revision; }
}
