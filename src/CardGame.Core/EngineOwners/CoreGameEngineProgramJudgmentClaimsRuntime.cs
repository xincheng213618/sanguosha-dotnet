#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramJudgmentClaimsRuntime
{
    private readonly ICoreProgramJudgmentClaimsCardZonesOperations _cardZonesOperations;
    private readonly ICoreProgramJudgmentClaimsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreProgramJudgmentClaimsCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICoreProgramJudgmentClaimsCoreJudgmentOutcomeClaimsOperations _coreJudgmentOutcomeClaimsOperations;
    private readonly ICoreProgramJudgmentClaimsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreProgramJudgmentClaimsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreProgramJudgmentClaimsCoreJudgmentCursorRuntimeOperations _coreJudgmentCursorRuntimeOperations;
    private readonly ICoreProgramJudgmentClaimsCoreNativeDamageHandlesOperations _coreNativeDamageHandlesOperations;
    private readonly ICoreProgramJudgmentClaimsCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly ICoreProgramJudgmentClaimsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess _coreCardMovementJournalMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramJudgmentClaimsRuntime(ICoreProgramJudgmentClaimsCardZonesOperations cardZonesOperations, ICoreProgramJudgmentClaimsResolutionStackOperations resolutionStackOperations, ICoreProgramJudgmentClaimsCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICoreProgramJudgmentClaimsCoreJudgmentOutcomeClaimsOperations coreJudgmentOutcomeClaimsOperations, ICoreProgramJudgmentClaimsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreProgramJudgmentClaimsCoreRuntimeOperations coreRuntimeOperations, ICoreProgramJudgmentClaimsCoreJudgmentCursorRuntimeOperations coreJudgmentCursorRuntimeOperations, ICoreProgramJudgmentClaimsCoreNativeDamageHandlesOperations coreNativeDamageHandlesOperations, ICoreProgramJudgmentClaimsCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, ICoreProgramJudgmentClaimsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess coreCardMovementJournalMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreJudgmentOutcomeClaimsOperations = coreJudgmentOutcomeClaimsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreJudgmentCursorRuntimeOperations = coreJudgmentCursorRuntimeOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreCardMovementJournalMovementSequenceAccess = coreCardMovementJournalMovementSequenceAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
