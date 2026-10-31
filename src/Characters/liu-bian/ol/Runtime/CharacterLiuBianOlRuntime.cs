#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterLiuBianOlRuntime : global::CardGame.Core.ILiuBianProgramHost
{
    private readonly ICharacterLiuBianOlContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterLiuBianOlCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICharacterLiuBianOlCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICharacterLiuBianOlCoreCharacterHostOperations _coreCharacterHostOperations;
    private readonly ICharacterLiuBianOlCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICharacterLiuBianOlCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterLiuBianOlCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterLiuBianOlCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICharacterLiuBianOlCoreMatchSetupDefinitionsOperations _coreMatchSetupDefinitionsOperations;
    private readonly ICharacterLiuBianOlCorePhysicalDrawFacadesOperations _corePhysicalDrawFacadesOperations;
    private readonly ICharacterLiuBianOlCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICharacterLiuBianOlKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterLiuBianOlRuntime(ICharacterLiuBianOlContentRegistryOperations contentRegistryOperations, ICharacterLiuBianOlCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICharacterLiuBianOlCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICharacterLiuBianOlCoreCharacterHostOperations coreCharacterHostOperations, ICharacterLiuBianOlCoreNationalSkillsOperations coreNationalSkillsOperations, ICharacterLiuBianOlCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterLiuBianOlCoreRuntimeOperations coreRuntimeOperations, ICharacterLiuBianOlCoreMatchLogOperations coreMatchLogOperations, ICharacterLiuBianOlCoreMatchSetupDefinitionsOperations coreMatchSetupDefinitionsOperations, ICharacterLiuBianOlCorePhysicalDrawFacadesOperations corePhysicalDrawFacadesOperations, ICharacterLiuBianOlCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICharacterLiuBianOlKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreCharacterHostOperations = coreCharacterHostOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreMatchSetupDefinitionsOperations = coreMatchSetupDefinitionsOperations;
        this._corePhysicalDrawFacadesOperations = corePhysicalDrawFacadesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess = coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
