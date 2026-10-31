#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhaoTongZhaoGuangAnniversaryRuntime))]
internal sealed class CoreExtendedViewAsCharacterZhaoTongZhaoGuangAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoTongZhaoGuangAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtendedViewAsCharacterZhaoTongZhaoGuangAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoTongZhaoGuangAnniversaryRuntime> _targetFactory = targetFactory;
    public bool IsAnniversaryYizanSelection(global::CardGame.Core.ProgramMultiCardViewAsSelection s) => this._targetFactory().IsAnniversaryYizanSelection(s);
    public bool TryResolveAnniversaryYizanDyingAlcohol(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => this._targetFactory().TryResolveAnniversaryYizanDyingAlcohol(owner, selection);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime))]
internal sealed class CoreExtendedViewAsCoreDyingCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtendedViewAsCoreDyingCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DyingFrame? ActiveDying { get => this._targetFactory().ActiveDying; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime))]
internal sealed class CoreExtendedViewAsCoreProgramCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtendedViewAsCoreProgramCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.IndexedSkillProgramInstance Source, global::CardGame.Core.SkillProgramCardPolicy Policy)> CardPolicies(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.CardKind? requiredKind = default, bool? eventTargetIsFemale = default, bool? eventSourceIsFemale = default) => this._targetFactory().CardPolicies(owner, kind, effectiveKind, requiredKind, eventTargetIsFemale, eventSourceIsFemale);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeResponseWindowsRuntime))]
internal sealed class CoreExtendedViewAsCoreNativeResponseWindowsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeResponseWindowsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtendedViewAsCoreNativeResponseWindowsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeResponseWindowsRuntime> _targetFactory = targetFactory;
    public void PopResponseWindow(long parentFrameId) => this._targetFactory().PopResponseWindow(parentFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterChengYuMouRuntime))]
internal sealed class CoreExtendedViewAsCharacterChengYuMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengYuMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtendedViewAsCharacterChengYuMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengYuMouRuntime> _targetFactory = targetFactory;
    public bool HasMouCyMatchingName(int actor, global::CardGame.Core.CardKind kind) => this._targetFactory().HasMouCyMatchingName(actor, kind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime))]
internal sealed class CoreExtendedViewAsCoreNativeCardContinuationCursorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtendedViewAsCoreNativeCardContinuationCursorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.FactionDefenseHandle? ActiveFactionDefense { get => this._targetFactory().ActiveFactionDefense; set => this._targetFactory().ActiveFactionDefense = value; }
    public global::CardGame.Core.NullificationWindowFrame? ActiveNullificationWindow { get => this._targetFactory().ActiveNullificationWindow; }
    public global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get => this._targetFactory().ActiveGroupCard; set => this._targetFactory().ActiveGroupCard = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CoreExtendedViewAsCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtendedViewAsCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public void ClearPendingDecision() => this._targetFactory().ClearPendingDecision();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime))]
internal sealed class CoreExtendedViewAsCoreNativeCardUseFramesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtendedViewAsCoreNativeCardUseFramesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime> _targetFactory = targetFactory;
    public void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step) => this._targetFactory().SetCardUseStep(frameId, step);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime))]
internal sealed class CoreExtendedViewAsCoreCardActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtendedViewAsCoreCardActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> _targetFactory = targetFactory;
    public bool TryBeginProgramCardWindow(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<int> opponents, global::CardGame.Core.ProgramCardContinuation? continuation, bool? cardUseCausedDamage = default, global::CardGame.Core.ProgramTrickContinuation? trickContinuation = default, global::CardGame.Core.ProgramSimpleCardContinuation? simpleContinuation = default, global::CardGame.Core.ProgramCompletedResponseReturn? completedResponseReturn = default, global::CardGame.Core.CaoChunHandUsePayment? caoChunPayment = default, bool caoChunDebtOnly = false) => this._targetFactory().TryBeginProgramCardWindow(attack, action, window, opponents, continuation, cardUseCausedDamage, trickContinuation, simpleContinuation, completedResponseReturn, caoChunPayment, caoChunDebtOnly);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime))]
internal sealed class CoreExtendedViewAsCoreNativeNullificationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtendedViewAsCoreNativeNullificationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> BuildNullificationCandidateSeats(int startAfterSeat) => this._targetFactory().BuildNullificationCandidateSeats(startAfterSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuBuMobileGodRuntime))]
internal sealed class CoreExtendedViewAsCharacterLuBuMobileGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuMobileGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtendedViewAsCharacterLuBuMobileGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuMobileGodRuntime> _targetFactory = targetFactory;
    public bool HasMobileWuqianArmorDisabled(int targetSeat) => this._targetFactory().HasMobileWuqianArmorDisabled(targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangZhongAnniversaryGodRuntime))]
internal sealed class CoreExtendedViewAsCharacterHuangZhongAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangZhongAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtendedViewAsCharacterHuangZhongAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangZhongAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryHzPendingInvalidation(int actor) => this._targetFactory().HasAnniversaryHzPendingInvalidation(actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXunXuAnniversaryRuntime))]
internal sealed class CoreExtendedViewAsCharacterXunXuAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXunXuAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtendedViewAsCharacterXunXuAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXunXuAnniversaryRuntime> _targetFactory = targetFactory;
    public bool IsAnniversaryXunXuBoqiaSource(global::CardGame.Core.CardConversionSource? s) => this._targetFactory().IsAnniversaryXunXuBoqiaSource(s);
    public bool TryResolveAnniversaryXunXuDyingAlcohol(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection s) => this._targetFactory().TryResolveAnniversaryXunXuDyingAlcohol(owner, s);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCommittedResponseUsesRuntime))]
internal sealed class CoreExtendedViewAsCoreCommittedResponseUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCommittedResponseUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtendedViewAsCoreCommittedResponseUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCommittedResponseUsesRuntime> _targetFactory = targetFactory;
    public bool TryBeginCommittedResponseUsePrograms(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => this._targetFactory().TryBeginCommittedResponseUsePrograms(attack, action, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundDistinctBasicPlayRuntime))]
internal sealed class CoreExtendedViewAsCoreRoundDistinctBasicPlayAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundDistinctBasicPlayRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtendedViewAsCoreRoundDistinctBasicPlayOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundDistinctBasicPlayRuntime> _targetFactory = targetFactory;
    public bool TryResolveRoundDistinctBasicDyingResponse(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => this._targetFactory().TryResolveRoundDistinctBasicDyingResponse(owner, selection);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime))]
internal sealed class CoreExtendedViewAsCharacterGuanHaiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtendedViewAsCharacterGuanHaiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime> _targetFactory = targetFactory;
    public bool IsXieweiSelection(global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => this._targetFactory().IsXieweiSelection(selection);
    public bool TryCompleteXieweiFactionDefense(global::CardGame.Core.NativeOwners.FactionDefenseHandle pending, global::CardGame.Core.CharacterState provider, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => this._targetFactory().TryCompleteXieweiFactionDefense(pending, provider, selection);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActionQueriesRuntime))]
internal sealed class CoreExtendedViewAsCoreActionQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtendedViewAsCoreActionQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> GetProgramViewAsConversions(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind outputKind, bool forResponse, bool dyingUse = false) => this._targetFactory().GetProgramViewAsConversions(owner, card, outputKind, forResponse, dyingUse);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime))]
internal sealed class CoreExtendedViewAsCoreDeckEquipmentProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtendedViewAsCoreDeckEquipmentProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime> _targetFactory = targetFactory;
    public bool IsShieldedRescueSelection(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => this._targetFactory().IsShieldedRescueSelection(owner, selection);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime))]
internal sealed class CoreExtendedViewAsCorePlayCardHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtendedViewAsCorePlayCardHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramViewAs? ViewAsRule(global::CardGame.Core.CardConversionSource source) => this._targetFactory().ViewAsRule(source);
    public global::CardGame.Core.Card ApplyProgramUseAppearance(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? source) => this._targetFactory().ApplyProgramUseAppearance(owner, card, source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNamedAmbushInvalidationRuntime))]
internal sealed class CoreExtendedViewAsCoreNamedAmbushInvalidationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNamedAmbushInvalidationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtendedViewAsCoreNamedAmbushInvalidationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNamedAmbushInvalidationRuntime> _targetFactory = targetFactory;
    public bool HasEnabledMatchingNamedAmbush(int actor, global::CardGame.Core.CardKind kind) => this._targetFactory().HasEnabledMatchingNamedAmbush(actor, kind);
    public global::CardGame.Core.NullificationWindowFrame PrepareNamedAmbushCounterspell(global::CardGame.Core.NullificationWindowFrame pending, int responder, int cardId) => this._targetFactory().PrepareNamedAmbushCounterspell(pending, responder, cardId);
    public void BindNamedAmbushCounterspell(long id, global::CardGame.Core.CardActionContext action) => this._targetFactory().BindNamedAmbushCounterspell(id, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreExtendedViewAsCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtendedViewAsCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime))]
internal sealed class CoreExtendedViewAsCoreCardConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtendedViewAsCoreCardConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramMultiCardViewAsSelection> GetProgramMultiCardViewAsSelections(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind outputKind, bool forResponse, bool ignoreSuitUseProhibition = false, int? requestedUseTarget = default) => this._targetFactory().GetProgramMultiCardViewAsSelections(owner, outputKind, forResponse, ignoreSuitUseProhibition, requestedUseTarget);
    public string ProgramConversionName(global::CardGame.Core.CardConversionSource source) => this._targetFactory().ProgramConversionName(source);
    public global::CardGame.Core.ProgramMultiCardViewAsSelection? FindProgramMultiCardViewAsSelection(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.CardKind outputKind, bool forResponse, global::CardGame.Core.CardConversionSource source) => this._targetFactory().FindProgramMultiCardViewAsSelection(owner, cardIds, outputKind, forResponse, source);
    public global::CardGame.Core.CardActionContext MoveProgramMultiCardResponse(global::CardGame.Core.CharacterState responder, global::CardGame.Core.ProgramMultiCardViewAsSelection selection, long resolutionId, int responseTargetSeat, int? actorSeat = default) => this._targetFactory().MoveProgramMultiCardResponse(responder, selection, resolutionId, responseTargetSeat, actorSeat);
    public void FinishProgramMultiCardResponse(global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => this._targetFactory().FinishProgramMultiCardResponse(selection);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeNullificationFramesRuntime))]
internal sealed class CoreExtendedViewAsCoreNativeNullificationFramesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationFramesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtendedViewAsCoreNativeNullificationFramesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationFramesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NullificationWindowFrame ReplaceNullificationWindowFrame(global::CardGame.Core.NullificationWindowFrame next) => this._targetFactory().ReplaceNullificationWindowFrame(next);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShenwuExpandedPoliciesRuntime))]
internal sealed class CoreExtendedViewAsCoreShenwuExpandedPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShenwuExpandedPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtendedViewAsCoreShenwuExpandedPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShenwuExpandedPoliciesRuntime> _targetFactory = targetFactory;
    public bool ShenwuZhaohuoDisablesArmor(int targetSeat) => this._targetFactory().ShenwuZhaohuoDisablesArmor(targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterKongShuOlRuntime))]
internal sealed class CoreExtendedViewAsCharacterKongShuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterKongShuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtendedViewAsCharacterKongShuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterKongShuOlRuntime> _targetFactory = targetFactory;
    public bool TryResolveLeiluanDyingResponse(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => this._targetFactory().TryResolveLeiluanDyingResponse(owner, selection);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchSetupDefinitionsRuntime))]
internal sealed class CoreExtendedViewAsCoreMatchSetupDefinitionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchSetupDefinitionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtendedViewAsCoreMatchSetupDefinitionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchSetupDefinitionsRuntime> _targetFactory = targetFactory;
    public string? GetEffectiveFactionId(global::CardGame.Core.CharacterState player) => this._targetFactory().GetEffectiveFactionId(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime))]
internal sealed class CoreExtendedViewAsCoreActiveNativeAttackQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtendedViewAsCoreActiveNativeAttackQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get => this._targetFactory().ActiveCardAttack; set => this._targetFactory().ActiveCardAttack = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime))]
internal sealed class CoreExtendedViewAsCoreSkillProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtendedViewAsCoreSkillProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgram GetEnabledSkillProgram(global::CardGame.Core.CharacterState player, string skillId) => this._targetFactory().GetEnabledSkillProgram(player, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineUnrespondableCounterspellsRuntime))]
internal sealed class CoreExtendedViewAsCoreUnrespondableCounterspellsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnrespondableCounterspellsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtendedViewAsCoreUnrespondableCounterspellsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnrespondableCounterspellsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardConversionSource? FreezeUnrespondableCounterspellSource(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> physicalCardIds) => this._targetFactory().FreezeUnrespondableCounterspellSource(owner, physicalCardIds);
    public bool TryBeginPolicyCounterspellPayment(global::CardGame.Core.NullificationWindowFrame window, global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardConversionSource? frozenSource, global::System.Collections.Generic.IReadOnlyList<int> frozenMaterialIds) => this._targetFactory().TryBeginPolicyCounterspellPayment(window, action, frozenSource, frozenMaterialIds);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangJinzhiRuntime))]
internal sealed class CoreExtendedViewAsCoreWushuangJinzhiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangJinzhiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtendedViewAsCoreWushuangJinzhiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangJinzhiRuntime> _targetFactory = targetFactory;
    public bool IsWushuangJinzhiSelection(global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => this._targetFactory().IsWushuangJinzhiSelection(selection);
    public bool TryResolveWushuangJinzhiDyingAlcohol(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => this._targetFactory().TryResolveWushuangJinzhiDyingAlcohol(owner, selection);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFieldEquipmentRuntime))]
internal sealed class CoreExtendedViewAsCoreFieldEquipmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFieldEquipmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtendedViewAsCoreFieldEquipmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFieldEquipmentRuntime> _targetFactory = targetFactory;
    public bool HasHappyWeiYanPotentialCancellation(int seat) => this._targetFactory().HasHappyWeiYanPotentialCancellation(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDodgeRuntime))]
internal sealed class CoreExtendedViewAsCoreNativeDodgeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDodgeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtendedViewAsCoreNativeDodgeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDodgeRuntime> _targetFactory = targetFactory;
    public void ContinueAcceptedCardResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => this._targetFactory().ContinueAcceptedCardResponse(attack, action, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoXiOlRuntime))]
internal sealed class CoreExtendedViewAsCharacterCaoXiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoXiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtendedViewAsCharacterCaoXiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoXiOlRuntime> _targetFactory = targetFactory;
    public void CaptureCaoXiNativeCounterspellOffset(global::CardGame.Core.NullificationWindowFrame pending, global::CardGame.Core.CardActionContext action, bool invalidated) => this._targetFactory().CaptureCaoXiNativeCounterspellOffset(pending, action, invalidated);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineResponseUseCompletionRuntime))]
internal sealed class CoreExtendedViewAsCoreResponseUseCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseUseCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtendedViewAsCoreResponseUseCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseUseCompletionRuntime> _targetFactory = targetFactory;
    public void ContinueNullificationAfterResponseUse(global::CardGame.Core.CardActionContext action) => this._targetFactory().ContinueNullificationAfterResponseUse(action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramAllHandBonusOperationRuntime))]
internal sealed class CoreExtendedViewAsCoreProgramAllHandBonusOperationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramAllHandBonusOperationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtendedViewAsCoreProgramAllHandBonusOperationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramAllHandBonusOperationRuntime> _targetFactory = targetFactory;
    public bool CanUsePeachToRescue(int responderSeat, int victimSeat) => this._targetFactory().CanUsePeachToRescue(responderSeat, victimSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeRecoveryCardsRuntime))]
internal sealed class CoreExtendedViewAsCoreNativeRecoveryCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRecoveryCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtendedViewAsCoreNativeRecoveryCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRecoveryCardsRuntime> _targetFactory = targetFactory;
    public void ResolveRecoveryCard(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card card, string cardName, global::CardGame.Core.CardKind? playedCardKind = default, int recoveryAmount = 1, global::System.Collections.Generic.IReadOnlyList<(string SkillId, string PolicyId)>? recoveryPolicySources = default, global::CardGame.Core.CardConversionSource? conversionSource = default, global::CardGame.Core.DyingResponseEvent? dyingResponse = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, global::CardGame.Core.DrawFundedDistinctBasicPayment? drawFundedPayment = default, global::CardGame.Core.HuanpeiPaidOrigin? huanpeiOrigin = default) => this._targetFactory().ResolveRecoveryCard(source, target, card, cardName, playedCardKind, recoveryAmount, recoveryPolicySources, conversionSource, dyingResponse, physicalCards, drawFundedPayment, huanpeiOrigin);
}
