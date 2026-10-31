#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CharacterDaQiaoMobileRuntime
{
    private readonly ICharacterDaQiaoMobileCharacterSunYuanOlOperations _characterSunYuanOlOperations;
    private readonly ICharacterDaQiaoMobileCharacterTaishiCiMobileGodOperations _characterTaishiCiMobileGodOperations;
    private readonly ICharacterDaQiaoMobileCardZonesOperations _cardZonesOperations;
    private readonly ICharacterDaQiaoMobileContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterDaQiaoMobileResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterDaQiaoMobileCoreActiveNativeAttackQueriesOperations _coreActiveNativeAttackQueriesOperations;
    private readonly ICharacterDaQiaoMobileCoreActionQueriesOperations _coreActionQueriesOperations;
    private readonly ICharacterDaQiaoMobileCoreCardActionsOperations _coreCardActionsOperations;
    private readonly ICharacterDaQiaoMobileCoreCardAttackStateOperations _coreCardAttackStateOperations;
    private readonly ICharacterDaQiaoMobileCoreProgramAttackEffectsOperations _coreProgramAttackEffectsOperations;
    private readonly ICharacterDaQiaoMobileCoreProgramCardPaymentsOperations _coreProgramCardPaymentsOperations;
    private readonly ICharacterDaQiaoMobileCoreProgramOwnedCardsOperations _coreProgramOwnedCardsOperations;
    private readonly ICharacterDaQiaoMobileCoreProgramPersistentCombatPoliciesOperations _coreProgramPersistentCombatPoliciesOperations;
    private readonly ICharacterDaQiaoMobileCoreTurnHandCategoryRestrictionsOperations _coreTurnHandCategoryRestrictionsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterDaQiaoMobileRuntime(ICharacterDaQiaoMobileCharacterSunYuanOlOperations characterSunYuanOlOperations, ICharacterDaQiaoMobileCharacterTaishiCiMobileGodOperations characterTaishiCiMobileGodOperations, ICharacterDaQiaoMobileCardZonesOperations cardZonesOperations, ICharacterDaQiaoMobileContentRegistryOperations contentRegistryOperations, ICharacterDaQiaoMobileResolutionStackOperations resolutionStackOperations, ICharacterDaQiaoMobileCoreActiveNativeAttackQueriesOperations coreActiveNativeAttackQueriesOperations, ICharacterDaQiaoMobileCoreActionQueriesOperations coreActionQueriesOperations, ICharacterDaQiaoMobileCoreCardActionsOperations coreCardActionsOperations, ICharacterDaQiaoMobileCoreCardAttackStateOperations coreCardAttackStateOperations, ICharacterDaQiaoMobileCoreProgramAttackEffectsOperations coreProgramAttackEffectsOperations, ICharacterDaQiaoMobileCoreProgramCardPaymentsOperations coreProgramCardPaymentsOperations, ICharacterDaQiaoMobileCoreProgramOwnedCardsOperations coreProgramOwnedCardsOperations, ICharacterDaQiaoMobileCoreProgramPersistentCombatPoliciesOperations coreProgramPersistentCombatPoliciesOperations, ICharacterDaQiaoMobileCoreTurnHandCategoryRestrictionsOperations coreTurnHandCategoryRestrictionsOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterSunYuanOlOperations = characterSunYuanOlOperations;
        this._characterTaishiCiMobileGodOperations = characterTaishiCiMobileGodOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActiveNativeAttackQueriesOperations = coreActiveNativeAttackQueriesOperations;
        this._coreActionQueriesOperations = coreActionQueriesOperations;
        this._coreCardActionsOperations = coreCardActionsOperations;
        this._coreCardAttackStateOperations = coreCardAttackStateOperations;
        this._coreProgramAttackEffectsOperations = coreProgramAttackEffectsOperations;
        this._coreProgramCardPaymentsOperations = coreProgramCardPaymentsOperations;
        this._coreProgramOwnedCardsOperations = coreProgramOwnedCardsOperations;
        this._coreProgramPersistentCombatPoliciesOperations = coreProgramPersistentCombatPoliciesOperations;
        this._coreTurnHandCategoryRestrictionsOperations = coreTurnHandCategoryRestrictionsOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
