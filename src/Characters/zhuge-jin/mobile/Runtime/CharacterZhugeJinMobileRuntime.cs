#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CharacterZhugeJinMobileRuntime
{
    private readonly ICharacterZhugeJinMobileContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterZhugeJinMobileResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterZhugeJinMobileCoreCardUsePaymentsOperations _coreCardUsePaymentsOperations;
    private readonly ICharacterZhugeJinMobileCoreProgramDiscardOriginsOperations _coreProgramDiscardOriginsOperations;
    private readonly ICharacterZhugeJinMobileCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICharacterZhugeJinMobileCoreTableOrderOperations _coreTableOrderOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnProgressionAccess _coreActualTurnProgressTurnProgressionAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    internal CharacterZhugeJinMobileRuntime(ICharacterZhugeJinMobileContentRegistryOperations contentRegistryOperations, ICharacterZhugeJinMobileResolutionStackOperations resolutionStackOperations, ICharacterZhugeJinMobileCoreCardUsePaymentsOperations coreCardUsePaymentsOperations, ICharacterZhugeJinMobileCoreProgramDiscardOriginsOperations coreProgramDiscardOriginsOperations, ICharacterZhugeJinMobileCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICharacterZhugeJinMobileCoreTableOrderOperations coreTableOrderOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnProgressionAccess coreActualTurnProgressTurnProgressionAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardUsePaymentsOperations = coreCardUsePaymentsOperations;
        this._coreProgramDiscardOriginsOperations = coreProgramDiscardOriginsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._coreActualTurnProgressTurnProgressionAccess = coreActualTurnProgressTurnProgressionAccess;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
    }
}
