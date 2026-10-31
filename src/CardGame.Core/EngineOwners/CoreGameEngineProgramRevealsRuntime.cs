#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramRevealsRuntime
{
    private readonly ICoreProgramRevealsCharacterLuMengHappyGodOperations _characterLuMengHappyGodOperations;
    private readonly ICoreProgramRevealsCardZonesOperations _cardZonesOperations;
    private readonly ICoreProgramRevealsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreProgramRevealsRandomOperations _randomOperations;
    private readonly ICoreProgramRevealsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreProgramRevealsCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreProgramRevealsCoreProgramCardPaymentsOperations _coreProgramCardPaymentsOperations;
    private readonly ICoreProgramRevealsCoreProgramCategoryChallengesOperations _coreProgramCategoryChallengesOperations;
    private readonly ICoreProgramRevealsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreProgramRevealsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreProgramRevealsCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICoreProgramRevealsCoreNativeCardAppearanceOperations _coreNativeCardAppearanceOperations;
    private readonly ICoreProgramRevealsCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICoreProgramRevealsCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICoreProgramRevealsCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramRevealsRuntime(ICoreProgramRevealsCharacterLuMengHappyGodOperations characterLuMengHappyGodOperations, ICoreProgramRevealsCardZonesOperations cardZonesOperations, ICoreProgramRevealsContentRegistryOperations contentRegistryOperations, ICoreProgramRevealsRandomOperations randomOperations, ICoreProgramRevealsResolutionStackOperations resolutionStackOperations, ICoreProgramRevealsCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreProgramRevealsCoreProgramCardPaymentsOperations coreProgramCardPaymentsOperations, ICoreProgramRevealsCoreProgramCategoryChallengesOperations coreProgramCategoryChallengesOperations, ICoreProgramRevealsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreProgramRevealsCoreRuntimeOperations coreRuntimeOperations, ICoreProgramRevealsCoreMatchLogOperations coreMatchLogOperations, ICoreProgramRevealsCoreNativeCardAppearanceOperations coreNativeCardAppearanceOperations, ICoreProgramRevealsCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICoreProgramRevealsCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICoreProgramRevealsCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterLuMengHappyGodOperations = characterLuMengHappyGodOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._randomOperations = randomOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramCardPaymentsOperations = coreProgramCardPaymentsOperations;
        this._coreProgramCategoryChallengesOperations = coreProgramCategoryChallengesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativeCardAppearanceOperations = coreNativeCardAppearanceOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
