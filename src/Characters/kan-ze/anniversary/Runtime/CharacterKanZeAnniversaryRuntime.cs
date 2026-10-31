#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterKanZeAnniversaryRuntime : global::CardGame.Core.IAnniversaryKanZeHost
{
    private readonly ICharacterKanZeAnniversaryContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterKanZeAnniversaryResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterKanZeAnniversaryCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICharacterKanZeAnniversaryCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICharacterKanZeAnniversaryCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterKanZeAnniversaryCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterKanZeAnniversaryCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICharacterKanZeAnniversaryKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterKanZeAnniversaryRuntime(ICharacterKanZeAnniversaryContentRegistryOperations contentRegistryOperations, ICharacterKanZeAnniversaryResolutionStackOperations resolutionStackOperations, ICharacterKanZeAnniversaryCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICharacterKanZeAnniversaryCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICharacterKanZeAnniversaryCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterKanZeAnniversaryCoreRuntimeOperations coreRuntimeOperations, ICharacterKanZeAnniversaryCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICharacterKanZeAnniversaryKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
