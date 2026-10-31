#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineExtraEffectMaterialClaimsRuntime
{
    private readonly ICoreExtraEffectMaterialClaimsCharacterFeiYiOlOperations _characterFeiYiOlOperations;
    private readonly ICoreExtraEffectMaterialClaimsCardZonesOperations _cardZonesOperations;
    private readonly ICoreExtraEffectMaterialClaimsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreExtraEffectMaterialClaimsCoreDamageCursorRuntimeOperations _coreDamageCursorRuntimeOperations;
    private readonly ICoreExtraEffectMaterialClaimsCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreExtraEffectMaterialClaimsCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICoreExtraEffectMaterialClaimsCoreNativeDamageHandlesOperations _coreNativeDamageHandlesOperations;
    private readonly ICoreExtraEffectMaterialClaimsCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    internal CoreGameEngineExtraEffectMaterialClaimsRuntime(ICoreExtraEffectMaterialClaimsCharacterFeiYiOlOperations characterFeiYiOlOperations, ICoreExtraEffectMaterialClaimsCardZonesOperations cardZonesOperations, ICoreExtraEffectMaterialClaimsContentRegistryOperations contentRegistryOperations, ICoreExtraEffectMaterialClaimsCoreDamageCursorRuntimeOperations coreDamageCursorRuntimeOperations, ICoreExtraEffectMaterialClaimsCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreExtraEffectMaterialClaimsCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICoreExtraEffectMaterialClaimsCoreNativeDamageHandlesOperations coreNativeDamageHandlesOperations, ICoreExtraEffectMaterialClaimsCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess)
    {
        this._characterFeiYiOlOperations = characterFeiYiOlOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreDamageCursorRuntimeOperations = coreDamageCursorRuntimeOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
    }
}
