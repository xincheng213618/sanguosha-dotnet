#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicConversionsRuntime))]
internal sealed class CoreProgramSlashRequestCoreChainedStateBasicConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSlashRequestCoreChainedStateBasicConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicConversionsRuntime> _targetFactory = targetFactory;
    public bool HasChainedStateBasicProgramSlash(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<int> targets) => this._targetFactory().HasChainedStateBasicProgramSlash(actor, targets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CoreProgramSlashRequestCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSlashRequestCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PromptId CreatePromptId() => this._targetFactory().CreatePromptId();
    public void ClearPendingDecision() => this._targetFactory().ClearPendingDecision();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicOffersRuntime))]
internal sealed class CoreProgramSlashRequestCoreDrawFundedDistinctBasicOffersAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicOffersRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSlashRequestCoreDrawFundedDistinctBasicOffersOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicOffersRuntime> _targetFactory = targetFactory;
    public bool HasDrawFundedDistinctBasicProgramSlash(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<int> targets) => this._targetFactory().HasDrawFundedDistinctBasicProgramSlash(actor, targets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramCardLegalityRuntime))]
internal sealed class CoreProgramSlashRequestCoreProgramCardLegalityAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramCardLegalityRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSlashRequestCoreProgramCardLegalityOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramCardLegalityRuntime> _targetFactory = targetFactory;
    public bool IsProgramEntityPhysicalRestricted(int actorSeat, int cardId) => this._targetFactory().IsProgramEntityPhysicalRestricted(actorSeat, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterChengJiChengCuiOlRuntime))]
internal sealed class CoreProgramSlashRequestCharacterChengJiChengCuiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengJiChengCuiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSlashRequestCharacterChengJiChengCuiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengJiChengCuiOlRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> TousuiProgramSlashChoices(global::CardGame.Core.ProgramSkillFrame f, int actorSeat, global::System.Collections.Generic.IReadOnlyList<int> targets, string? resultBind = default) => this._targetFactory().TousuiProgramSlashChoices(f, actorSeat, targets, resultBind);
    public bool HasTousuiProgramSlash(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<int> targets) => this._targetFactory().HasTousuiProgramSlash(actor, targets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime))]
internal sealed class CoreProgramSlashRequestCharacterMaXiuMaTieOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSlashRequestCharacterMaXiuMaTieOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> KenshangProgramSlashChoices(global::CardGame.Core.ProgramSkillFrame f, int actorSeat, global::System.Collections.Generic.IReadOnlyList<int> targets, string? resultBind = default) => this._targetFactory().KenshangProgramSlashChoices(f, actorSeat, targets, resultBind);
    public bool HasKenshangProgramSlash(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<int> targets) => this._targetFactory().HasKenshangProgramSlash(actor, targets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterPangJiOlRuntime))]
internal sealed class CoreProgramSlashRequestCharacterPangJiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangJiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSlashRequestCharacterPangJiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangJiOlRuntime> _targetFactory = targetFactory;
    public bool IsBiguoCategoryForbidden(int seat, global::CardGame.Core.CardKind effectiveKind) => this._targetFactory().IsBiguoCategoryForbidden(seat, effectiveKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreProgramSlashRequestCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSlashRequestCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHand(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime))]
internal sealed class CoreProgramSlashRequestCharacterGuoTuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSlashRequestCharacterGuoTuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime> _targetFactory = targetFactory;
    public bool HasWeijieProgramSlash(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<int> targets) => this._targetFactory().HasWeijieProgramSlash(actor, targets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime))]
internal sealed class CoreProgramSlashRequestCharacterHuanShuJunOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSlashRequestCharacterHuanShuJunOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime> _targetFactory = targetFactory;
    public bool HasHuanpeiProgramSlash(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<int> targets) => this._targetFactory().HasHuanpeiProgramSlash(actor, targets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreProgramSlashRequestCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSlashRequestCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId) => this._targetFactory().GetActiveProgramFrame(frameId);
    public void CancelProgramBindingAndCleanup(global::CardGame.Core.ProgramSkillFrame frame, string reason) => this._targetFactory().CancelProgramBindingAndCleanup(frame, reason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreProgramSlashRequestCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSlashRequestCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId) => this._targetFactory().HasRuntimeSkillInstance(player, skillId, skillInstanceId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessProgramRequestsRuntime))]
internal sealed class CoreProgramSlashRequestCoreRoundOnceCardlessProgramRequestsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessProgramRequestsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSlashRequestCoreRoundOnceCardlessProgramRequestsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessProgramRequestsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> RoundOnceCardlessProgramSlashChoices(global::CardGame.Core.ProgramSkillFrame frame, int actorSeat, global::System.Collections.Generic.IReadOnlyList<int> targets, string? resultBind = default) => this._targetFactory().RoundOnceCardlessProgramSlashChoices(frame, actorSeat, targets, resultBind);
    public bool HasRoundOnceCardlessProgramSlash(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<int> targets) => this._targetFactory().HasRoundOnceCardlessProgramSlash(actor, targets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime))]
internal sealed class CoreProgramSlashRequestCoreNativeSlashUseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSlashRequestCoreNativeSlashUseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime> _targetFactory = targetFactory;
    public void ResolveSlashCore(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card slash, global::CardGame.Core.CardKind playedCardKind, int physicalOwnerSeat, global::CardGame.Core.NativeOwners.FactionCardRequestHandle? factionRequest = default, global::CardGame.Core.NativeOwners.BorrowedSwordHandle? borrowedSword = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, bool countsTowardSlashLimit = true, bool usesZhuqueFan = false, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default, long? programSkillCardUseFrameId = default, global::CardGame.Core.CardLocation? physicalSourceLocation = default) => this._targetFactory().ResolveSlashCore(source, target, slash, playedCardKind, physicalOwnerSeat, factionRequest, borrowedSword, physicalCards, countsTowardSlashLimit, usesZhuqueFan, conversionSource, additionalConversionSources, programSkillCardUseFrameId, physicalSourceLocation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreProgramSlashRequestCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSlashRequestCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void AdvanceRuntimeProgram(long frameId) => this._targetFactory().AdvanceRuntimeProgram(frameId);
    public void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeTop(next);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuZhangOlRuntime))]
internal sealed class CoreProgramSlashRequestCharacterLiuZhangOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuZhangOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSlashRequestCharacterLiuZhangOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuZhangOlRuntime> _targetFactory = targetFactory;
    public bool HasZonghuProgramSlash(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<int> targets) => this._targetFactory().HasZonghuProgramSlash(actor, targets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicsRuntime))]
internal sealed class CoreProgramSlashRequestCoreRequestedDeckBasicsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSlashRequestCoreRequestedDeckBasicsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> SelectedRequestedDeckBasicCards(global::CardGame.Core.CharacterState actor) => this._targetFactory().SelectedRequestedDeckBasicCards(actor);
    public bool HasRequestedDeckBasicSource(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind required) => this._targetFactory().HasRequestedDeckBasicSource(actor, required);
    public bool IsSelectedRequestedDeckBasicMaterial(int actor, int id) => this._targetFactory().IsSelectedRequestedDeckBasicMaterial(actor, id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTurnHandCategoryRestrictionsRuntime))]
internal sealed class CoreProgramSlashRequestCoreTurnHandCategoryRestrictionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnHandCategoryRestrictionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSlashRequestCoreTurnHandCategoryRestrictionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnHandCategoryRestrictionsRuntime> _targetFactory = targetFactory;
    public bool IsTurnHandCategoryRestricted(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().IsTurnHandCategoryRestricted(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundDistinctBasicRequestsRuntime))]
internal sealed class CoreProgramSlashRequestCoreRoundDistinctBasicRequestsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundDistinctBasicRequestsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSlashRequestCoreRoundDistinctBasicRequestsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundDistinctBasicRequestsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> RoundDistinctBasicRequestedSlashChoices(global::CardGame.Core.ProgramSkillFrame frame, int actorSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, string? resultBind = default) => this._targetFactory().RoundDistinctBasicRequestedSlashChoices(frame, actorSeat, targetSeats, resultBind);
    public global::CardGame.Core.ProgramMultiCardViewAsSelection RequireRoundDistinctBasicRequestedSlash(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.PendingDecision decision, global::CardGame.Core.PromptChoice selected, int actorSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, string? resultBind = default) => this._targetFactory().RequireRoundDistinctBasicRequestedSlash(frame, decision, selected, actorSeat, targetSeats, resultBind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreProgramSlashRequestCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSlashRequestCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileOriginalCardsRuntime))]
internal sealed class CoreProgramSlashRequestCoreNativePublicPileOriginalCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileOriginalCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSlashRequestCoreNativePublicPileOriginalCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileOriginalCardsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> NativePublicPileSlashUseCards(global::CardGame.Core.CharacterState provider) => this._targetFactory().NativePublicPileSlashUseCards(provider);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime))]
internal sealed class CoreProgramSlashRequestCoreNativeRuleQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSlashRequestCoreNativeRuleQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetCombatDistance(int sourceSeat, int targetSeat) => this._targetFactory().GetCombatDistance(sourceSeat, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuCheErAnniversaryRuntime))]
internal sealed class CoreProgramSlashRequestCharacterHuCheErAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuCheErAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSlashRequestCharacterHuCheErAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuCheErAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HuCheErSlashBan(int actor, global::CardGame.Core.CardKind kind) => this._targetFactory().HuCheErSlashBan(actor, kind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuFengBoundaryRuntime))]
internal sealed class CoreProgramSlashRequestCharacterLiuFengBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuFengBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSlashRequestCharacterLiuFengBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuFengBoundaryRuntime> _targetFactory = targetFactory;
    public bool HasBoundaryLiuFengProgramSlash(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<int> targets) => this._targetFactory().HasBoundaryLiuFengProgramSlash(actor, targets);
}
