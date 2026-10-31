#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineLegacyDyingAlcoholReturnsRuntime
{
    private readonly ICoreLegacyDyingAlcoholReturnsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreLegacyDyingAlcoholReturnsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreLegacyDyingAlcoholReturnsCoreDyingCursorRuntimeOperations _coreDyingCursorRuntimeOperations;
    private readonly ICoreLegacyDyingAlcoholReturnsCoreActualTurnLegacyCompletionOperations _coreActualTurnLegacyCompletionOperations;
    private readonly ICoreLegacyDyingAlcoholReturnsCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreLegacyDyingAlcoholReturnsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    internal CoreGameEngineLegacyDyingAlcoholReturnsRuntime(ICoreLegacyDyingAlcoholReturnsContentRegistryOperations contentRegistryOperations, ICoreLegacyDyingAlcoholReturnsResolutionStackOperations resolutionStackOperations, ICoreLegacyDyingAlcoholReturnsCoreDyingCursorRuntimeOperations coreDyingCursorRuntimeOperations, ICoreLegacyDyingAlcoholReturnsCoreActualTurnLegacyCompletionOperations coreActualTurnLegacyCompletionOperations, ICoreLegacyDyingAlcoholReturnsCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreLegacyDyingAlcoholReturnsCoreProgramLifecycleOperations coreProgramLifecycleOperations)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreActualTurnLegacyCompletionOperations = coreActualTurnLegacyCompletionOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
    }
}
