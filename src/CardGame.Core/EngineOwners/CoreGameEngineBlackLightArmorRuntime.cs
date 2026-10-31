#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineBlackLightArmorRuntime
{
    private readonly ICoreBlackLightArmorResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreBlackLightArmorCoreCurrentCardEnhancementsOperations _coreCurrentCardEnhancementsOperations;
    private readonly ICoreBlackLightArmorCoreDirectedCardPolicyQueriesOperations _coreDirectedCardPolicyQueriesOperations;
    private readonly ICoreBlackLightArmorCoreEquipmentSkillsOperations _coreEquipmentSkillsOperations;
    private readonly ICoreBlackLightArmorCoreExtendedViewAsOperations _coreExtendedViewAsOperations;
    private readonly ICoreBlackLightArmorCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreBlackLightArmorCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineBlackLightArmorRuntime(ICoreBlackLightArmorResolutionStackOperations resolutionStackOperations, ICoreBlackLightArmorCoreCurrentCardEnhancementsOperations coreCurrentCardEnhancementsOperations, ICoreBlackLightArmorCoreDirectedCardPolicyQueriesOperations coreDirectedCardPolicyQueriesOperations, ICoreBlackLightArmorCoreEquipmentSkillsOperations coreEquipmentSkillsOperations, ICoreBlackLightArmorCoreExtendedViewAsOperations coreExtendedViewAsOperations, ICoreBlackLightArmorCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICoreBlackLightArmorCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCurrentCardEnhancementsOperations = coreCurrentCardEnhancementsOperations;
        this._coreDirectedCardPolicyQueriesOperations = coreDirectedCardPolicyQueriesOperations;
        this._coreEquipmentSkillsOperations = coreEquipmentSkillsOperations;
        this._coreExtendedViewAsOperations = coreExtendedViewAsOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
