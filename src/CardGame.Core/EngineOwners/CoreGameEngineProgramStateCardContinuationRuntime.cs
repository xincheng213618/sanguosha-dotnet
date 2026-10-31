#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramStateCardContinuationRuntime
{
    private readonly ICoreProgramStateCardContinuationCharacterGuanYuWumiaoOperations _characterGuanYuWumiaoOperations;
    private readonly ICoreProgramStateCardContinuationResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreProgramStateCardContinuationCoreDragonBoatCardsOperations _coreDragonBoatCardsOperations;
    private readonly ICoreProgramStateCardContinuationCoreGuanduCardsOperations _coreGuanduCardsOperations;
    private readonly ICoreProgramStateCardContinuationCoreSoleIncomingRangeDrawOperations _coreSoleIncomingRangeDrawOperations;
    private readonly ICoreProgramStateCardContinuationCoreWenheCardsOperations _coreWenheCardsOperations;
    private readonly ICoreProgramStateCardContinuationCoreNativeCardEffectDrawsOperations _coreNativeCardEffectDrawsOperations;
    internal CoreGameEngineProgramStateCardContinuationRuntime(ICoreProgramStateCardContinuationCharacterGuanYuWumiaoOperations characterGuanYuWumiaoOperations, ICoreProgramStateCardContinuationResolutionStackOperations resolutionStackOperations, ICoreProgramStateCardContinuationCoreDragonBoatCardsOperations coreDragonBoatCardsOperations, ICoreProgramStateCardContinuationCoreGuanduCardsOperations coreGuanduCardsOperations, ICoreProgramStateCardContinuationCoreSoleIncomingRangeDrawOperations coreSoleIncomingRangeDrawOperations, ICoreProgramStateCardContinuationCoreWenheCardsOperations coreWenheCardsOperations, ICoreProgramStateCardContinuationCoreNativeCardEffectDrawsOperations coreNativeCardEffectDrawsOperations)
    {
        this._characterGuanYuWumiaoOperations = characterGuanYuWumiaoOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDragonBoatCardsOperations = coreDragonBoatCardsOperations;
        this._coreGuanduCardsOperations = coreGuanduCardsOperations;
        this._coreSoleIncomingRangeDrawOperations = coreSoleIncomingRangeDrawOperations;
        this._coreWenheCardsOperations = coreWenheCardsOperations;
        this._coreNativeCardEffectDrawsOperations = coreNativeCardEffectDrawsOperations;
    }
}
