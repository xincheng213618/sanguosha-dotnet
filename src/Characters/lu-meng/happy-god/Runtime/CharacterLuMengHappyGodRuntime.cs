#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterLuMengHappyGodRuntime : global::CardGame.Core.IHappyGongxinHost
{
    private readonly ICharacterLuMengHappyGodResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterLuMengHappyGodSkillRuntimeStateOperations _skillRuntimeStateOperations;
    private readonly ICharacterLuMengHappyGodCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICharacterLuMengHappyGodCoreDesignatedExtraTargetsOperations _coreDesignatedExtraTargetsOperations;
    private readonly ICharacterLuMengHappyGodCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICharacterLuMengHappyGodCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterLuMengHappyGodCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterLuMengHappyGodCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICharacterLuMengHappyGodCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICharacterLuMengHappyGodKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess _coreActualTurnProgressTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterLuMengHappyGodRuntime(ICharacterLuMengHappyGodResolutionStackOperations resolutionStackOperations, ICharacterLuMengHappyGodSkillRuntimeStateOperations skillRuntimeStateOperations, ICharacterLuMengHappyGodCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICharacterLuMengHappyGodCoreDesignatedExtraTargetsOperations coreDesignatedExtraTargetsOperations, ICharacterLuMengHappyGodCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICharacterLuMengHappyGodCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterLuMengHappyGodCoreRuntimeOperations coreRuntimeOperations, ICharacterLuMengHappyGodCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICharacterLuMengHappyGodCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICharacterLuMengHappyGodKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess coreActualTurnProgressTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._skillRuntimeStateOperations = skillRuntimeStateOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreDesignatedExtraTargetsOperations = coreDesignatedExtraTargetsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressTurnNumberAccess = coreActualTurnProgressTurnNumberAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
