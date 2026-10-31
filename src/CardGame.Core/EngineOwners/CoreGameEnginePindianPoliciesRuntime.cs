#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEnginePindianPoliciesRuntime
{
    private readonly ICorePindianPoliciesCharacterCuiLieAnniversaryOperations _characterCuiLieAnniversaryOperations;
    private readonly ICorePindianPoliciesCharacterGuanHaiOlOperations _characterGuanHaiOlOperations;
    private readonly ICorePindianPoliciesCharacterLuKangWumiaoOperations _characterLuKangWumiaoOperations;
    private readonly ICorePindianPoliciesCardZonesOperations _cardZonesOperations;
    private readonly ICorePindianPoliciesContentRegistryOperations _contentRegistryOperations;
    private readonly ICorePindianPoliciesCoreAlcoholIdentityPoliciesOperations _coreAlcoholIdentityPoliciesOperations;
    private readonly ICorePindianPoliciesCorePindianOperations _corePindianOperations;
    private readonly ICorePindianPoliciesCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    private readonly ICorePindianPoliciesCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICorePindianPoliciesCoreNativeCardAppearanceOperations _coreNativeCardAppearanceOperations;
    private readonly ICorePindianPoliciesCorePhysicalDrawFacadesOperations _corePhysicalDrawFacadesOperations;
    private readonly ICorePindianPoliciesCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly ICorePindianPoliciesCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICorePindianPoliciesKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEnginePindianPoliciesRuntime(ICorePindianPoliciesCharacterCuiLieAnniversaryOperations characterCuiLieAnniversaryOperations, ICorePindianPoliciesCharacterGuanHaiOlOperations characterGuanHaiOlOperations, ICorePindianPoliciesCharacterLuKangWumiaoOperations characterLuKangWumiaoOperations, ICorePindianPoliciesCardZonesOperations cardZonesOperations, ICorePindianPoliciesContentRegistryOperations contentRegistryOperations, ICorePindianPoliciesCoreAlcoholIdentityPoliciesOperations coreAlcoholIdentityPoliciesOperations, ICorePindianPoliciesCorePindianOperations corePindianOperations, ICorePindianPoliciesCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations, ICorePindianPoliciesCoreMatchLogOperations coreMatchLogOperations, ICorePindianPoliciesCoreNativeCardAppearanceOperations coreNativeCardAppearanceOperations, ICorePindianPoliciesCorePhysicalDrawFacadesOperations corePhysicalDrawFacadesOperations, ICorePindianPoliciesCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, ICorePindianPoliciesCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICorePindianPoliciesKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterCuiLieAnniversaryOperations = characterCuiLieAnniversaryOperations;
        this._characterGuanHaiOlOperations = characterGuanHaiOlOperations;
        this._characterLuKangWumiaoOperations = characterLuKangWumiaoOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreAlcoholIdentityPoliciesOperations = coreAlcoholIdentityPoliciesOperations;
        this._corePindianOperations = corePindianOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativeCardAppearanceOperations = coreNativeCardAppearanceOperations;
        this._corePhysicalDrawFacadesOperations = corePhysicalDrawFacadesOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
