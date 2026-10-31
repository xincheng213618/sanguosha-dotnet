#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineTreatedWoundedRuntime
{
    private readonly RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries _characterDaQiaoXiaoQiaoHappyGodOperations;
    private readonly RuntimeEntries.CharacterSunHaoOlEntries _characterSunHaoOlOperations;
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    internal CoreGameEngineTreatedWoundedRuntime(RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries characterDaQiaoXiaoQiaoHappyGodOperations, RuntimeEntries.CharacterSunHaoOlEntries characterSunHaoOlOperations, IKernelContentRegistryAccess contentRegistryOperations)
    {
        this._characterDaQiaoXiaoQiaoHappyGodOperations = characterDaQiaoXiaoQiaoHappyGodOperations;
        this._characterSunHaoOlOperations = characterSunHaoOlOperations;
        this._contentRegistryOperations = contentRegistryOperations;
    }
}
