#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreProgramOwnedZoneDiscardRuntimePortsRuntime : IProgramOwnedZoneDiscardFacts, IProgramOwnedZoneDiscardReceipts
{
    private readonly RuntimeEntries.CharacterBuZhiOlEntries _characterBuZhiOlOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramDiscardOriginsEntries _coreProgramDiscardOriginsOperations;
    private readonly RuntimeEntries.CoreHellMengpoProgramsEntries _coreHellMengpoProgramsOperations;
    internal CoreProgramOwnedZoneDiscardRuntimePortsRuntime(RuntimeEntries.CharacterBuZhiOlEntries characterBuZhiOlOperations, RuntimeEntries.CoreGameEngineProgramDiscardOriginsEntries coreProgramDiscardOriginsOperations, RuntimeEntries.CoreHellMengpoProgramsEntries coreHellMengpoProgramsOperations)
    {
        this._characterBuZhiOlOperations = characterBuZhiOlOperations;
        this._coreProgramDiscardOriginsOperations = coreProgramDiscardOriginsOperations;
        this._coreHellMengpoProgramsOperations = coreHellMengpoProgramsOperations;
    }
}
