#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineCuiYanNativeQueriesRuntime
{
    private readonly ICoreCuiYanNativeQueriesCharacterCuiYanOlOperations _characterCuiYanOlOperations;
    private readonly ICoreCuiYanNativeQueriesCharacterWuJingOlOperations _characterWuJingOlOperations;
    private readonly ICoreCuiYanNativeQueriesCoreAdvancedLifecycleOperations _coreAdvancedLifecycleOperations;
    private readonly ICoreCuiYanNativeQueriesCoreCardConversionsOperations _coreCardConversionsOperations;
    private readonly ICoreCuiYanNativeQueriesCoreOwnPublicPileConversionsOperations _coreOwnPublicPileConversionsOperations;
    private readonly ICoreCuiYanNativeQueriesCorePlayCardHistoryOperations _corePlayCardHistoryOperations;
    private readonly ICoreCuiYanNativeQueriesCoreNativeCardAppearanceOperations _coreNativeCardAppearanceOperations;
    private readonly ICoreCuiYanNativeQueriesCoreNativeTargetLegalityOperations _coreNativeTargetLegalityOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineCuiYanNativeQueriesRuntime(ICoreCuiYanNativeQueriesCharacterCuiYanOlOperations characterCuiYanOlOperations, ICoreCuiYanNativeQueriesCharacterWuJingOlOperations characterWuJingOlOperations, ICoreCuiYanNativeQueriesCoreAdvancedLifecycleOperations coreAdvancedLifecycleOperations, ICoreCuiYanNativeQueriesCoreCardConversionsOperations coreCardConversionsOperations, ICoreCuiYanNativeQueriesCoreOwnPublicPileConversionsOperations coreOwnPublicPileConversionsOperations, ICoreCuiYanNativeQueriesCorePlayCardHistoryOperations corePlayCardHistoryOperations, ICoreCuiYanNativeQueriesCoreNativeCardAppearanceOperations coreNativeCardAppearanceOperations, ICoreCuiYanNativeQueriesCoreNativeTargetLegalityOperations coreNativeTargetLegalityOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterCuiYanOlOperations = characterCuiYanOlOperations;
        this._characterWuJingOlOperations = characterWuJingOlOperations;
        this._coreAdvancedLifecycleOperations = coreAdvancedLifecycleOperations;
        this._coreCardConversionsOperations = coreCardConversionsOperations;
        this._coreOwnPublicPileConversionsOperations = coreOwnPublicPileConversionsOperations;
        this._corePlayCardHistoryOperations = corePlayCardHistoryOperations;
        this._coreNativeCardAppearanceOperations = coreNativeCardAppearanceOperations;
        this._coreNativeTargetLegalityOperations = coreNativeTargetLegalityOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
