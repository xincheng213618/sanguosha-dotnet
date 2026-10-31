#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime))]
internal sealed class CharacterQuHuangOlCoreEquipmentContentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterQuHuangOlCoreEquipmentContentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get => this._targetFactory().EquipmentRules; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.QuHuangPhaseEndLedger))]
internal sealed class CharacterQuHuangOlQuHuangPhaseEndLedgerAdapter(global::System.Func<global::CardGame.Core.QuHuangPhaseEndLedger> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterQuHuangOlQuHuangPhaseEndLedgerOperations
{
    private readonly global::System.Func<global::CardGame.Core.QuHuangPhaseEndLedger> _targetFactory = targetFactory;
    public global::CardGame.Core.QuHuangPhaseEndLedger Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.QuHuangRuntime))]
internal sealed class CharacterQuHuangOlQuHuangAdapter(global::System.Func<global::CardGame.Core.QuHuangRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterQuHuangOlQuHuangOperations
{
    private readonly global::System.Func<global::CardGame.Core.QuHuangRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.QuHuangRuntime Get() => this._targetFactory();
}
