#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineSourceHuojieRuntime
{
    private readonly ICoreSourceHuojieCardZonesOperations _cardZonesOperations;
    private readonly ICoreSourceHuojieResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreSourceHuojieCoreAdvancedLifecycleOperations _coreAdvancedLifecycleOperations;
    private readonly ICoreSourceHuojieCoreProgramAtomicCardMovesOperations _coreProgramAtomicCardMovesOperations;
    private readonly ICoreSourceHuojieCoreProgramAttackAttemptOperations _coreProgramAttackAttemptOperations;
    private readonly ICoreSourceHuojieCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreSourceHuojieCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreSourceHuojieCorePublicPileSourcesOperations _corePublicPileSourcesOperations;
    private readonly ICoreSourceHuojieCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreSourceHuojieCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICoreSourceHuojieCoreSourceBeigongOperations _coreSourceBeigongOperations;
    private readonly ICoreSourceHuojieCoreNativeJudgmentOperations _coreNativeJudgmentOperations;
    private readonly ICoreSourceHuojieCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly ICoreSourceHuojieCoreResolutionIdentityBoundaryOperations _coreResolutionIdentityBoundaryOperations;
    private readonly ICoreSourceHuojieKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess _coreCardMovementJournalCardMovementsMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineSourceHuojieRuntime(ICoreSourceHuojieCardZonesOperations cardZonesOperations, ICoreSourceHuojieResolutionStackOperations resolutionStackOperations, ICoreSourceHuojieCoreAdvancedLifecycleOperations coreAdvancedLifecycleOperations, ICoreSourceHuojieCoreProgramAtomicCardMovesOperations coreProgramAtomicCardMovesOperations, ICoreSourceHuojieCoreProgramAttackAttemptOperations coreProgramAttackAttemptOperations, ICoreSourceHuojieCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreSourceHuojieCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreSourceHuojieCorePublicPileSourcesOperations corePublicPileSourcesOperations, ICoreSourceHuojieCoreRuntimeOperations coreRuntimeOperations, ICoreSourceHuojieCoreSkillProgramsOperations coreSkillProgramsOperations, ICoreSourceHuojieCoreSourceBeigongOperations coreSourceBeigongOperations, ICoreSourceHuojieCoreNativeJudgmentOperations coreNativeJudgmentOperations, ICoreSourceHuojieCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, ICoreSourceHuojieCoreResolutionIdentityBoundaryOperations coreResolutionIdentityBoundaryOperations, ICoreSourceHuojieKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess coreCardMovementJournalCardMovementsMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAdvancedLifecycleOperations = coreAdvancedLifecycleOperations;
        this._coreProgramAtomicCardMovesOperations = coreProgramAtomicCardMovesOperations;
        this._coreProgramAttackAttemptOperations = coreProgramAttackAttemptOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._corePublicPileSourcesOperations = corePublicPileSourcesOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreSourceBeigongOperations = coreSourceBeigongOperations;
        this._coreNativeJudgmentOperations = coreNativeJudgmentOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._coreResolutionIdentityBoundaryOperations = coreResolutionIdentityBoundaryOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreCardMovementJournalCardMovementsMovementSequenceAccess = coreCardMovementJournalCardMovementsMovementSequenceAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
