#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeCardUseFramesRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLiuPanOlEntries _characterLiuPanOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterMaChengOlEntries _characterMaChengOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterMaWanOlEntries _characterMaWanOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterMaXiuMaTieOlEntries _characterMaXiuMaTieOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterQianZhaoOlEntries _characterQianZhaoOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterYangFuOlEntries _characterYangFuOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangYanOlEntries _characterZhangYanOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentEnhancementSlashRedirectsEntries _coreCurrentEnhancementSlashRedirectsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    internal CoreNativeCardUseFramesRuntime(global::CardGame.Core.RuntimeEntries.CharacterLiuPanOlEntries characterLiuPanOlOperations, global::CardGame.Core.RuntimeEntries.CharacterMaChengOlEntries characterMaChengOlOperations, global::CardGame.Core.RuntimeEntries.CharacterMaWanOlEntries characterMaWanOlOperations, global::CardGame.Core.RuntimeEntries.CharacterMaXiuMaTieOlEntries characterMaXiuMaTieOlOperations, global::CardGame.Core.RuntimeEntries.CharacterQianZhaoOlEntries characterQianZhaoOlOperations, global::CardGame.Core.RuntimeEntries.CharacterYangFuOlEntries characterYangFuOlOperations, global::CardGame.Core.RuntimeEntries.CharacterZhangYanOlEntries characterZhangYanOlOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentEnhancementSlashRedirectsEntries coreCurrentEnhancementSlashRedirectsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations)
    {
        this._characterLiuPanOlOperations = characterLiuPanOlOperations;
        this._characterMaChengOlOperations = characterMaChengOlOperations;
        this._characterMaWanOlOperations = characterMaWanOlOperations;
        this._characterMaXiuMaTieOlOperations = characterMaXiuMaTieOlOperations;
        this._characterQianZhaoOlOperations = characterQianZhaoOlOperations;
        this._characterYangFuOlOperations = characterYangFuOlOperations;
        this._characterZhangYanOlOperations = characterZhangYanOlOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCurrentEnhancementSlashRedirectsOperations = coreCurrentEnhancementSlashRedirectsOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
    }
}
