#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineTreatedWoundedRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries _characterDaQiaoXiaoQiaoHappyGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterSunHaoOlEntries _characterSunHaoOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    internal CoreGameEngineTreatedWoundedRuntime(global::CardGame.Core.RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries characterDaQiaoXiaoQiaoHappyGodOperations, global::CardGame.Core.RuntimeEntries.CharacterSunHaoOlEntries characterSunHaoOlOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations)
    {
        this._characterDaQiaoXiaoQiaoHappyGodOperations = characterDaQiaoXiaoQiaoHappyGodOperations;
        this._characterSunHaoOlOperations = characterSunHaoOlOperations;
        this._contentRegistryOperations = contentRegistryOperations;
    }
}
