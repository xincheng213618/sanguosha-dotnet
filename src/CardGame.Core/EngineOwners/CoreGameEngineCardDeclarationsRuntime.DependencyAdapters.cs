#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDuelRuntime))]
internal sealed class CoreCardDeclarationsCoreNativeDuelAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDuelRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCoreNativeDuelOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDuelRuntime> _targetFactory = targetFactory;
    public void ResolveDuelResponse(global::CardGame.Core.NativeOwners.DuelHandle duel, global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card? selectedSlash) => this._targetFactory().ResolveDuelResponse(duel, responder, selectedSlash);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreCardDeclarationsCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeFactionDefenseRuntime))]
internal sealed class CoreCardDeclarationsCoreNativeFactionDefenseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeFactionDefenseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCoreNativeFactionDefenseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeFactionDefenseRuntime> _targetFactory = targetFactory;
    public void CompleteFactionDefenseResponse(global::CardGame.Core.NativeOwners.FactionDefenseHandle pending, global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card? selectedDodge, bool usedBagua, global::CardGame.Core.CardConversionSource? virtualResponseSource = default) => this._targetFactory().CompleteFactionDefenseResponse(pending, provider, selectedDodge, usedBagua, virtualResponseSource);
    public void AdvanceFactionDefenseCandidate(bool keepCurrentRequest = false) => this._targetFactory().AdvanceFactionDefenseCandidate(keepCurrentRequest);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime))]
internal sealed class CoreCardDeclarationsCoreCardMovementProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCoreCardMovementProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> _targetFactory = targetFactory;
    public bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default) => this._targetFactory().TryBeginCardsMovedProgramWindow(instructionFrameId, subjectSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime))]
internal sealed class CoreCardDeclarationsCoreNativeNullificationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCoreNativeNullificationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> _targetFactory = targetFactory;
    public void ResolveNullificationChoice(global::CardGame.Core.NullificationWindowFrame pending, global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card? selectedCard, global::CardGame.Core.CardConversionSource? selectedConversionSource = default) => this._targetFactory().ResolveNullificationChoice(pending, responder, selectedCard, selectedConversionSource);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CoreCardDeclarationsCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public void ClearPendingDecision() => this._targetFactory().ClearPendingDecision();
    public global::CardGame.Core.PromptId CreatePromptId() => this._targetFactory().CreatePromptId();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunHuanAnniversaryRuntime))]
internal sealed class CoreCardDeclarationsCharacterSunHuanAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunHuanAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCharacterSunHuanAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunHuanAnniversaryRuntime> _targetFactory = targetFactory;
    public bool TryReturnAnniversarySunHuanDeclaration(global::CardGame.Core.CardDeclarationFrame d, bool success) => this._targetFactory().TryReturnAnniversarySunHuanDeclaration(d, success);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime))]
internal sealed class CoreCardDeclarationsCharacterGuanYuWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCharacterGuanYuWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime> _targetFactory = targetFactory;
    public int? JuewuForeignHandRank(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().JuewuForeignHandRank(owner, card);
    public global::CardGame.Core.DeclaredCardPayment CaptureWuyouDeclarationPayment(global::CardGame.Core.DeclaredCardPayment payment, global::CardGame.Core.CardDeclarationParentReturn parent) => this._targetFactory().CaptureWuyouDeclarationPayment(payment, parent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime))]
internal sealed class CoreCardDeclarationsCoreResolutionIdentityBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCoreResolutionIdentityBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> _targetFactory = targetFactory;
    public long _resolutionSequence { get => this._targetFactory()._resolutionSequence; set => this._targetFactory()._resolutionSequence = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDodgeRuntime))]
internal sealed class CoreCardDeclarationsCoreNativeDodgeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDodgeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCoreNativeDodgeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDodgeRuntime> _targetFactory = targetFactory;
    public void ResolveDodgeResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CharacterState defender, global::CardGame.Core.Card responseCard) => this._targetFactory().ResolveDodgeResponse(attack, defender, responseCard);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuoYouZhiMouMobileRuntime))]
internal sealed class CoreCardDeclarationsCharacterGuoYouZhiMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoYouZhiMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCharacterGuoYouZhiMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoYouZhiMouMobileRuntime> _targetFactory = targetFactory;
    public bool ReturnMouGyzDeclaration(global::CardGame.Core.CardDeclarationFrame d, bool success) => this._targetFactory().ReturnMouGyzDeclaration(d, success);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunQuanOlMagicRuntime))]
internal sealed class CoreCardDeclarationsCharacterSunQuanOlMagicAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanOlMagicRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCharacterSunQuanOlMagicOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanOlMagicRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DeclaredCardPayment CaptureMoQuanyuDeclarationQuota(global::CardGame.Core.DeclaredCardPayment paid, global::CardGame.Core.CardDeclarationParentReturn parent, global::System.Collections.Generic.IReadOnlyList<int> targets) => this._targetFactory().CaptureMoQuanyuDeclarationQuota(paid, parent, targets);
    public void RecordMoQuanyuDeclarationPayment(long declarationId) => this._targetFactory().RecordMoQuanyuDeclarationPayment(declarationId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActionDiscardColorsRuntime))]
internal sealed class CoreCardDeclarationsCoreActionDiscardColorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionDiscardColorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCoreActionDiscardColorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionDiscardColorsRuntime> _targetFactory = targetFactory;
    public bool? CapturePhysicalCardColor(int seat, global::CardGame.Core.Card card) => this._targetFactory().CapturePhysicalCardColor(seat, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuKangWumiaoRuntime))]
internal sealed class CoreCardDeclarationsCharacterLuKangWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuKangWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCharacterLuKangWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuKangWumiaoRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DeclaredCardPayment CaptureWuLuKangDixianDeclaration(global::CardGame.Core.DeclaredCardPayment payment, global::CardGame.Core.CardDeclarationParentReturn parent) => this._targetFactory().CaptureWuLuKangDixianDeclaration(payment, parent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeRecoveryCardsRuntime))]
internal sealed class CoreCardDeclarationsCoreNativeRecoveryCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRecoveryCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCoreNativeRecoveryCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRecoveryCardsRuntime> _targetFactory = targetFactory;
    public void ResolveRecoveryCard(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card card, string cardName, global::CardGame.Core.CardKind? playedCardKind = default, int recoveryAmount = 1, global::System.Collections.Generic.IReadOnlyList<(string SkillId, string PolicyId)>? recoveryPolicySources = default, global::CardGame.Core.CardConversionSource? conversionSource = default, global::CardGame.Core.DyingResponseEvent? dyingResponse = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, global::CardGame.Core.DrawFundedDistinctBasicPayment? drawFundedPayment = default, global::CardGame.Core.HuanpeiPaidOrigin? huanpeiOrigin = default) => this._targetFactory().ResolveRecoveryCard(source, target, card, cardName, playedCardKind, recoveryAmount, recoveryPolicySources, conversionSource, dyingResponse, physicalCards, drawFundedPayment, huanpeiOrigin);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLvKuangLvXiangOlRuntime))]
internal sealed class CoreCardDeclarationsCharacterLvKuangLvXiangOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvKuangLvXiangOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCharacterLvKuangLvXiangOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvKuangLvXiangOlRuntime> _targetFactory = targetFactory;
    public bool TryApplyQigongDeclaration(global::CardGame.Core.CardDeclarationFrame declaration) => this._targetFactory().TryApplyQigongDeclaration(declaration);
    public bool TryReturnQigongDeclaration(global::CardGame.Core.CardDeclarationFrame d, bool success) => this._targetFactory().TryReturnQigongDeclaration(d, success);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangGaiMouMobileRuntime))]
internal sealed class CoreCardDeclarationsCharacterHuangGaiMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangGaiMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCharacterHuangGaiMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangGaiMouMobileRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DeclaredCardPayment CaptureMouZhaJiangDeclaration(global::CardGame.Core.DeclaredCardPayment payment, global::CardGame.Core.CardDeclarationParentReturn parent) => this._targetFactory().CaptureMouZhaJiangDeclaration(payment, parent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterJuShouMouRuntime))]
internal sealed class CoreCardDeclarationsCharacterJuShouMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJuShouMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCharacterJuShouMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterJuShouMouRuntime> _targetFactory = targetFactory;
    public bool ReturnMouJsDeclaration(global::CardGame.Core.CardDeclarationFrame d, bool succeeded) => this._targetFactory().ReturnMouJsDeclaration(d, succeeded);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCharacterHostRuntime))]
internal sealed class CoreCardDeclarationsCoreCharacterHostAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCharacterHostRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCoreCharacterHostOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCharacterHostRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<string> AcquireRuntimeSkills(global::CardGame.Core.CharacterState player, string sourceSkillId, global::System.Collections.Generic.IReadOnlyList<string> skillIds) => this._targetFactory().AcquireRuntimeSkills(player, sourceSkillId, skillIds);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime))]
internal sealed class CoreCardDeclarationsCoreActiveNativeAttackQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCoreActiveNativeAttackQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get => this._targetFactory().ActiveCardAttack; set => this._targetFactory().ActiveCardAttack = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime))]
internal sealed class CoreCardDeclarationsCoreDyingCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCoreDyingCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DyingFrame? ActiveDying { get => this._targetFactory().ActiveDying; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeMultiTargetActionsRuntime))]
internal sealed class CoreCardDeclarationsCoreNativeMultiTargetActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeMultiTargetActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCoreNativeMultiTargetActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeMultiTargetActionsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CharacterState> GetFangtianOrderedSlashTargets(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card slash, global::CardGame.Core.CardConversionSource? conversionSource = default, bool ignoresSlashLimit = false, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0) => this._targetFactory().GetFangtianOrderedSlashTargets(actor, slash, conversionSource, ignoresSlashLimit, effectiveKind);
    public void AddFangtianHalberdSlashActions(global::System.Collections.Generic.ICollection<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card physicalCard, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CharacterState> legalTargets, string slashName, global::CardGame.Core.CardKind? playedCardKind, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default) => this._targetFactory().AddFangtianHalberdSlashActions(actions, actor, physicalCard, legalTargets, slashName, playedCardKind, conversionSource, additionalConversionSources);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheStartingResourcesRuntime))]
internal sealed class CoreCardDeclarationsCoreShanheStartingResourcesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheStartingResourcesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCoreShanheStartingResourcesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheStartingResourcesRuntime> _targetFactory = targetFactory;
    public void CaptureShanheClaimedDeclarationUse(global::CardGame.Core.DeclaredCardPayment paid) => this._targetFactory().CaptureShanheClaimedDeclarationUse(paid);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDyingRuntime))]
internal sealed class CoreCardDeclarationsCoreNativeDyingAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDyingRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCoreNativeDyingOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDyingRuntime> _targetFactory = targetFactory;
    public void CompleteDyingCardResponse(global::CardGame.Core.DyingResponseEvent response) => this._targetFactory().CompleteDyingCardResponse(response);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineJianjiDeclarationsRuntime))]
internal sealed class CoreCardDeclarationsCoreJianjiDeclarationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJianjiDeclarationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCoreJianjiDeclarationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJianjiDeclarationsRuntime> _targetFactory = targetFactory;
    public bool TryReturnJianjiDeclaration(global::CardGame.Core.CardDeclarationFrame declaration, bool success) => this._targetFactory().TryReturnJianjiDeclaration(declaration, success);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXunYuMobileGodRuntime))]
internal sealed class CoreCardDeclarationsCharacterXunYuMobileGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXunYuMobileGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCharacterXunYuMobileGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXunYuMobileGodRuntime> _targetFactory = targetFactory;
    public void PayQizhengSingleResponse(long id, global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind) => this._targetFactory().PayQizhengSingleResponse(id, responder, card, kind);
    public void ResolveQizhengAfterResponse(long id) => this._targetFactory().ResolveQizhengAfterResponse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSimaYiMouRuntime))]
internal sealed class CoreCardDeclarationsCharacterSimaYiMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCharacterSimaYiMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiMouRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DeclaredCardPayment CaptureMouSmDeclaration(global::CardGame.Core.DeclaredCardPayment payment, global::CardGame.Core.CardDeclarationParentReturn parent) => this._targetFactory().CaptureMouSmDeclaration(payment, parent);
    public global::System.Collections.Generic.IReadOnlyList<int> MouSmDeclarationTargets(global::CardGame.Core.CardDeclarationFrame frame, int viewer, bool revealAll) => this._targetFactory().MouSmDeclarationTargets(frame, viewer, revealAll);
    public bool ReturnMouSmPlayedDeclaration(global::CardGame.Core.CardDeclarationFrame d, bool succeeded) => this._targetFactory().ReturnMouSmPlayedDeclaration(d, succeeded);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSkillOwnershipWindowsRuntime))]
internal sealed class CoreCardDeclarationsCoreSkillOwnershipWindowsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillOwnershipWindowsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCoreSkillOwnershipWindowsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillOwnershipWindowsRuntime> _targetFactory = targetFactory;
    public bool TryBeginAdvancedSkillsChanged(long parentFrameId) => this._targetFactory().TryBeginAdvancedSkillsChanged(parentFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime))]
internal sealed class CoreCardDeclarationsCharacterDaQiaoXiaoQiaoHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCharacterDaQiaoXiaoQiaoHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> _targetFactory = targetFactory;
    public bool ReturnHappyQiaoShutuDeclaration(global::CardGame.Core.CardDeclarationFrame declaration, bool succeeded) => this._targetFactory().ReturnHappyQiaoShutuDeclaration(declaration, succeeded);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuoYuanMobileShiRuntime))]
internal sealed class CoreCardDeclarationsCharacterGuoYuanMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoYuanMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCharacterGuoYuanMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoYuanMobileShiRuntime> _targetFactory = targetFactory;
    public bool TryReturnShiGyDeclaration(global::CardGame.Core.CardDeclarationFrame d, bool success) => this._targetFactory().TryReturnShiGyDeclaration(d, success);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime))]
internal sealed class CoreCardDeclarationsCorePhysicalMovementFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCorePhysicalMovementFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> _targetFactory = targetFactory;
    public void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0) => this._targetFactory().MoveCard(card, from, to, reason, beforeFact, emitCardMovedEvents, disclosure);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholIdentityPoliciesRuntime))]
internal sealed class CoreCardDeclarationsCoreAlcoholIdentityPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholIdentityPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCoreAlcoholIdentityPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholIdentityPoliciesRuntime> _targetFactory = targetFactory;
    public int EffectiveOwnedCardRank(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().EffectiveOwnedCardRank(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime))]
internal sealed class CoreCardDeclarationsCoreNativeCardContinuationCursorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCoreNativeCardContinuationCursorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.DuelHandle? ActiveDuel { get => this._targetFactory().ActiveDuel; set => this._targetFactory().ActiveDuel = value; }
    public global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get => this._targetFactory().ActiveGroupCard; set => this._targetFactory().ActiveGroupCard = value; }
    public global::CardGame.Core.NullificationWindowFrame? ActiveNullificationWindow { get => this._targetFactory().ActiveNullificationWindow; }
    public global::CardGame.Core.NativeOwners.FactionDefenseHandle? ActiveFactionDefense { get => this._targetFactory().ActiveFactionDefense; set => this._targetFactory().ActiveFactionDefense = value; }
    public global::CardGame.Core.NativeOwners.FactionCardRequestHandle? ActiveFactionCardRequest { get => this._targetFactory().ActiveFactionCardRequest; set => this._targetFactory().ActiveFactionCardRequest = value; }
    public global::CardGame.Core.NativeOwners.BorrowedSwordHandle? ActiveBorrowedSword { get => this._targetFactory().ActiveBorrowedSword; set => this._targetFactory().ActiveBorrowedSword = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreSnapshotValueProjectionRuntime))]
internal sealed class CoreCardDeclarationsCoreSnapshotValueProjectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreSnapshotValueProjectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCoreSnapshotValueProjectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreSnapshotValueProjectionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardSnapshot ToSnapshot(global::CardGame.Core.Card card) => this._targetFactory().ToSnapshot(card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime))]
internal sealed class CoreCardDeclarationsCoreNativeBorrowedSwordAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCoreNativeBorrowedSwordOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime> _targetFactory = targetFactory;
    public void ResolveBorrowedSwordSlashChoice(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, global::CardGame.Core.Card slash, global::CardGame.Core.CardKind? requestedEffectiveKind = default) => this._targetFactory().ResolveBorrowedSwordSlashChoice(pending, slash, requestedEffectiveKind);
    public void CompleteBorrowedSwordWithoutSlash(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, bool transferWeapon) => this._targetFactory().CompleteBorrowedSwordWithoutSlash(pending, transferWeapon);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeActionDispatchRuntime))]
internal sealed class CoreCardDeclarationsCoreNativeActionDispatchAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeActionDispatchRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCoreNativeActionDispatchOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeActionDispatchRuntime> _targetFactory = targetFactory;
    public void ExecuteAction(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action, global::System.Collections.Generic.IReadOnlyList<int>? activeSkillCardIds = default, global::System.Collections.Generic.IReadOnlyList<int>? activeSkillTargetSeats = default) => this._targetFactory().ExecuteAction(actor, action, activeSkillCardIds, activeSkillTargetSeats);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime))]
internal sealed class CoreCardDeclarationsCorePlayCardHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCorePlayCardHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramViewAs? ViewAsRule(global::CardGame.Core.CardConversionSource source) => this._targetFactory().ViewAsRule(source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeFactionSlashRuntime))]
internal sealed class CoreCardDeclarationsCoreNativeFactionSlashAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeFactionSlashRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCoreNativeFactionSlashOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeFactionSlashRuntime> _targetFactory = targetFactory;
    public void CompleteFactionSlashResponse(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card selectedSlash) => this._targetFactory().CompleteFactionSlashResponse(pending, provider, selectedSlash);
    public void AdvanceFactionSlashCandidate(bool keepCurrentRequest = false) => this._targetFactory().AdvanceFactionSlashCandidate(keepCurrentRequest);
    public void BeginProvidedFactionSlashSlash(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CharacterState provider, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> physicalCards, global::CardGame.Core.CardKind effectiveKind, bool usesZhuqueFan, global::CardGame.Core.CardConversionSource? conversionSource = default) => this._targetFactory().BeginProvidedFactionSlashSlash(pending, provider, physicalCards, effectiveKind, usesZhuqueFan, conversionSource);
    public void BeginBorrowedSwordFactionSlashSlash(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CharacterState provider, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> physicalCards, global::CardGame.Core.CardKind effectiveKind, bool usesZhuqueFan, global::CardGame.Core.CardConversionSource? conversionSource = default) => this._targetFactory().BeginBorrowedSwordFactionSlashSlash(pending, provider, physicalCards, effectiveKind, usesZhuqueFan, conversionSource);
    public void BeginQinglongCrescentBladeFactionSlashSlash(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CharacterState provider, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> physicalCards, global::CardGame.Core.CardKind effectiveKind, bool usesZhuqueFan, global::CardGame.Core.CardConversionSource? conversionSource = default) => this._targetFactory().BeginQinglongCrescentBladeFactionSlashSlash(pending, provider, physicalCards, effectiveKind, usesZhuqueFan, conversionSource);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageApplicationRuntime))]
internal sealed class CoreCardDeclarationsCoreNativeDamageApplicationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageApplicationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCoreNativeDamageApplicationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageApplicationRuntime> _targetFactory = targetFactory;
    public bool ApplyAttackDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().ApplyAttackDamage(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeGroupResponsesRuntime))]
internal sealed class CoreCardDeclarationsCoreNativeGroupResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeGroupResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCoreNativeGroupResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeGroupResponsesRuntime> _targetFactory = targetFactory;
    public void ResolveGroupResponse(global::CardGame.Core.NativeOwners.GroupCardHandle group, global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card? selectedResponse) => this._targetFactory().ResolveGroupResponse(group, responder, selectedResponse);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime))]
internal sealed class CoreCardDeclarationsCoreNativeAttackCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCoreNativeAttackCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime> _targetFactory = targetFactory;
    public void CompleteAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().CompleteAttack(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterBianFuRenOlRuntime))]
internal sealed class CoreCardDeclarationsCharacterBianFuRenOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBianFuRenOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCharacterBianFuRenOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterBianFuRenOlRuntime> _targetFactory = targetFactory;
    public void CaptureBianFuRenClaimedDeclarationPayment(long nativeUseId) => this._targetFactory().CaptureBianFuRenClaimedDeclarationPayment(nativeUseId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime))]
internal sealed class CoreCardDeclarationsCoreNativeCardAppearanceAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCoreNativeCardAppearanceOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().EffectiveSuit(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime))]
internal sealed class CoreCardDeclarationsCoreCardUseModulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCoreCardUseModulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> _targetFactory = targetFactory;
    public bool IsTurnHandCardRestricted(global::CardGame.Core.CharacterState player, global::CardGame.Core.Card card) => this._targetFactory().IsTurnHandCardRestricted(player, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActionQueriesRuntime))]
internal sealed class CoreCardDeclarationsCoreActionQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCoreActionQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> GetProgramViewAsConversions(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind outputKind, bool forResponse, bool dyingUse = false) => this._targetFactory().GetProgramViewAsConversions(owner, card, outputKind, forResponse, dyingUse);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreCardDeclarationsCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind) => this._targetFactory().PopResolutionFrame(frameId, expectedKind);
    public void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeFrame(expectedFrameId, next);
    public void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame) => this._targetFactory().PushRuntimeFrame(frame);
    public void AdvanceRuntimeFrame(long frameId) => this._targetFactory().AdvanceRuntimeFrame(frameId);
    public void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeTop(next);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativeUsedDiscardOriginsRuntime))]
internal sealed class CoreCardDeclarationsCoreNativeUsedDiscardOriginsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeUsedDiscardOriginsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCoreNativeUsedDiscardOriginsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeUsedDiscardOriginsRuntime> _targetFactory = targetFactory;
    public void CaptureClaimedDeclarationTrueUse(global::CardGame.Core.DeclaredCardPayment paid) => this._targetFactory().CaptureClaimedDeclarationTrueUse(paid);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseRulesRuntime))]
internal sealed class CoreCardDeclarationsCoreProgramCardUseRulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseRulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCoreProgramCardUseRulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseRulesRuntime> _targetFactory = targetFactory;
    public void AddProgramTargetCountSlashActions(global::System.Collections.Generic.ICollection<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card physicalCard, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CharacterState> legalTargets, string slashName, global::CardGame.Core.CardKind? playedCardKind, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default) => this._targetFactory().AddProgramTargetCountSlashActions(actions, actor, physicalCard, legalTargets, slashName, playedCardKind, conversionSource, additionalConversionSources);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime))]
internal sealed class CoreCardDeclarationsCoreTableOrderAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCoreTableOrderOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> _targetFactory = targetFactory;
    public int TableStepSeat(int from, int offset) => this._targetFactory().TableStepSeat(from, offset);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaiWenJiLeRuntime))]
internal sealed class CoreCardDeclarationsCharacterCaiWenJiLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiWenJiLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCharacterCaiWenJiLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiWenJiLeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DeclaredCardPayment CaptureBeifenDeclarationPayment(global::CardGame.Core.DeclaredCardPayment payment, global::CardGame.Core.CardDeclarationParentReturn parent) => this._targetFactory().CaptureBeifenDeclarationPayment(payment, parent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXiaoQiaoLeRuntime))]
internal sealed class CoreCardDeclarationsCharacterXiaoQiaoLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCharacterXiaoQiaoLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoLeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DeclaredCardPayment CaptureWeiwanDeclarationPayment(global::CardGame.Core.DeclaredCardPayment paid, global::CardGame.Core.CardDeclarationParentReturn parent) => this._targetFactory().CaptureWeiwanDeclarationPayment(paid, parent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuSuMouRuntime))]
internal sealed class CoreCardDeclarationsCharacterLuSuMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCharacterLuSuMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMouRuntime> _targetFactory = targetFactory;
    public bool ReturnMouLuSuPlayedDeclaration(global::CardGame.Core.CardDeclarationFrame d, bool succeeded) => this._targetFactory().ReturnMouLuSuPlayedDeclaration(d, succeeded);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreCardDeclarationsCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player) => this._targetFactory().GetSkillBindingShard(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterKeBiNengOlRuntime))]
internal sealed class CoreCardDeclarationsCharacterKeBiNengOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterKeBiNengOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCharacterKeBiNengOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterKeBiNengOlRuntime> _targetFactory = targetFactory;
    public bool TryReturnKeBiNengPingduanDeclaration(global::CardGame.Core.CardDeclarationFrame declaration, bool success) => this._targetFactory().TryReturnKeBiNengPingduanDeclaration(declaration, success);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhouYuOlMouRuntime))]
internal sealed class CoreCardDeclarationsCharacterZhouYuOlMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuOlMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCharacterZhouYuOlMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuOlMouRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DeclaredCardPayment CaptureMouGuquDeclaration(global::CardGame.Core.DeclaredCardPayment paid, global::CardGame.Core.CardDeclarationParentReturn parent) => this._targetFactory().CaptureMouGuquDeclaration(paid, parent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime))]
internal sealed class CoreCardDeclarationsCharacterDiaoChanLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCharacterDiaoChanLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DeclaredCardPayment CaptureDiouDeclaration(global::CardGame.Core.DeclaredCardPayment paid, global::CardGame.Core.CardDeclarationParentReturn parent) => this._targetFactory().CaptureDiouDeclaration(paid, parent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeResponseWindowsRuntime))]
internal sealed class CoreCardDeclarationsCoreNativeResponseWindowsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeResponseWindowsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCoreNativeResponseWindowsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeResponseWindowsRuntime> _targetFactory = targetFactory;
    public void PopResponseWindow(long parentFrameId) => this._targetFactory().PopResponseWindow(parentFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime))]
internal sealed class CoreCardDeclarationsCoreCardConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDeclarationsCoreCardConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime> _targetFactory = targetFactory;
    public string DescribeConversion(global::CardGame.Core.CardConversionSource? source, string description) => this._targetFactory().DescribeConversion(source, description);
}
