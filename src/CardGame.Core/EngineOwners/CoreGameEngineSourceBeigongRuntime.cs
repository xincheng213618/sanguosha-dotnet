#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineSourceBeigongRuntime : global::CardGame.Core.ISourceBeigongHost
{
    private readonly ICoreSourceBeigongResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreSourceBeigongCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICoreSourceBeigongCoreCharacterStateProgramsOperations _coreCharacterStateProgramsOperations;
    private readonly ICoreSourceBeigongCoreHpChangeProgramsOperations _coreHpChangeProgramsOperations;
    private readonly ICoreSourceBeigongCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreSourceBeigongCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreSourceBeigongCorePublicPileSourcesOperations _corePublicPileSourcesOperations;
    private readonly ICoreSourceBeigongCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreSourceBeigongCoreSkillOwnershipWindowsOperations _coreSkillOwnershipWindowsOperations;
    private readonly ICoreSourceBeigongCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICoreSourceBeigongCoreSourceDingxiOperations _coreSourceDingxiOperations;
    private readonly ICoreSourceBeigongCoreSourceHuojieOperations _coreSourceHuojieOperations;
    private readonly ICoreSourceBeigongCoreSourceXingxiaOperations _coreSourceXingxiaOperations;
    private readonly ICoreSourceBeigongCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICoreSourceBeigongCoreRecoveryReplacementOperations _coreRecoveryReplacementOperations;
    private readonly ICoreSourceBeigongCoreTeamActivityProgramsOperations _coreTeamActivityProgramsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess _coreActualRoundProgressRoundNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess _coreCardMovementJournalMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineSourceBeigongRuntime(ICoreSourceBeigongResolutionStackOperations resolutionStackOperations, ICoreSourceBeigongCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICoreSourceBeigongCoreCharacterStateProgramsOperations coreCharacterStateProgramsOperations, ICoreSourceBeigongCoreHpChangeProgramsOperations coreHpChangeProgramsOperations, ICoreSourceBeigongCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreSourceBeigongCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreSourceBeigongCorePublicPileSourcesOperations corePublicPileSourcesOperations, ICoreSourceBeigongCoreRuntimeOperations coreRuntimeOperations, ICoreSourceBeigongCoreSkillOwnershipWindowsOperations coreSkillOwnershipWindowsOperations, ICoreSourceBeigongCoreSkillProgramsOperations coreSkillProgramsOperations, ICoreSourceBeigongCoreSourceDingxiOperations coreSourceDingxiOperations, ICoreSourceBeigongCoreSourceHuojieOperations coreSourceHuojieOperations, ICoreSourceBeigongCoreSourceXingxiaOperations coreSourceXingxiaOperations, ICoreSourceBeigongCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICoreSourceBeigongCoreRecoveryReplacementOperations coreRecoveryReplacementOperations, ICoreSourceBeigongCoreTeamActivityProgramsOperations coreTeamActivityProgramsOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess coreActualRoundProgressRoundNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess coreCardMovementJournalMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreCharacterStateProgramsOperations = coreCharacterStateProgramsOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._corePublicPileSourcesOperations = corePublicPileSourcesOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillOwnershipWindowsOperations = coreSkillOwnershipWindowsOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreSourceDingxiOperations = coreSourceDingxiOperations;
        this._coreSourceHuojieOperations = coreSourceHuojieOperations;
        this._coreSourceXingxiaOperations = coreSourceXingxiaOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreRecoveryReplacementOperations = coreRecoveryReplacementOperations;
        this._coreTeamActivityProgramsOperations = coreTeamActivityProgramsOperations;
        this._coreActualRoundProgressRoundNumberAccess = coreActualRoundProgressRoundNumberAccess;
        this._coreCardMovementJournalMovementSequenceAccess = coreCardMovementJournalMovementSequenceAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
