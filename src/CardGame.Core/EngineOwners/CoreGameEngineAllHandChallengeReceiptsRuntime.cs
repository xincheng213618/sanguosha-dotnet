#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineAllHandChallengeReceiptsRuntime
{
    private readonly ICoreAllHandChallengeReceiptsCardZonesOperations _cardZonesOperations;
    private readonly ICoreAllHandChallengeReceiptsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreAllHandChallengeReceiptsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreAllHandChallengeReceiptsCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreAllHandChallengeReceiptsCoreSequentialDiscardProgramsOperations _coreSequentialDiscardProgramsOperations;
    private readonly ICoreAllHandChallengeReceiptsCoreSequentialDiscardReceiptsOperations _coreSequentialDiscardReceiptsOperations;
    private readonly ICoreAllHandChallengeReceiptsCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess _coreActualRoundProgressRoundNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineSkillProgramsProgramPhaseUsesAccess _coreGameEngineSkillProgramsProgramPhaseUsesAccess;
    internal CoreGameEngineAllHandChallengeReceiptsRuntime(ICoreAllHandChallengeReceiptsCardZonesOperations cardZonesOperations, ICoreAllHandChallengeReceiptsContentRegistryOperations contentRegistryOperations, ICoreAllHandChallengeReceiptsResolutionStackOperations resolutionStackOperations, ICoreAllHandChallengeReceiptsCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreAllHandChallengeReceiptsCoreSequentialDiscardProgramsOperations coreSequentialDiscardProgramsOperations, ICoreAllHandChallengeReceiptsCoreSequentialDiscardReceiptsOperations coreSequentialDiscardReceiptsOperations, ICoreAllHandChallengeReceiptsCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess coreActualRoundProgressRoundNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineSkillProgramsProgramPhaseUsesAccess coreGameEngineSkillProgramsProgramPhaseUsesAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreSequentialDiscardProgramsOperations = coreSequentialDiscardProgramsOperations;
        this._coreSequentialDiscardReceiptsOperations = coreSequentialDiscardReceiptsOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreActualRoundProgressRoundNumberAccess = coreActualRoundProgressRoundNumberAccess;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreGameEngineSkillProgramsProgramPhaseUsesAccess = coreGameEngineSkillProgramsProgramPhaseUsesAccess;
    }
}
