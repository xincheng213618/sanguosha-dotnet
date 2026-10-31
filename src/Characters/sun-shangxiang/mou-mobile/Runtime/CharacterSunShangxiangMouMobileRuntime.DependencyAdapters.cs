#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.MouSunShangxiangRuntime))]
internal sealed class CharacterSunShangxiangMouMobileMouSunShangxiangAdapter(global::System.Func<global::CardGame.Core.MouSunShangxiangRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterSunShangxiangMouMobileMouSunShangxiangOperations
{
    private readonly global::System.Func<global::CardGame.Core.MouSunShangxiangRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.MouSunShangxiangRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreWushuangModeProjectionRuntime))]
internal sealed class CharacterSunShangxiangMouMobileCoreWushuangModeProjectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreWushuangModeProjectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterSunShangxiangMouMobileCoreWushuangModeProjectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreWushuangModeProjectionRuntime> _targetFactory = targetFactory;
    public bool IsWushuangMode { get => this._targetFactory().IsWushuangMode; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime))]
internal sealed class CharacterSunShangxiangMouMobileCorePlayerViewProjectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterSunShangxiangMouMobileCorePlayerViewProjectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}
