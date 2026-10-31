#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchLogRuntime))]
internal sealed class CoreMatchParticipantSetupCoreMatchLogAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMatchParticipantSetupCoreMatchLogOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> _targetFactory = targetFactory;
    public void AddLog(string type, string message, int? actor = default, int? target = default) => this._targetFactory().AddLog(type, message, actor, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CoreMatchParticipantSetupCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMatchParticipantSetupCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PromptId CreatePromptId() => this._targetFactory().CreatePromptId();
    public void ClearPendingDecision() => this._targetFactory().ClearPendingDecision();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineModeSetupRuntime))]
internal sealed class CoreMatchParticipantSetupCoreModeSetupAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModeSetupRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMatchParticipantSetupCoreModeSetupOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModeSetupRuntime> _targetFactory = targetFactory;
    public void ApplyModeSetupPlan(global::CardGame.Core.ModeSetupPlan plan) => this._targetFactory().ApplyModeSetupPlan(plan);
    public global::CardGame.Core.IMatchModeSetupFeature ModeSetup { get => this._targetFactory().ModeSetup; }

    public global::System.Collections.Generic.IReadOnlyList<(global::CardGame.Core.GeneralDefinition General, string CandidateId)> GetModeGeneralOffers(global::CardGame.Core.CharacterState player, bool fullRoster) => this._targetFactory().GetModeGeneralOffers(player, fullRoster);
    public void ApplyModeSelectedGeneral(global::CardGame.Core.CharacterState player, global::CardGame.Core.GeneralDefinition selected) => this._targetFactory().ApplyModeSelectedGeneral(player, selected);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreMatchParticipantSetupCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMatchParticipantSetupCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public void InitializeDualGeneralHealth() => this._targetFactory().InitializeDualGeneralHealth();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineModeRulesRuntime))]
internal sealed class CoreMatchParticipantSetupCoreModeRulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModeRulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMatchParticipantSetupCoreModeRulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModeRulesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ModePlayerLife[] CaptureModeLife() => this._targetFactory().CaptureModeLife();
    public global::CardGame.Core.IMatchModeSetupPresentation ModeSetupPresentation { get => this._targetFactory().ModeSetupPresentation; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime))]
internal sealed class CoreMatchParticipantSetupCoreNativeDamageHandlesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMatchParticipantSetupCoreNativeDamageHandlesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> _targetFactory = targetFactory;
    public bool UsesFormalWoodenOx { get => this._targetFactory().UsesFormalWoodenOx; }
    public bool IsNationalWarMode { get => this._targetFactory().IsNationalWarMode; }
    public bool IsTeamMode { get => this._targetFactory().IsTeamMode; }
    public bool IsDualGeneralMode { get => this._targetFactory().IsDualGeneralMode; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreMatchParticipantSetupCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMatchParticipantSetupCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchSetupDefinitionsRuntime))]
internal sealed class CoreMatchParticipantSetupCoreMatchSetupDefinitionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchSetupDefinitionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMatchParticipantSetupCoreMatchSetupDefinitionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchSetupDefinitionsRuntime> _targetFactory = targetFactory;
    public bool RequiresGodFactionSelection(global::CardGame.Core.CharacterState player) => this._targetFactory().RequiresGodFactionSelection(player);
    public string ChooseAutomaticGodFaction(global::CardGame.Core.CharacterState player) => this._targetFactory().ChooseAutomaticGodFaction(player);
    public global::System.Collections.Generic.IReadOnlyList<string> GetModeTeamIds() => this._targetFactory().GetModeTeamIds();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime))]
internal sealed class CoreMatchParticipantSetupCorePlayerViewProjectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMatchParticipantSetupCorePlayerViewProjectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime))]
internal sealed class CoreMatchParticipantSetupCorePhysicalDrawFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMatchParticipantSetupCorePhysicalDrawFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default) => this._targetFactory().DrawCards(player, count, log, reason, directProducer);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineConversionStatesRuntime))]
internal sealed class CoreMatchParticipantSetupCoreConversionStatesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConversionStatesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMatchParticipantSetupCoreConversionStatesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConversionStatesRuntime> _targetFactory = targetFactory;
    public void InitializeStructuredConversionSkills() => this._targetFactory().InitializeStructuredConversionSkills();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime))]
internal sealed class CoreMatchParticipantSetupCharacterDaQiaoXiaoQiaoHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMatchParticipantSetupCharacterDaQiaoXiaoQiaoHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> _targetFactory = targetFactory;
    public void BindHappyQiaoHealth(global::CardGame.Core.CharacterState player) => this._targetFactory().BindHappyQiaoHealth(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCoreInvariantsRuntime))]
internal sealed class CoreMatchParticipantSetupCoreNativeCoreInvariantsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCoreInvariantsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMatchParticipantSetupCoreNativeCoreInvariantsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCoreInvariantsRuntime> _targetFactory = targetFactory;
    public void AssertCoreInvariants() => this._targetFactory().AssertCoreInvariants();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOpeningCustomizationRuntime))]
internal sealed class CoreMatchParticipantSetupCoreOpeningCustomizationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOpeningCustomizationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMatchParticipantSetupCoreOpeningCustomizationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOpeningCustomizationRuntime> _targetFactory = targetFactory;
    public void RequestOpeningHandConfirmation() => this._targetFactory().RequestOpeningHandConfirmation();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineInitialHandModifiersRuntime))]
internal sealed class CoreMatchParticipantSetupCoreInitialHandModifiersAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineInitialHandModifiersRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMatchParticipantSetupCoreInitialHandModifiersOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineInitialHandModifiersRuntime> _targetFactory = targetFactory;
    public int GetInitialHandSize(global::CardGame.Core.CharacterState player) => this._targetFactory().GetInitialHandSize(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineIdentityConcealmentRuntime))]
internal sealed class CoreMatchParticipantSetupCoreIdentityConcealmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIdentityConcealmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMatchParticipantSetupCoreIdentityConcealmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIdentityConcealmentRuntime> _targetFactory = targetFactory;
    public bool GeneralHasIdentityConcealment(global::CardGame.Core.GeneralDefinition general) => this._targetFactory().GeneralHasIdentityConcealment(general);
    public void InitializeSelectedIdentityConcealment(global::CardGame.Core.CharacterState player) => this._targetFactory().InitializeSelectedIdentityConcealment(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineModeSetupEquipmentRuntime))]
internal sealed class CoreMatchParticipantSetupCoreModeSetupEquipmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModeSetupEquipmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMatchParticipantSetupCoreModeSetupEquipmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModeSetupEquipmentRuntime> _targetFactory = targetFactory;
    public void PrepareModeSelectedEquipmentAndDeck() => this._targetFactory().PrepareModeSelectedEquipmentAndDeck();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime))]
internal sealed class CoreMatchParticipantSetupCoreNativeAiDecisionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMatchParticipantSetupCoreNativeAiDecisionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime> _targetFactory = targetFactory;
    public void AddGeneralThought(global::CardGame.Core.AiGeneralThought thought) => this._targetFactory().AddGeneralThought(thought);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchPresentationRuntime))]
internal sealed class CoreMatchParticipantSetupCoreMatchPresentationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchPresentationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMatchParticipantSetupCoreMatchPresentationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchPresentationRuntime> _targetFactory = targetFactory;
    public string GetPublicGeneralName(global::CardGame.Core.CharacterState player) => this._targetFactory().GetPublicGeneralName(player);
}
