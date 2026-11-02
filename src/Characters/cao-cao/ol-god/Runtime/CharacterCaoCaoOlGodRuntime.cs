#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CharacterCaoCaoOlGodRuntime(
    IProgramSeatAcquisitions acquisitions,
    IProgramRuntimeFrames frames,
    IProgramRuntimePrompts prompts,
    IProgramRuntimeEvents events,
    IKernelContentRegistryAccess registry,
    IKernelRuleProgressionOperations progression,
    EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess participants) : IOlGuixinHost
{
}
