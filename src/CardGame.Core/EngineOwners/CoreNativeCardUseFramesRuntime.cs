#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeCardUseFramesRuntime
{
    private readonly RuntimeEntries.CharacterLiuPanOlEntries _characterLiuPanOlOperations;
    private readonly RuntimeEntries.CharacterMaChengOlEntries _characterMaChengOlOperations;
    private readonly RuntimeEntries.CharacterMaWanOlEntries _characterMaWanOlOperations;
    private readonly RuntimeEntries.CharacterMaXiuMaTieOlEntries _characterMaXiuMaTieOlOperations;
    private readonly RuntimeEntries.CharacterQianZhaoOlEntries _characterQianZhaoOlOperations;
    private readonly RuntimeEntries.CharacterYangFuOlEntries _characterYangFuOlOperations;
    private readonly RuntimeEntries.CharacterZhangYanOlEntries _characterZhangYanOlOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreGameEngineCurrentEnhancementSlashRedirectsEntries _coreCurrentEnhancementSlashRedirectsOperations;
    private readonly RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    internal CoreNativeCardUseFramesRuntime(RuntimeEntries.CharacterLiuPanOlEntries characterLiuPanOlOperations, RuntimeEntries.CharacterMaChengOlEntries characterMaChengOlOperations, RuntimeEntries.CharacterMaWanOlEntries characterMaWanOlOperations, RuntimeEntries.CharacterMaXiuMaTieOlEntries characterMaXiuMaTieOlOperations, RuntimeEntries.CharacterQianZhaoOlEntries characterQianZhaoOlOperations, RuntimeEntries.CharacterYangFuOlEntries characterYangFuOlOperations, RuntimeEntries.CharacterZhangYanOlEntries characterZhangYanOlOperations, IKernelFrameStoreAccess resolutionStackOperations, RuntimeEntries.CoreGameEngineCurrentEnhancementSlashRedirectsEntries coreCurrentEnhancementSlashRedirectsOperations, RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations)
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
