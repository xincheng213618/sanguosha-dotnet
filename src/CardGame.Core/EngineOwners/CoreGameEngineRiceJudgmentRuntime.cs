#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineRiceJudgmentRuntime
{
    private readonly ICoreRiceJudgmentCoreJudgmentReplacementProgramsOperations _coreJudgmentReplacementProgramsOperations;
    private readonly ICoreRiceJudgmentCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreRiceJudgmentCorePublicPileSourcesOperations _corePublicPileSourcesOperations;
    private readonly ICoreRiceJudgmentCoreRicePileOperations _coreRicePileOperations;
    private readonly ICoreRiceJudgmentCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreRiceJudgmentCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICoreRiceJudgmentCoreJudgmentCursorRuntimeOperations _coreJudgmentCursorRuntimeOperations;
    private readonly ICoreRiceJudgmentCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICoreRiceJudgmentCoreNativeCardAppearanceOperations _coreNativeCardAppearanceOperations;
    private readonly ICoreRiceJudgmentCoreNativeDamageHandlesOperations _coreNativeDamageHandlesOperations;
    private readonly ICoreRiceJudgmentKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess _coreActualTurnProgressTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess _coreCardMovementJournalMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineRiceJudgmentRuntime(ICoreRiceJudgmentCoreJudgmentReplacementProgramsOperations coreJudgmentReplacementProgramsOperations, ICoreRiceJudgmentCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreRiceJudgmentCorePublicPileSourcesOperations corePublicPileSourcesOperations, ICoreRiceJudgmentCoreRicePileOperations coreRicePileOperations, ICoreRiceJudgmentCoreRuntimeOperations coreRuntimeOperations, ICoreRiceJudgmentCoreSkillProgramsOperations coreSkillProgramsOperations, ICoreRiceJudgmentCoreJudgmentCursorRuntimeOperations coreJudgmentCursorRuntimeOperations, ICoreRiceJudgmentCoreMatchLogOperations coreMatchLogOperations, ICoreRiceJudgmentCoreNativeCardAppearanceOperations coreNativeCardAppearanceOperations, ICoreRiceJudgmentCoreNativeDamageHandlesOperations coreNativeDamageHandlesOperations, ICoreRiceJudgmentKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess coreActualTurnProgressTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess coreCardMovementJournalMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._coreJudgmentReplacementProgramsOperations = coreJudgmentReplacementProgramsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._corePublicPileSourcesOperations = corePublicPileSourcesOperations;
        this._coreRicePileOperations = coreRicePileOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreJudgmentCursorRuntimeOperations = coreJudgmentCursorRuntimeOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativeCardAppearanceOperations = coreNativeCardAppearanceOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressTurnNumberAccess = coreActualTurnProgressTurnNumberAccess;
        this._coreCardMovementJournalMovementSequenceAccess = coreCardMovementJournalMovementSequenceAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
