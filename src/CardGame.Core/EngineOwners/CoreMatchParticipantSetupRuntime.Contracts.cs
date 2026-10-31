#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreMatchParticipantSetupCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreMatchParticipantSetupCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreMatchParticipantSetupCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICoreMatchParticipantSetupCoreModeSetupOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ApplyModeSetupPlan(global::CardGame.Core.ModeSetupPlan plan);
    global::CardGame.Core.IMatchModeSetupFeature ModeSetup { get; }

    global::System.Collections.Generic.IReadOnlyList<(global::CardGame.Core.GeneralDefinition General, string CandidateId)> GetModeGeneralOffers(global::CardGame.Core.CharacterState player, bool fullRoster);
    void ApplyModeSelectedGeneral(global::CardGame.Core.CharacterState player, global::CardGame.Core.GeneralDefinition selected);
}

internal interface ICoreMatchParticipantSetupCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void InitializeDualGeneralHealth();
}

internal interface ICoreMatchParticipantSetupCoreModeRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ModePlayerLife[] CaptureModeLife();
    global::CardGame.Core.IMatchModeSetupPresentation ModeSetupPresentation { get; }
}

internal interface ICoreMatchParticipantSetupContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreMatchParticipantSetupKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
    void AdvanceRulesAndPublishState();
}

internal interface ICoreMatchParticipantSetupCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool UsesFormalWoodenOx { get; }

    bool IsNationalWarMode { get; }

    bool IsTeamMode { get; }

    bool IsDualGeneralMode { get; }
}

internal interface ICoreMatchParticipantSetupCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreMatchParticipantSetupCoreMatchSetupDefinitionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RequiresGodFactionSelection(global::CardGame.Core.CharacterState player);
    string ChooseAutomaticGodFaction(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<string> GetModeTeamIds();
}

internal interface ICoreMatchParticipantSetupCorePlayerViewProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICoreMatchParticipantSetupCorePhysicalDrawFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default);
}

internal interface ICoreMatchParticipantSetupCoreConversionStatesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void InitializeStructuredConversionSkills();
}

internal interface ICoreMatchParticipantSetupCharacterDaQiaoXiaoQiaoHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BindHappyQiaoHealth(global::CardGame.Core.CharacterState player);
}

internal interface ICoreMatchParticipantSetupCoreNativeCoreInvariantsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertCoreInvariants();
}

internal interface ICoreMatchParticipantSetupModeRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ModeRuleSet _modeRules { get; }
}

internal interface ICoreMatchParticipantSetupModeRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.MatchModeRuntime _modeRuntime { get; }
}

internal interface ICoreMatchParticipantSetupCoreOpeningCustomizationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RequestOpeningHandConfirmation();
}

internal interface ICoreMatchParticipantSetupCoreInitialHandModifiersOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetInitialHandSize(global::CardGame.Core.CharacterState player);
}

internal interface ICoreMatchParticipantSetupRandomOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.DeterministicRandom _random { get; }
}

internal interface ICoreMatchParticipantSetupCoreIdentityConcealmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool GeneralHasIdentityConcealment(global::CardGame.Core.GeneralDefinition general);
    void InitializeSelectedIdentityConcealment(global::CardGame.Core.CharacterState player);
}

internal interface ICoreMatchParticipantSetupCoreModeSetupEquipmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PrepareModeSelectedEquipmentAndDeck();
}

internal interface ICoreMatchParticipantSetupCoreNativeAiDecisionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddGeneralThought(global::CardGame.Core.AiGeneralThought thought);
}

internal interface ICoreMatchParticipantSetupCoreMatchPresentationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string GetPublicGeneralName(global::CardGame.Core.CharacterState player);
}
