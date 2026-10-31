#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineAlcoholIdentityPoliciesRuntime
{
    private readonly ICoreAlcoholIdentityPoliciesCharacterGuanYuWumiaoOperations _characterGuanYuWumiaoOperations;
    private readonly ICoreAlcoholIdentityPoliciesCharacterZhouYuOlMouOperations _characterZhouYuOlMouOperations;
    private readonly ICoreAlcoholIdentityPoliciesCardZonesOperations _cardZonesOperations;
    private readonly ICoreAlcoholIdentityPoliciesContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreAlcoholIdentityPoliciesResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreAlcoholIdentityPoliciesCoreDyingCursorRuntimeOperations _coreDyingCursorRuntimeOperations;
    private readonly ICoreAlcoholIdentityPoliciesCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    private readonly ICoreAlcoholIdentityPoliciesCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreAlcoholIdentityPoliciesCoreWushuangTacticRulesOperations _coreWushuangTacticRulesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineAlcoholIdentityPoliciesRuntime(ICoreAlcoholIdentityPoliciesCharacterGuanYuWumiaoOperations characterGuanYuWumiaoOperations, ICoreAlcoholIdentityPoliciesCharacterZhouYuOlMouOperations characterZhouYuOlMouOperations, ICoreAlcoholIdentityPoliciesCardZonesOperations cardZonesOperations, ICoreAlcoholIdentityPoliciesContentRegistryOperations contentRegistryOperations, ICoreAlcoholIdentityPoliciesResolutionStackOperations resolutionStackOperations, ICoreAlcoholIdentityPoliciesCoreDyingCursorRuntimeOperations coreDyingCursorRuntimeOperations, ICoreAlcoholIdentityPoliciesCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations, ICoreAlcoholIdentityPoliciesCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreAlcoholIdentityPoliciesCoreWushuangTacticRulesOperations coreWushuangTacticRulesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterGuanYuWumiaoOperations = characterGuanYuWumiaoOperations;
        this._characterZhouYuOlMouOperations = characterZhouYuOlMouOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreWushuangTacticRulesOperations = coreWushuangTacticRulesOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
