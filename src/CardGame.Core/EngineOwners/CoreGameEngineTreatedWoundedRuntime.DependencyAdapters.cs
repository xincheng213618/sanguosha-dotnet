#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime))]
internal sealed class CoreTreatedWoundedCharacterDaQiaoXiaoQiaoHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTreatedWoundedCharacterDaQiaoXiaoQiaoHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.HappyQiaoPool? CurrentHappyQiaoPool(int seat, long? parentFrameId = default, bool healthWrite = false) => this._targetFactory().CurrentHappyQiaoPool(seat, parentFrameId, healthWrite);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunHaoOlRuntime))]
internal sealed class CoreTreatedWoundedCharacterSunHaoOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunHaoOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTreatedWoundedCharacterSunHaoOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunHaoOlRuntime> _targetFactory = targetFactory;
    public bool IsGuimingTreatedWounded(global::CardGame.Core.CharacterState player) => this._targetFactory().IsGuimingTreatedWounded(player);
}
