#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreProgramOwnedZoneDiscardRuntimePortsRuntime : global::CardGame.Core.IProgramOwnedZoneDiscardFacts, global::CardGame.Core.IProgramOwnedZoneDiscardReceipts
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterBuZhiOlEntries _characterBuZhiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramDiscardOriginsEntries _coreProgramDiscardOriginsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreHellMengpoProgramsEntries _coreHellMengpoProgramsOperations;
    internal CoreProgramOwnedZoneDiscardRuntimePortsRuntime(global::CardGame.Core.RuntimeEntries.CharacterBuZhiOlEntries characterBuZhiOlOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramDiscardOriginsEntries coreProgramDiscardOriginsOperations, global::CardGame.Core.RuntimeEntries.CoreHellMengpoProgramsEntries coreHellMengpoProgramsOperations)
    {
        this._characterBuZhiOlOperations = characterBuZhiOlOperations;
        this._coreProgramDiscardOriginsOperations = coreProgramDiscardOriginsOperations;
        this._coreHellMengpoProgramsOperations = coreHellMengpoProgramsOperations;
    }
}
