#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime))]
internal sealed class CharacterCaoCaoOlMagicCoreEquipmentContentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterCaoCaoOlMagicCoreEquipmentContentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get => this._targetFactory().EquipmentRules; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.MoCaoCaoFuzaiRuntime))]
internal sealed class CharacterCaoCaoOlMagicMoCaoCaoFuzaiAdapter(global::System.Func<global::CardGame.Core.MoCaoCaoFuzaiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterCaoCaoOlMagicMoCaoCaoFuzaiOperations
{
    private readonly global::System.Func<global::CardGame.Core.MoCaoCaoFuzaiRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.MoCaoCaoFuzaiRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime))]
internal sealed class CharacterCaoCaoOlMagicCorePlayerViewProjectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterCaoCaoOlMagicCorePlayerViewProjectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.MoCaoCaoBachaoRuntime))]
internal sealed class CharacterCaoCaoOlMagicMoCaoCaoBachaoAdapter(global::System.Func<global::CardGame.Core.MoCaoCaoBachaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterCaoCaoOlMagicMoCaoCaoBachaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.MoCaoCaoBachaoRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.MoCaoCaoBachaoRuntime Get() => this._targetFactory();
}
