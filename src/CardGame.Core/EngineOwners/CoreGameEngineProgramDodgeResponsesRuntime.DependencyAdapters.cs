#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeFactionDefenseRuntime))]
internal sealed class CoreProgramDodgeResponsesCoreNativeFactionDefenseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeFactionDefenseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDodgeResponsesCoreNativeFactionDefenseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeFactionDefenseRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> CreateFactionDefenseResponseChoices(global::CardGame.Core.CharacterState provider, global::CardGame.Core.NativeOwners.FactionDefenseHandle pending, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> responseCards, bool includeBagua) => this._targetFactory().CreateFactionDefenseResponseChoices(provider, pending, responseCards, includeBagua);
    public void CompleteFactionDefenseResponse(global::CardGame.Core.NativeOwners.FactionDefenseHandle pending, global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card? selectedDodge, bool usedBagua, global::CardGame.Core.CardConversionSource? virtualResponseSource = default) => this._targetFactory().CompleteFactionDefenseResponse(pending, provider, selectedDodge, usedBagua, virtualResponseSource);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineIssuedPlayUsePoliciesRuntime))]
internal sealed class CoreProgramDodgeResponsesCoreIssuedPlayUsePoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIssuedPlayUsePoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDodgeResponsesCoreIssuedPlayUsePoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIssuedPlayUsePoliciesRuntime> _targetFactory = targetFactory;
    public bool HasIssuedPlayPhaseUseBan(int seat) => this._targetFactory().HasIssuedPlayPhaseUseBan(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime))]
internal sealed class CoreProgramDodgeResponsesCoreNativeAttackCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDodgeResponsesCoreNativeAttackCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime> _targetFactory = targetFactory;
    public void CompleteAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().CompleteAttack(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime))]
internal sealed class CoreProgramDodgeResponsesCoreResolutionIdentityBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDodgeResponsesCoreResolutionIdentityBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> _targetFactory = targetFactory;
    public long _resolutionSequence { get => this._targetFactory()._resolutionSequence; set => this._targetFactory()._resolutionSequence = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreProgramDodgeResponsesCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDodgeResponsesCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId) => this._targetFactory().GetActiveProgramFrame(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreProgramDodgeResponsesCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDodgeResponsesCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeResponseWindowsRuntime))]
internal sealed class CoreProgramDodgeResponsesCoreNativeResponseWindowsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeResponseWindowsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDodgeResponsesCoreNativeResponseWindowsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeResponseWindowsRuntime> _targetFactory = targetFactory;
    public void PopResponseWindow(long parentFrameId) => this._targetFactory().PopResponseWindow(parentFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterTaishiCiMobileGodRuntime))]
internal sealed class CoreProgramDodgeResponsesCharacterTaishiCiMobileGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTaishiCiMobileGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDodgeResponsesCharacterTaishiCiMobileGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterTaishiCiMobileGodRuntime> _targetFactory = targetFactory;
    public bool HasShenTaishiCiPoweiRange(int actor, int target) => this._targetFactory().HasShenTaishiCiPoweiRange(actor, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime))]
internal sealed class CoreProgramDodgeResponsesCoreCardActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDodgeResponsesCoreCardActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> _targetFactory = targetFactory;
    public bool TryBeginCardResponsePrograms(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState provider, int? requesterSeat, int opponentSeat, global::CardGame.Core.CardKind effectiveKind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardActionCost> costs, global::CardGame.Core.ProgramCardContinuation continuation, global::CardGame.Core.CardConversionSource? conversionSource = default, global::CardGame.Core.Suit? completedResponseUseSuit = default) => this._targetFactory().TryBeginCardResponsePrograms(attack, actor, provider, requesterSeat, opponentSeat, effectiveKind, costs, continuation, conversionSource, completedResponseUseSuit);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeResponseMaterialQueriesRuntime))]
internal sealed class CoreProgramDodgeResponsesCoreNativeResponseMaterialQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeResponseMaterialQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDodgeResponsesCoreNativeResponseMaterialQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeResponseMaterialQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetResponseCards(global::CardGame.Core.CharacterState responder, global::CardGame.Core.CardKind requiredCardKind) => this._targetFactory().GetResponseCards(responder, requiredCardKind);
    public global::CardGame.Core.CardKind GetEffectiveResponseKind(global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card responseCard, global::CardGame.Core.CardKind requiredCardKind) => this._targetFactory().GetEffectiveResponseKind(responder, responseCard, requiredCardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreProgramDodgeResponsesCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDodgeResponsesCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame) => this._targetFactory().PushRuntimeFrame(frame);
    public void AdvanceRuntimeProgram(long frameId) => this._targetFactory().AdvanceRuntimeProgram(frameId);
    public void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind) => this._targetFactory().PopResolutionFrame(frameId, expectedKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreProgramDodgeResponsesCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDodgeResponsesCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public string GetRuntimeSkillInstanceId(global::CardGame.Core.CharacterState player, string skillId) => this._targetFactory().GetRuntimeSkillInstanceId(player, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime))]
internal sealed class CoreProgramDodgeResponsesCoreAlternativeViewAsCostsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDodgeResponsesCoreAlternativeViewAsCostsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime> _targetFactory = targetFactory;
    public bool IsProgramResponseCardUse(global::CardGame.Core.CharacterState responder, global::CardGame.Core.CardKind effectiveKind) => this._targetFactory().IsProgramResponseCardUse(responder, effectiveKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreProgramDodgeResponsesCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDodgeResponsesCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime))]
internal sealed class CoreProgramDodgeResponsesCoreNativeCardUseFramesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDodgeResponsesCoreNativeCardUseFramesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime> _targetFactory = targetFactory;
    public void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step) => this._targetFactory().SetCardUseStep(frameId, step);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime))]
internal sealed class CoreProgramDodgeResponsesCoreDamageObserverCursorQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDodgeResponsesCoreDamageObserverCursorQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ResolutionFrame? DamageCursorEffectiveTop(bool includeNestedObservers = false) => this._targetFactory().DamageCursorEffectiveTop(includeNestedObservers);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime))]
internal sealed class CoreProgramDodgeResponsesCoreActiveNativeAttackQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDodgeResponsesCoreActiveNativeAttackQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get => this._targetFactory().ActiveCardAttack; set => this._targetFactory().ActiveCardAttack = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramPersistentCombatPoliciesRuntime))]
internal sealed class CoreProgramDodgeResponsesCoreProgramPersistentCombatPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramPersistentCombatPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDodgeResponsesCoreProgramPersistentCombatPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramPersistentCombatPoliciesRuntime> _targetFactory = targetFactory;
    public bool IsGameFactionAttackRangeTarget(int actorSeat, int targetSeat) => this._targetFactory().IsGameFactionAttackRangeTarget(actorSeat, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunYuanOlRuntime))]
internal sealed class CoreProgramDodgeResponsesCharacterSunYuanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunYuanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDodgeResponsesCharacterSunYuanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunYuanOlRuntime> _targetFactory = targetFactory;
    public bool HasSayingRoundRange(int actor, int target) => this._targetFactory().HasSayingRoundRange(actor, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime))]
internal sealed class CoreProgramDodgeResponsesCharacterXiaoQiaoMouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDodgeResponsesCharacterXiaoQiaoMouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MouXqJueyanResponseChoices(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, bool dying = false) => this._targetFactory().MouXqJueyanResponseChoices(actor, kind, dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CoreProgramDodgeResponsesCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDodgeResponsesCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public void ClearPendingDecision() => this._targetFactory().ClearPendingDecision();
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> CreateResponseChoices(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> responseCards, global::CardGame.Core.CardKind requiredCardKind, global::CardGame.Core.CardKind? incomingCard = default, global::System.Func<global::CardGame.Core.Card, global::CardGame.Core.CardKind>? effectiveCardKindSelector = default, bool includeBagua = false, bool includeFactionDefense = false, bool includeFactionSlash = false, global::CardGame.Core.CharacterState? responder = default) => this._targetFactory().CreateResponseChoices(responseCards, requiredCardKind, incomingCard, effectiveCardKindSelector, includeBagua, includeFactionDefense, includeFactionSlash, responder);
    public global::CardGame.Core.PromptId CreatePromptId() => this._targetFactory().CreatePromptId();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDodgeRuntime))]
internal sealed class CoreProgramDodgeResponsesCoreNativeDodgeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDodgeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDodgeResponsesCoreNativeDodgeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDodgeRuntime> _targetFactory = targetFactory;
    public void CompleteSuccessfulDodgeResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().CompleteSuccessfulDodgeResponse(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicConversionsRuntime))]
internal sealed class CoreProgramDodgeResponsesCoreChainedStateBasicConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDodgeResponsesCoreChainedStateBasicConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicConversionsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ChainedStateBasicDodgeChoices(global::CardGame.Core.CharacterState actor) => this._targetFactory().ChainedStateBasicDodgeChoices(actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeResponsesRuntime))]
internal sealed class CoreProgramDodgeResponsesCoreRoundOnceCardlessNativeResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDodgeResponsesCoreRoundOnceCardlessNativeResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeResponsesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> RoundOnceCardlessResponseChoices(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind) => this._targetFactory().RoundOnceCardlessResponseChoices(actor, kind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime))]
internal sealed class CoreProgramDodgeResponsesCoreNativeRuleQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDodgeResponsesCoreNativeRuleQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetCombatDistance(int sourceSeat, int targetSeat) => this._targetFactory().GetCombatDistance(sourceSeat, targetSeat);
    public int GetAttackRange(int sourceSeat) => this._targetFactory().GetAttackRange(sourceSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroResponsesRuntime))]
internal sealed class CoreProgramDodgeResponsesCoreTieredRoundZeroResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDodgeResponsesCoreTieredRoundZeroResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroResponsesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> TieredRoundZeroResponseChoices(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, bool dyingUse = false) => this._targetFactory().TieredRoundZeroResponseChoices(actor, kind, dyingUse);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime))]
internal sealed class CoreProgramDodgeResponsesCoreSkillProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDodgeResponsesCoreSkillProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SkillProgram> EnabledActivationPrograms(global::CardGame.Core.CharacterState player) => this._targetFactory().EnabledActivationPrograms(player);
    public global::CardGame.Core.SkillProgram GetEnabledSkillProgram(global::CardGame.Core.CharacterState player, string skillId) => this._targetFactory().GetEnabledSkillProgram(player, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime))]
internal sealed class CoreProgramDodgeResponsesCoreNativeEquipmentAndFactionQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDodgeResponsesCoreNativeEquipmentAndFactionQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime> _targetFactory = targetFactory;
    public bool HasBagua(global::CardGame.Core.CharacterState player) => this._targetFactory().HasBagua(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime))]
internal sealed class CoreProgramDodgeResponsesCoreNativeCardContinuationCursorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDodgeResponsesCoreNativeCardContinuationCursorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.FactionDefenseHandle? ActiveFactionDefense { get => this._targetFactory().ActiveFactionDefense; set => this._targetFactory().ActiveFactionDefense = value; }
    public global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get => this._targetFactory().ActiveGroupCard; set => this._targetFactory().ActiveGroupCard = value; }
}
