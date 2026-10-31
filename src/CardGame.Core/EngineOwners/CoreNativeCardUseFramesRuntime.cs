#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeCardUseFramesRuntime
{
    private readonly ICoreNativeCardUseFramesCharacterLiuPanOlOperations _characterLiuPanOlOperations;
    private readonly ICoreNativeCardUseFramesCharacterMaChengOlOperations _characterMaChengOlOperations;
    private readonly ICoreNativeCardUseFramesCharacterMaWanOlOperations _characterMaWanOlOperations;
    private readonly ICoreNativeCardUseFramesCharacterMaXiuMaTieOlOperations _characterMaXiuMaTieOlOperations;
    private readonly ICoreNativeCardUseFramesCharacterQianZhaoOlOperations _characterQianZhaoOlOperations;
    private readonly ICoreNativeCardUseFramesCharacterYangFuOlOperations _characterYangFuOlOperations;
    private readonly ICoreNativeCardUseFramesCharacterZhangYanOlOperations _characterZhangYanOlOperations;
    private readonly ICoreNativeCardUseFramesResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreNativeCardUseFramesCoreCurrentEnhancementSlashRedirectsOperations _coreCurrentEnhancementSlashRedirectsOperations;
    private readonly ICoreNativeCardUseFramesCoreRuntimeOperations _coreRuntimeOperations;
    internal CoreNativeCardUseFramesRuntime(ICoreNativeCardUseFramesCharacterLiuPanOlOperations characterLiuPanOlOperations, ICoreNativeCardUseFramesCharacterMaChengOlOperations characterMaChengOlOperations, ICoreNativeCardUseFramesCharacterMaWanOlOperations characterMaWanOlOperations, ICoreNativeCardUseFramesCharacterMaXiuMaTieOlOperations characterMaXiuMaTieOlOperations, ICoreNativeCardUseFramesCharacterQianZhaoOlOperations characterQianZhaoOlOperations, ICoreNativeCardUseFramesCharacterYangFuOlOperations characterYangFuOlOperations, ICoreNativeCardUseFramesCharacterZhangYanOlOperations characterZhangYanOlOperations, ICoreNativeCardUseFramesResolutionStackOperations resolutionStackOperations, ICoreNativeCardUseFramesCoreCurrentEnhancementSlashRedirectsOperations coreCurrentEnhancementSlashRedirectsOperations, ICoreNativeCardUseFramesCoreRuntimeOperations coreRuntimeOperations)
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
