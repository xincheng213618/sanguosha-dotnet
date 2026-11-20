#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreGameEngineTreatedWoundedRuntime(
    RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries _characterDaQiaoXiaoQiaoHappyGodOperations,
    RuntimeEntries.DeferredRuntime<EngineOwners.CharacterSunHaoOlRuntime> _characterSunHaoOlOperations,
    IKernelContentRegistryAccess _contentRegistryOperations,
    EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _participants) : ISelectionWoundedQueries
{
}
