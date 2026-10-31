#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEnginePrivateOfferReturnsRuntime
{
    private readonly ICorePrivateOfferReturnsCardZonesOperations _cardZonesOperations;
    private readonly ICorePrivateOfferReturnsContentRegistryOperations _contentRegistryOperations;
    private readonly ICorePrivateOfferReturnsResolutionStackOperations _resolutionStackOperations;
    private readonly ICorePrivateOfferReturnsCoreAdvancedLifecycleOperations _coreAdvancedLifecycleOperations;
    private readonly ICorePrivateOfferReturnsCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICorePrivateOfferReturnsCoreFireTargetBenefitsOperations _coreFireTargetBenefitsOperations;
    private readonly ICorePrivateOfferReturnsCoreHpChangeProgramsOperations _coreHpChangeProgramsOperations;
    private readonly ICorePrivateOfferReturnsCorePrivateCardOffersOperations _corePrivateCardOffersOperations;
    private readonly ICorePrivateOfferReturnsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICorePrivateOfferReturnsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICorePrivateOfferReturnsCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICorePrivateOfferReturnsCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly ICorePrivateOfferReturnsCoreRecoveryReplacementOperations _coreRecoveryReplacementOperations;
    private readonly ICorePrivateOfferReturnsKernelCoreGameEngineAdvancedLifecycleHostOperations _kernelCoreGameEngineAdvancedLifecycleHostOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess _coreActualTurnProgressTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEnginePrivateOfferReturnsRuntime(ICorePrivateOfferReturnsCardZonesOperations cardZonesOperations, ICorePrivateOfferReturnsContentRegistryOperations contentRegistryOperations, ICorePrivateOfferReturnsResolutionStackOperations resolutionStackOperations, ICorePrivateOfferReturnsCoreAdvancedLifecycleOperations coreAdvancedLifecycleOperations, ICorePrivateOfferReturnsCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICorePrivateOfferReturnsCoreFireTargetBenefitsOperations coreFireTargetBenefitsOperations, ICorePrivateOfferReturnsCoreHpChangeProgramsOperations coreHpChangeProgramsOperations, ICorePrivateOfferReturnsCorePrivateCardOffersOperations corePrivateCardOffersOperations, ICorePrivateOfferReturnsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICorePrivateOfferReturnsCoreRuntimeOperations coreRuntimeOperations, ICorePrivateOfferReturnsCoreSkillProgramsOperations coreSkillProgramsOperations, ICorePrivateOfferReturnsCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, ICorePrivateOfferReturnsCoreRecoveryReplacementOperations coreRecoveryReplacementOperations, ICorePrivateOfferReturnsKernelCoreGameEngineAdvancedLifecycleHostOperations kernelCoreGameEngineAdvancedLifecycleHostOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess coreActualTurnProgressTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAdvancedLifecycleOperations = coreAdvancedLifecycleOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreFireTargetBenefitsOperations = coreFireTargetBenefitsOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._corePrivateCardOffersOperations = corePrivateCardOffersOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._coreRecoveryReplacementOperations = coreRecoveryReplacementOperations;
        this._kernelCoreGameEngineAdvancedLifecycleHostOperations = kernelCoreGameEngineAdvancedLifecycleHostOperations;
        this._coreActualTurnProgressTurnNumberAccess = coreActualTurnProgressTurnNumberAccess;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
