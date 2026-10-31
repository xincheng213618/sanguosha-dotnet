#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineCommittedResponseUsesRuntime
{
    private readonly ICoreCommittedResponseUsesCharacterCaoChunOlOperations _characterCaoChunOlOperations;
    private readonly ICoreCommittedResponseUsesCharacterSunHaoOlOperations _characterSunHaoOlOperations;
    private readonly ICoreCommittedResponseUsesCharacterZhouYuOlMouOperations _characterZhouYuOlMouOperations;
    private readonly ICoreCommittedResponseUsesCoreCardActionsOperations _coreCardActionsOperations;
    private readonly ICoreCommittedResponseUsesCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICoreCommittedResponseUsesCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreCommittedResponseUsesCoreResponseUseCompletionOperations _coreResponseUseCompletionOperations;
    private readonly ICoreCommittedResponseUsesCoreNativeCardContinuationCursorsOperations _coreNativeCardContinuationCursorsOperations;
    private readonly ICoreCommittedResponseUsesCoreNativeDodgeOperations _coreNativeDodgeOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineCommittedResponseUsesRuntime(ICoreCommittedResponseUsesCharacterCaoChunOlOperations characterCaoChunOlOperations, ICoreCommittedResponseUsesCharacterSunHaoOlOperations characterSunHaoOlOperations, ICoreCommittedResponseUsesCharacterZhouYuOlMouOperations characterZhouYuOlMouOperations, ICoreCommittedResponseUsesCoreCardActionsOperations coreCardActionsOperations, ICoreCommittedResponseUsesCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICoreCommittedResponseUsesCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreCommittedResponseUsesCoreResponseUseCompletionOperations coreResponseUseCompletionOperations, ICoreCommittedResponseUsesCoreNativeCardContinuationCursorsOperations coreNativeCardContinuationCursorsOperations, ICoreCommittedResponseUsesCoreNativeDodgeOperations coreNativeDodgeOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterCaoChunOlOperations = characterCaoChunOlOperations;
        this._characterSunHaoOlOperations = characterSunHaoOlOperations;
        this._characterZhouYuOlMouOperations = characterZhouYuOlMouOperations;
        this._coreCardActionsOperations = coreCardActionsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreResponseUseCompletionOperations = coreResponseUseCompletionOperations;
        this._coreNativeCardContinuationCursorsOperations = coreNativeCardContinuationCursorsOperations;
        this._coreNativeDodgeOperations = coreNativeDodgeOperations;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
