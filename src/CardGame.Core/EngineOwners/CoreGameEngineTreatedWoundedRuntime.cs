#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineTreatedWoundedRuntime
{
    private readonly ICoreTreatedWoundedCharacterDaQiaoXiaoQiaoHappyGodOperations _characterDaQiaoXiaoQiaoHappyGodOperations;
    private readonly ICoreTreatedWoundedCharacterSunHaoOlOperations _characterSunHaoOlOperations;
    private readonly ICoreTreatedWoundedContentRegistryOperations _contentRegistryOperations;
    internal CoreGameEngineTreatedWoundedRuntime(ICoreTreatedWoundedCharacterDaQiaoXiaoQiaoHappyGodOperations characterDaQiaoXiaoQiaoHappyGodOperations, ICoreTreatedWoundedCharacterSunHaoOlOperations characterSunHaoOlOperations, ICoreTreatedWoundedContentRegistryOperations contentRegistryOperations)
    {
        this._characterDaQiaoXiaoQiaoHappyGodOperations = characterDaQiaoXiaoQiaoHappyGodOperations;
        this._characterSunHaoOlOperations = characterSunHaoOlOperations;
        this._contentRegistryOperations = contentRegistryOperations;
    }
}
