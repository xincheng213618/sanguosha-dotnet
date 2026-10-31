#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterCaoZhiBoundaryMobileRuntime : global::CardGame.Core.IBoundaryCaoZhiHost
{
    private readonly ICharacterCaoZhiBoundaryMobileCardZonesOperations _cardZonesOperations;
    private readonly ICharacterCaoZhiBoundaryMobileContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterCaoZhiBoundaryMobileRandomOperations _randomOperations;
    private readonly ICharacterCaoZhiBoundaryMobileResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterCaoZhiBoundaryMobileCoreProgramAttackAttemptOperations _coreProgramAttackAttemptOperations;
    private readonly ICharacterCaoZhiBoundaryMobileCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICharacterCaoZhiBoundaryMobileCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterCaoZhiBoundaryMobileCoreProgramSkillChangesOperations _coreProgramSkillChangesOperations;
    private readonly ICharacterCaoZhiBoundaryMobileKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess _coreActualTurnProgressCurrentSeatAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterCaoZhiBoundaryMobileRuntime(ICharacterCaoZhiBoundaryMobileCardZonesOperations cardZonesOperations, ICharacterCaoZhiBoundaryMobileContentRegistryOperations contentRegistryOperations, ICharacterCaoZhiBoundaryMobileRandomOperations randomOperations, ICharacterCaoZhiBoundaryMobileResolutionStackOperations resolutionStackOperations, ICharacterCaoZhiBoundaryMobileCoreProgramAttackAttemptOperations coreProgramAttackAttemptOperations, ICharacterCaoZhiBoundaryMobileCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICharacterCaoZhiBoundaryMobileCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterCaoZhiBoundaryMobileCoreProgramSkillChangesOperations coreProgramSkillChangesOperations, ICharacterCaoZhiBoundaryMobileKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess coreActualTurnProgressCurrentSeatAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._randomOperations = randomOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreProgramAttackAttemptOperations = coreProgramAttackAttemptOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreProgramSkillChangesOperations = coreProgramSkillChangesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatAccess = coreActualTurnProgressCurrentSeatAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
