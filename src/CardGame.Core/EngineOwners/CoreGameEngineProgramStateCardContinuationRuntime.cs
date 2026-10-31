#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramStateCardContinuationRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGuanYuWumiaoEntries _characterGuanYuWumiaoOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDragonBoatCardsEntries _coreDragonBoatCardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineGuanduCardsEntries _coreGuanduCardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSoleIncomingRangeDrawEntries _coreSoleIncomingRangeDrawOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWenheCardsEntries _coreWenheCardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardEffectDrawsEntries _coreNativeCardEffectDrawsOperations;
    internal CoreGameEngineProgramStateCardContinuationRuntime(global::CardGame.Core.RuntimeEntries.CharacterGuanYuWumiaoEntries characterGuanYuWumiaoOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDragonBoatCardsEntries coreDragonBoatCardsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineGuanduCardsEntries coreGuanduCardsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSoleIncomingRangeDrawEntries coreSoleIncomingRangeDrawOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineWenheCardsEntries coreWenheCardsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardEffectDrawsEntries coreNativeCardEffectDrawsOperations)
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
