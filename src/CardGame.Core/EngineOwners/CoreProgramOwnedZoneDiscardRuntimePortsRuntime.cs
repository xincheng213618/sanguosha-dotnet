#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreProgramOwnedZoneDiscardRuntimePortsRuntime : global::CardGame.Core.IProgramOwnedZoneDiscardFacts, global::CardGame.Core.IProgramOwnedZoneDiscardReceipts
{
    private readonly ICoreProgramOwnedZoneDiscardRuntimePortsCharacterBuZhiOlOperations _characterBuZhiOlOperations;
    private readonly ICoreProgramOwnedZoneDiscardRuntimePortsCoreProgramDiscardOriginsOperations _coreProgramDiscardOriginsOperations;
    private readonly ICoreProgramOwnedZoneDiscardRuntimePortsCoreHellMengpoProgramsOperations _coreHellMengpoProgramsOperations;
    internal CoreProgramOwnedZoneDiscardRuntimePortsRuntime(ICoreProgramOwnedZoneDiscardRuntimePortsCharacterBuZhiOlOperations characterBuZhiOlOperations, ICoreProgramOwnedZoneDiscardRuntimePortsCoreProgramDiscardOriginsOperations coreProgramDiscardOriginsOperations, ICoreProgramOwnedZoneDiscardRuntimePortsCoreHellMengpoProgramsOperations coreHellMengpoProgramsOperations)
    {
        this._characterBuZhiOlOperations = characterBuZhiOlOperations;
        this._coreProgramDiscardOriginsOperations = coreProgramDiscardOriginsOperations;
        this._coreHellMengpoProgramsOperations = coreHellMengpoProgramsOperations;
    }
}
