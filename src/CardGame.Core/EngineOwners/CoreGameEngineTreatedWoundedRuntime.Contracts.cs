#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreTreatedWoundedCharacterDaQiaoXiaoQiaoHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.HappyQiaoPool? CurrentHappyQiaoPool(int seat, long? parentFrameId = default, bool healthWrite = false);
}

internal interface ICoreTreatedWoundedContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreTreatedWoundedCharacterSunHaoOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsGuimingTreatedWounded(global::CardGame.Core.CharacterState player);
}
