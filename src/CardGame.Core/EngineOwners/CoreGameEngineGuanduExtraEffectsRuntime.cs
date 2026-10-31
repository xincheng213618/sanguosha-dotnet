#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineGuanduExtraEffectsRuntime
{
    private readonly ICoreGuanduExtraEffectsCoreDirectedTurnCardPoliciesOperations _coreDirectedTurnCardPoliciesOperations;
    private readonly ICoreGuanduExtraEffectsCoreDeckEquipmentProgramsOperations _coreDeckEquipmentProgramsOperations;
    private readonly ICoreGuanduExtraEffectsCoreGuanduActivitiesOperations _coreGuanduActivitiesOperations;
    private readonly ICoreGuanduExtraEffectsCoreProgramCardUseRolesOperations _coreProgramCardUseRolesOperations;
    private readonly ICoreGuanduExtraEffectsCoreShenwuEquipmentOperations _coreShenwuEquipmentOperations;
    private readonly ICoreGuanduExtraEffectsCoreTreatedWoundedOperations _coreTreatedWoundedOperations;
    private readonly ICoreGuanduExtraEffectsCoreNativeBorrowedSwordOperations _coreNativeBorrowedSwordOperations;
    private readonly ICoreGuanduExtraEffectsCoreNativeDamagePoliciesOperations _coreNativeDamagePoliciesOperations;
    private readonly ICoreGuanduExtraEffectsCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreGuanduExtraEffectsCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineGuanduExtraEffectsRuntime(ICoreGuanduExtraEffectsCoreDirectedTurnCardPoliciesOperations coreDirectedTurnCardPoliciesOperations, ICoreGuanduExtraEffectsCoreDeckEquipmentProgramsOperations coreDeckEquipmentProgramsOperations, ICoreGuanduExtraEffectsCoreGuanduActivitiesOperations coreGuanduActivitiesOperations, ICoreGuanduExtraEffectsCoreProgramCardUseRolesOperations coreProgramCardUseRolesOperations, ICoreGuanduExtraEffectsCoreShenwuEquipmentOperations coreShenwuEquipmentOperations, ICoreGuanduExtraEffectsCoreTreatedWoundedOperations coreTreatedWoundedOperations, ICoreGuanduExtraEffectsCoreNativeBorrowedSwordOperations coreNativeBorrowedSwordOperations, ICoreGuanduExtraEffectsCoreNativeDamagePoliciesOperations coreNativeDamagePoliciesOperations, ICoreGuanduExtraEffectsCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICoreGuanduExtraEffectsCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._coreDirectedTurnCardPoliciesOperations = coreDirectedTurnCardPoliciesOperations;
        this._coreDeckEquipmentProgramsOperations = coreDeckEquipmentProgramsOperations;
        this._coreGuanduActivitiesOperations = coreGuanduActivitiesOperations;
        this._coreProgramCardUseRolesOperations = coreProgramCardUseRolesOperations;
        this._coreShenwuEquipmentOperations = coreShenwuEquipmentOperations;
        this._coreTreatedWoundedOperations = coreTreatedWoundedOperations;
        this._coreNativeBorrowedSwordOperations = coreNativeBorrowedSwordOperations;
        this._coreNativeDamagePoliciesOperations = coreNativeDamagePoliciesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
