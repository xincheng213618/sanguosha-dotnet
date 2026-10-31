#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineShanheMuluRuntime : global::CardGame.Core.IShanheMuluProgramHost
{
    private readonly ICoreShanheMuluCardZonesOperations _cardZonesOperations;
    private readonly ICoreShanheMuluContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreShanheMuluResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreShanheMuluCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreShanheMuluCoreCardAttackStateOperations _coreCardAttackStateOperations;
    private readonly ICoreShanheMuluCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICoreShanheMuluCoreCharacterStateProgramsOperations _coreCharacterStateProgramsOperations;
    private readonly ICoreShanheMuluCoreHpChangeProgramsOperations _coreHpChangeProgramsOperations;
    private readonly ICoreShanheMuluCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreShanheMuluCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreShanheMuluCoreProgramLifeRestorationOperations _coreProgramLifeRestorationOperations;
    private readonly ICoreShanheMuluCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreShanheMuluCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreShanheMuluCoreSkillOwnershipWindowsOperations _coreSkillOwnershipWindowsOperations;
    private readonly ICoreShanheMuluCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICoreShanheMuluCoreTeamActivitiesOperations _coreTeamActivitiesOperations;
    private readonly ICoreShanheMuluCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly ICoreShanheMuluCoreRecoveryReplacementOperations _coreRecoveryReplacementOperations;
    private readonly ICoreShanheMuluCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreShanheMuluKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess _coreActualRoundProgressRoundNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineShanheMuluRuntime(ICoreShanheMuluCardZonesOperations cardZonesOperations, ICoreShanheMuluContentRegistryOperations contentRegistryOperations, ICoreShanheMuluResolutionStackOperations resolutionStackOperations, ICoreShanheMuluCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreShanheMuluCoreCardAttackStateOperations coreCardAttackStateOperations, ICoreShanheMuluCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICoreShanheMuluCoreCharacterStateProgramsOperations coreCharacterStateProgramsOperations, ICoreShanheMuluCoreHpChangeProgramsOperations coreHpChangeProgramsOperations, ICoreShanheMuluCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreShanheMuluCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreShanheMuluCoreProgramLifeRestorationOperations coreProgramLifeRestorationOperations, ICoreShanheMuluCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreShanheMuluCoreRuntimeOperations coreRuntimeOperations, ICoreShanheMuluCoreSkillOwnershipWindowsOperations coreSkillOwnershipWindowsOperations, ICoreShanheMuluCoreSkillProgramsOperations coreSkillProgramsOperations, ICoreShanheMuluCoreTeamActivitiesOperations coreTeamActivitiesOperations, ICoreShanheMuluCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, ICoreShanheMuluCoreRecoveryReplacementOperations coreRecoveryReplacementOperations, ICoreShanheMuluCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreShanheMuluKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess coreActualRoundProgressRoundNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardAttackStateOperations = coreCardAttackStateOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreCharacterStateProgramsOperations = coreCharacterStateProgramsOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifeRestorationOperations = coreProgramLifeRestorationOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillOwnershipWindowsOperations = coreSkillOwnershipWindowsOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreTeamActivitiesOperations = coreTeamActivitiesOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._coreRecoveryReplacementOperations = coreRecoveryReplacementOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualRoundProgressRoundNumberAccess = coreActualRoundProgressRoundNumberAccess;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
