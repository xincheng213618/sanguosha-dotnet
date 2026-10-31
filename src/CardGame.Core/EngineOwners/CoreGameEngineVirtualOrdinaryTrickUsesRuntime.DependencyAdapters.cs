#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualTurnDamageClaimsRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCoreActualTurnDamageClaimsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualTurnDamageClaimsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCoreActualTurnDamageClaimsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualTurnDamageClaimsRuntime> _targetFactory = targetFactory;
    public bool MatchesDeclaredActualDamageUse(global::CardGame.Core.CardUseFrame use, int actor, int provider) => this._targetFactory().MatchesDeclaredActualDamageUse(use, actor, provider);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCoreCardActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCoreCardActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> _targetFactory = targetFactory;
    public bool TryBeginProgramCardWindow(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<int> opponents, global::CardGame.Core.ProgramCardContinuation? continuation, bool? cardUseCausedDamage = default, global::CardGame.Core.ProgramTrickContinuation? trickContinuation = default, global::CardGame.Core.ProgramSimpleCardContinuation? simpleContinuation = default, global::CardGame.Core.ProgramCompletedResponseReturn? completedResponseReturn = default, global::CardGame.Core.CaoChunHandUsePayment? caoChunPayment = default, bool caoChunDebtOnly = false) => this._targetFactory().TryBeginProgramCardWindow(attack, action, window, opponents, continuation, cardUseCausedDamage, trickContinuation, simpleContinuation, completedResponseReturn, caoChunPayment, caoChunDebtOnly);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDaQiaoLeRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCharacterDaQiaoLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCharacterDaQiaoLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoLeRuntime> _targetFactory = targetFactory;
    public bool IsZixiExtraEffectTargetSubset(global::CardGame.Core.CardUseFrame use, global::System.Collections.Generic.IReadOnlyList<int> targets) => this._targetFactory().IsZixiExtraEffectTargetSubset(use, targets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOverflowTargetCancellationRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCoreOverflowTargetCancellationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOverflowTargetCancellationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCoreOverflowTargetCancellationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOverflowTargetCancellationRuntime> _targetFactory = targetFactory;
    public bool HasOverflowTargetCancellation(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasOverflowTargetCancellation(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhaoYunMobileShiRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCharacterZhaoYunMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoYunMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCharacterZhaoYunMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoYunMobileShiRuntime> _targetFactory = targetFactory;
    public bool IsExactMobileShiCuifengProgram(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().IsExactMobileShiCuifengProgram(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCoreActiveNativeAttackQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCoreActiveNativeAttackQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get => this._targetFactory().ActiveCardAttack; set => this._targetFactory().ActiveCardAttack = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDengAiAnniversaryGodRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCharacterDengAiAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDengAiAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCharacterDengAiAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDengAiAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool IsShenDaCuixinTrick(long id) => this._targetFactory().IsShenDaCuixinTrick(id);
    public bool IsShenDaCuixinUse(long id) => this._targetFactory().IsShenDaCuixinUse(id);
    public global::CardGame.Core.Card ReadShenDaCuixinAppearance(long id, global::CardGame.Core.CardAppearanceReference a) => this._targetFactory().ReadShenDaCuixinAppearance(id, a);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCoreNativeCardContinuationCursorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCoreNativeCardContinuationCursorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get => this._targetFactory().ActiveGroupCard; set => this._targetFactory().ActiveGroupCard = value; }
    public global::CardGame.Core.NativeOwners.DuelHandle? ActiveDuel { get => this._targetFactory().ActiveDuel; set => this._targetFactory().ActiveDuel = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePrivateOfferReturnsRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCorePrivateOfferReturnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateOfferReturnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCorePrivateOfferReturnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateOfferReturnsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramEffect? GetPausedPrivateOfferEffect(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().GetPausedPrivateOfferEffect(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineZhaoguMirrorRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCoreZhaoguMirrorAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineZhaoguMirrorRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCoreZhaoguMirrorOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineZhaoguMirrorRuntime> _targetFactory = targetFactory;
    public bool IsZhaoguVirtualOrdinaryTrickUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().IsZhaoguVirtualOrdinaryTrickUse(use);
    public void ReturnZhaoguVirtualOrdinaryTrickUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().ReturnZhaoguVirtualOrdinaryTrickUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuaManOlRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCharacterHuaManOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaManOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCharacterHuaManOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaManOlRuntime> _targetFactory = targetFactory;
    public bool HasHuaManSouyingCancellation(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasHuaManSouyingCancellation(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCoreProgramEventHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCoreProgramEventHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent => this._targetFactory().ProgramEventHistory<T>();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCoreNativeCardUseFramesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCoreNativeCardUseFramesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime> _targetFactory = targetFactory;
    public void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step) => this._targetFactory().SetCardUseStep(frameId, step);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDengAiMobileShiRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCharacterDengAiMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDengAiMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCharacterDengAiMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDengAiMobileShiRuntime> _targetFactory = targetFactory;
    public bool ValidShiDaVirtualSnatchUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().ValidShiDaVirtualSnatchUse(use);
    public bool TryReturnShiDaVirtualSnatch(global::CardGame.Core.CardUseFrame use) => this._targetFactory().TryReturnShiDaVirtualSnatch(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardUseCompletionRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCoreNativeCardUseCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCoreNativeCardUseCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseCompletionRuntime> _targetFactory = targetFactory;
    public void PopFinishedCardUse(long frameId) => this._targetFactory().PopFinishedCardUse(frameId);
    public void CompleteFinishedAttackCardUse(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().CompleteFinishedAttackCardUse(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAfterTurnEndedProgramsRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCoreAfterTurnEndedProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAfterTurnEndedProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCoreAfterTurnEndedProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAfterTurnEndedProgramsRuntime> _targetFactory = targetFactory;
    public bool MatchesAfterTurnEndedChild(global::CardGame.Core.DeferredTurnEndFrame parent, global::CardGame.Core.ProgramSkillFrame child, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().MatchesAfterTurnEndedChild(parent, child, context);
    public global::CardGame.Core.ProgramTriggerCandidate AfterTurnEndedCandidate(global::CardGame.Core.DeferredTurnEndFrame parent) => this._targetFactory().AfterTurnEndedCandidate(parent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void AdvanceRuntimeProgram(long frameId) => this._targetFactory().AdvanceRuntimeProgram(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXunYuMobileGodRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCharacterXunYuMobileGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXunYuMobileGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCharacterXunYuMobileGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXunYuMobileGodRuntime> _targetFactory = targetFactory;
    public bool HasDinghanCancellation(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasDinghanCancellation(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramOrdinaryTrickUsesRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCoreProgramOrdinaryTrickUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOrdinaryTrickUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCoreProgramOrdinaryTrickUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOrdinaryTrickUsesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption> BuildProgramOrdinaryTrickUseOptions(global::CardGame.Core.CharacterState source, global::CardGame.Core.CardKind? outputKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool excludeOwner = false, bool enforceUsePermission = false, global::CardGame.Core.Suit? beneficiaryShieldSuit = default, bool? actualEffectiveColor = default, bool hasActualColor = false, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, bool includeNextActualUseAdjustment = false, global::CardGame.Core.CardConversionSource? conversionSource = default) => this._targetFactory().BuildProgramOrdinaryTrickUseOptions(source, outputKind, physicalSuit, excludeOwner, enforceUsePermission, beneficiaryShieldSuit, actualEffectiveColor, hasActualColor, physicalCardIds, includeNextActualUseAdjustment, conversionSource);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCoreNativeCardClassificationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCoreNativeCardClassificationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DamageNature GetDamageNature(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().GetDamageNature(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterChengYuMouOlRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCharacterChengYuMouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengYuMouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCharacterChengYuMouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengYuMouOlRuntime> _targetFactory = targetFactory;
    public bool IsMouDanchiDrawTwo(long id) => this._targetFactory().IsMouDanchiDrawTwo(id);
    public global::CardGame.Core.Card ReadMouDanchiAppearance(long id, global::CardGame.Core.CardAppearanceReference appearance) => this._targetFactory().ReadMouDanchiAppearance(id, appearance);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangYanMobileShiRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCharacterZhangYanMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYanMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCharacterZhangYanMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYanMobileShiRuntime> _targetFactory = targetFactory;
    public bool ValidShiZyVirtualDuelUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().ValidShiZyVirtualDuelUse(use);
    public bool TryReturnShiZyVirtualDuel(global::CardGame.Core.CardUseFrame use) => this._targetFactory().TryReturnShiZyVirtualDuel(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuLiAnniversaryRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCharacterLiuLiAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuLiAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCharacterLiuLiAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuLiAnniversaryRuntime> _targetFactory = targetFactory;
    public bool IsAnniversaryLiuLiVirtualUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().IsAnniversaryLiuLiVirtualUse(use);
    public bool TryReturnAnniversaryLiuLiVirtualUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().TryReturnAnniversaryLiuLiVirtualUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuoJiaHappyGodRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCharacterGuoJiaHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoJiaHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCharacterGuoJiaHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoJiaHappyGodRuntime> _targetFactory = targetFactory;
    public bool IsHappyGuoJiaVirtualUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().IsHappyGuoJiaVirtualUse(use);
    public bool TryReturnHappyGuoJiaVirtualUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().TryReturnHappyGuoJiaVirtualUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId) => this._targetFactory().GetActiveProgramFrame(frameId);
    public bool HasPendingProgramBoundCards { get => this._targetFactory().HasPendingProgramBoundCards; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterJiangJiMouRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCharacterJiangJiMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiangJiMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCharacterJiangJiMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiangJiMouRuntime> _targetFactory = targetFactory;
    public bool IsMouJjEffectSubset(global::CardGame.Core.CardUseFrame use, global::System.Collections.Generic.IReadOnlyList<int> targets) => this._targetFactory().IsMouJjEffectSubset(use, targets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCoreProgramCardPaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCoreProgramCardPaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime> _targetFactory = targetFactory;
    public int ResolveProgramParticipant(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramParticipantReference reference) => this._targetFactory().ResolveProgramParticipant(frame, reference);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuYongAnniversaryRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCharacterLiuYongAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuYongAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCharacterLiuYongAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuYongAnniversaryRuntime> _targetFactory = targetFactory;
    public bool IsAnniversaryLiuYongVirtualUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().IsAnniversaryLiuYongVirtualUse(use);
    public bool TryReturnAnniversaryLiuYongVirtualUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().TryReturnAnniversaryLiuYongVirtualUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDongChengMouRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCharacterDongChengMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongChengMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCharacterDongChengMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongChengMouRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card ReadMouDcUseAppearance(long id, global::CardGame.Core.CardAppearanceReference appearance) => this._targetFactory().ReadMouDcUseAppearance(id, appearance);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuoJiaMobileGodRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCharacterGuoJiaMobileGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoJiaMobileGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCharacterGuoJiaMobileGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoJiaMobileGodRuntime> _targetFactory = targetFactory;
    public bool IsShenGuoJiaVirtualUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().IsShenGuoJiaVirtualUse(use);
    public bool TryReturnShenGuoJiaVirtualUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().TryReturnShenGuoJiaVirtualUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardUseRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCoreNativeCardUseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCoreNativeCardUseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseRuntime> _targetFactory = targetFactory;
    public long BeginCardUse(global::CardGame.Core.Card card, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default, global::System.Collections.Generic.IReadOnlyList<int>? designatedTargetSeats = default, bool isTrueZhangbaSlash = false, global::CardGame.Core.ProgramSelectedActorDuelOrigin? selectedActorDuelOrigin = default, global::CardGame.Core.ProgramDamageTargetDuelOrigin? damageTargetDuelOrigin = default, global::CardGame.Core.ProgramDualColorDuelOrigin? dualColorDuelOrigin = default, global::CardGame.Core.ConditionalDiscardDuelOrigin? conditionalDiscardDuelOrigin = default, global::CardGame.Core.ShiQuanDuelOrigin? shiQuanDuelOrigin = default, global::CardGame.Core.ProgramVirtualOrdinaryTrickOrigin? virtualOrdinaryTrickOrigin = default, global::CardGame.Core.DrawFundedDistinctBasicPayment? drawFundedPayment = default, global::CardGame.Core.ChainedStateBasicPayment? chainedStatePayment = default, global::CardGame.Core.FriendRecastDuelOrigin? friendRecastDuel = default, global::CardGame.Core.ChosenAssaultReturn? chosenAssault = default, global::CardGame.Core.ZhenyingDuelOrigin? zhenyingDuel = default, global::CardGame.Core.HeJinMouzhuUseOrigin? heJinMouzhu = default, global::CardGame.Core.WuAnGuoLiyongDuelOrigin? wuAnGuoLiyong = default, global::CardGame.Core.TianChouShandaoUseOrigin? tianChouShandao = default, global::CardGame.Core.QinLangXianyingUseOrigin? qinLangXianying = default, global::CardGame.Core.AHuiNanJuemanUseOrigin? aHuiNanJueman = default, global::CardGame.Core.LiuYanLimuUseOrigin? liuYanLimu = default, global::CardGame.Core.JueshengDuelOrigin? jueshengDuel = default, global::CardGame.Core.QuHuangNishouUseOrigin? quHuangNishou = default, global::CardGame.Core.WeijiePaidOrigin? weijieOrigin = default, global::CardGame.Core.ZonghuPaidOrigin? zonghuOrigin = default, global::CardGame.Core.HuanpeiPaidOrigin? huanpeiOrigin = default) => this._targetFactory().BeginCardUse(card, sourceSeat, targetSeats, playedCardKind, ignoresArmor, physicalCardIds, conversionSource, additionalConversionSources, designatedTargetSeats, isTrueZhangbaSlash, selectedActorDuelOrigin, damageTargetDuelOrigin, dualColorDuelOrigin, conditionalDiscardDuelOrigin, shiQuanDuelOrigin, virtualOrdinaryTrickOrigin, drawFundedPayment, chainedStatePayment, friendRecastDuel, chosenAssault, zhenyingDuel, heJinMouzhu, wuAnGuoLiyong, tianChouShandao, qinLangXianying, aHuiNanJueman, liuYanLimu, jueshengDuel, quHuangNishou, weijieOrigin, zonghuOrigin, huanpeiOrigin);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCoreCardUseLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCoreCardUseLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId) => this._targetFactory().LifecycleCardUse(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterYangBiaoAnniversaryRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCharacterYangBiaoAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangBiaoAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCharacterYangBiaoAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangBiaoAnniversaryRuntime> _targetFactory = targetFactory;
    public bool IsAnniversaryYangBiaoVirtualUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().IsAnniversaryYangBiaoVirtualUse(use);
    public bool TryReturnAnniversaryYangBiaoVirtualUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().TryReturnAnniversaryYangBiaoVirtualUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNextActualUseTargetAdjustmentsRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCoreNextActualUseTargetAdjustmentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextActualUseTargetAdjustmentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCoreNextActualUseTargetAdjustmentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextActualUseTargetAdjustmentsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption> NextActualUseOrdinaryTrickOptions(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption> ordinary, global::CardGame.Core.Suit? suit, bool? color, global::CardGame.Core.Suit? shieldSuit, bool excludeOwner) => this._targetFactory().NextActualUseOrdinaryTrickOptions(actor, ordinary, suit, color, shieldSuit, excludeOwner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCoreNativeNullificationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCoreNativeNullificationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> _targetFactory = targetFactory;
    public void BeginJizhiOrNullificationWindow(long resolutionId, global::CardGame.Core.Card effectCard, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.LegalActionKind actionKind, int? targetCardId = default, global::CardGame.Core.CardKind? requiredCardKind = default, global::CardGame.Core.CardKind? playedCardKind = default) => this._targetFactory().BeginJizhiOrNullificationWindow(resolutionId, effectCard, sourceSeat, targetSeats, actionKind, targetCardId, requiredCardKind, playedCardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PromptId CreatePromptId() => this._targetFactory().CreatePromptId();
    public void ClearPendingDecision() => this._targetFactory().ClearPendingDecision();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHellActivitiesRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCoreHellActivitiesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellActivitiesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCoreHellActivitiesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellActivitiesRuntime> _targetFactory = targetFactory;
    public bool IsExactHellArrivalProgram(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().IsExactHellArrivalProgram(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanduGeneralNativeCardsRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCoreGuanduGeneralNativeCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduGeneralNativeCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCoreGuanduGeneralNativeCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduGeneralNativeCardsRuntime> _targetFactory = targetFactory;
    public bool IsGuanduChoulueVirtualOrdinaryProgram(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect effect) => this._targetFactory().IsGuanduChoulueVirtualOrdinaryProgram(f, effect);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuXunMouMobileRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCharacterLuXunMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuXunMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCharacterLuXunMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuXunMouMobileRuntime> _targetFactory = targetFactory;
    public bool IsMouLuXunVirtualParent(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().IsMouLuXunVirtualParent(f);
    public global::CardGame.Core.SkillProgramEffect MouLuXunVirtualInstruction(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().MouLuXunVirtualInstruction(f);
    public bool ReturnMouLuXunNativeUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().ReturnMouLuXunNativeUse(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreHellKingProgramsRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCoreHellKingProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreHellKingProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCoreHellKingProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreHellKingProgramsRuntime> _targetFactory = targetFactory;
    public bool IsHellRandomAssaultBranch(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => this._targetFactory().IsHellRandomAssaultBranch(frame, effect);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXunYuMouRuntime))]
internal sealed class CoreVirtualOrdinaryTrickUsesCharacterXunYuMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXunYuMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreVirtualOrdinaryTrickUsesCharacterXunYuMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXunYuMouRuntime> _targetFactory = targetFactory;
    public bool IsMouXyVirtualUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().IsMouXyVirtualUse(use);
    public bool ReturnMouXyVirtualUse(global::CardGame.Core.CardUseFrame use) => this._targetFactory().ReturnMouXyVirtualUse(use);
}
