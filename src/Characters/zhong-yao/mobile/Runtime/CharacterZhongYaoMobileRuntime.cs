#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterZhongYaoMobileRuntime : global::CardGame.Core.IMobileZhongYaoSelectionQueries
{
    private readonly ICharacterZhongYaoMobileResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterZhongYaoMobileCoreCardActionsOperations _coreCardActionsOperations;
    private readonly ICharacterZhongYaoMobileCorePlayCardHistoryOperations _corePlayCardHistoryOperations;
    private readonly ICharacterZhongYaoMobileCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    internal CharacterZhongYaoMobileRuntime(ICharacterZhongYaoMobileResolutionStackOperations resolutionStackOperations, ICharacterZhongYaoMobileCoreCardActionsOperations coreCardActionsOperations, ICharacterZhongYaoMobileCorePlayCardHistoryOperations corePlayCardHistoryOperations, ICharacterZhongYaoMobileCoreProgramLifecycleOperations coreProgramLifecycleOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardActionsOperations = coreCardActionsOperations;
        this._corePlayCardHistoryOperations = corePlayCardHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
    }
}
