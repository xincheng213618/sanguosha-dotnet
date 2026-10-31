#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterGuYongBoundaryMobileRuntime : global::CardGame.Core.IBoundaryGuYongHost
{
    private readonly ICharacterGuYongBoundaryMobileCardZonesOperations _cardZonesOperations;
    private readonly ICharacterGuYongBoundaryMobileContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterGuYongBoundaryMobileCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    private readonly ICharacterGuYongBoundaryMobileCoreProgramCategoryChallengesOperations _coreProgramCategoryChallengesOperations;
    private readonly ICharacterGuYongBoundaryMobileCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICharacterGuYongBoundaryMobileCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterGuYongBoundaryMobileCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterGuYongBoundaryMobileCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICharacterGuYongBoundaryMobileCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICharacterGuYongBoundaryMobileKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterGuYongBoundaryMobileRuntime(ICharacterGuYongBoundaryMobileCardZonesOperations cardZonesOperations, ICharacterGuYongBoundaryMobileContentRegistryOperations contentRegistryOperations, ICharacterGuYongBoundaryMobileCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations, ICharacterGuYongBoundaryMobileCoreProgramCategoryChallengesOperations coreProgramCategoryChallengesOperations, ICharacterGuYongBoundaryMobileCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICharacterGuYongBoundaryMobileCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterGuYongBoundaryMobileCoreRuntimeOperations coreRuntimeOperations, ICharacterGuYongBoundaryMobileCoreSkillProgramsOperations coreSkillProgramsOperations, ICharacterGuYongBoundaryMobileCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICharacterGuYongBoundaryMobileKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreProgramCategoryChallengesOperations = coreProgramCategoryChallengesOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
