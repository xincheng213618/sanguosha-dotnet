#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime))]
internal sealed class CoreFieldEquipmentCoreEquipmentContentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreFieldEquipmentCoreEquipmentContentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get => this._targetFactory().EquipmentRules; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWeiYanHappyGodRuntime))]
internal sealed class CoreFieldEquipmentCharacterWeiYanHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWeiYanHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreFieldEquipmentCharacterWeiYanHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWeiYanHappyGodRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.EquipmentSlot? HappyBoneSlot(global::CardGame.Core.Card card) => this._targetFactory().HappyBoneSlot(card);
    public bool HasHappyWeiYanEzhiCapability { get => this._targetFactory().HasHappyWeiYanEzhiCapability; }

    public bool IsHappyWeiYanBone(int id) => this._targetFactory().IsHappyWeiYanBone(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreFieldEquipmentCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreFieldEquipmentCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(int seat) => this._targetFactory().GetEquipment(seat);
}
