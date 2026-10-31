#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterLiuZanMobileRuntime : global::CardGame.Core.IAlternateActualUseColorProgramHost
{
    private readonly ICharacterLiuZanMobileContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterLiuZanMobileResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterLiuZanMobileCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICharacterLiuZanMobileCoreAlternativeViewAsCostsOperations _coreAlternativeViewAsCostsOperations;
    private readonly ICharacterLiuZanMobileCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICharacterLiuZanMobileCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterLiuZanMobileKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterLiuZanMobileRuntime(ICharacterLiuZanMobileContentRegistryOperations contentRegistryOperations, ICharacterLiuZanMobileResolutionStackOperations resolutionStackOperations, ICharacterLiuZanMobileCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICharacterLiuZanMobileCoreAlternativeViewAsCostsOperations coreAlternativeViewAsCostsOperations, ICharacterLiuZanMobileCoreNationalSkillsOperations coreNationalSkillsOperations, ICharacterLiuZanMobileCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterLiuZanMobileKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreAlternativeViewAsCostsOperations = coreAlternativeViewAsCostsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
