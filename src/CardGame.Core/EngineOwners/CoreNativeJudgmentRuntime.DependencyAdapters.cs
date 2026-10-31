#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWangYiMobileRuntime))]
internal sealed class CoreNativeJudgmentCharacterWangYiMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangYiMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCharacterWangYiMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangYiMobileRuntime> _targetFactory = targetFactory;
    public bool HasDeckTopJudgmentReplacementMaterial() => this._targetFactory().HasDeckTopJudgmentReplacementMaterial();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaYuanYiOlRuntime))]
internal sealed class CoreNativeJudgmentCharacterMaYuanYiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaYuanYiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCharacterMaYuanYiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaYuanYiOlRuntime> _targetFactory = targetFactory;
    public bool TryReturnBinghuoJudgment(global::CardGame.Core.JudgmentFrame j) => this._targetFactory().TryReturnBinghuoJudgment(j);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterQianZhaoOlRuntime))]
internal sealed class CoreNativeJudgmentCharacterQianZhaoOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQianZhaoOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCharacterQianZhaoOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterQianZhaoOlRuntime> _targetFactory = targetFactory;
    public bool TryReturnWeifuJudgment(global::CardGame.Core.JudgmentFrame j) => this._targetFactory().TryReturnWeifuJudgment(j);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFinalizedJudgmentDamageChildrenRuntime))]
internal sealed class CoreNativeJudgmentCoreFinalizedJudgmentDamageChildrenAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalizedJudgmentDamageChildrenRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCoreFinalizedJudgmentDamageChildrenOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalizedJudgmentDamageChildrenRuntime> _targetFactory = targetFactory;
    public bool CanBeginFinalizedDamageChildJudgment(global::CardGame.Core.JudgmentFrame original, long parentFrameId, int targetSeat, string reason, global::CardGame.Core.JudgmentContinuationKind continuation, string? resultBind, global::CardGame.Core.SkillProgramCardSetVisibility? visibility) => this._targetFactory().CanBeginFinalizedDamageChildJudgment(original, parentFrameId, targetSeat, reason, continuation, resultBind, visibility);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangZuMobileShiRuntime))]
internal sealed class CoreNativeJudgmentCharacterHuangZuMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangZuMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCharacterHuangZuMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangZuMobileShiRuntime> _targetFactory = targetFactory;
    public bool ShiHzResponseColorAllowed(int responder, bool? effectiveIsRed) => this._targetFactory().ShiHzResponseColorAllowed(responder, effectiveIsRed);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime))]
internal sealed class CoreNativeJudgmentCorePhysicalDrawFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCorePhysicalDrawFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> _targetFactory = targetFactory;
    public bool EnsureDrawPile() => this._targetFactory().EnsureDrawPile();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreNativeJudgmentCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame) => this._targetFactory().PushRuntimeFrame(frame);
    public void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind) => this._targetFactory().PopResolutionFrame(frameId, expectedKind);
    public void AdvanceRuntimeProgram(long frameId) => this._targetFactory().AdvanceRuntimeProgram(frameId);
    public void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeFrame(expectedFrameId, next);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineExactRepeatedJudgmentRuntime))]
internal sealed class CoreNativeJudgmentCoreExactRepeatedJudgmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExactRepeatedJudgmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCoreExactRepeatedJudgmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExactRepeatedJudgmentRuntime> _targetFactory = targetFactory;
    public void CaptureProgramRepeatedJudgmentClaimOutcome(global::CardGame.Core.JudgmentFrame judgment, global::CardGame.Core.Card card, global::CardGame.Core.Suit suit) => this._targetFactory().CaptureProgramRepeatedJudgmentClaimOutcome(judgment, card, suit);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime))]
internal sealed class CoreNativeJudgmentCoreResolutionIdentityBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCoreResolutionIdentityBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> _targetFactory = targetFactory;
    public long _resolutionSequence { get => this._targetFactory()._resolutionSequence; set => this._targetFactory()._resolutionSequence = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangFeiAnniversaryGodRuntime))]
internal sealed class CoreNativeJudgmentCharacterZhangFeiAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCharacterZhangFeiAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool TryReturnShenZfJudgment(global::CardGame.Core.JudgmentFrame j) => this._targetFactory().TryReturnShenZfJudgment(j);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreWushuangCombatProgramsRuntime))]
internal sealed class CoreNativeJudgmentCoreWushuangCombatProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreWushuangCombatProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCoreWushuangCombatProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreWushuangCombatProgramsRuntime> _targetFactory = targetFactory;
    public bool TryReturnWushuangLightningJudgment(global::CardGame.Core.JudgmentFrame judgment) => this._targetFactory().TryReturnWushuangLightningJudgment(judgment);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime))]
internal sealed class CoreNativeJudgmentCoreProgramCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCoreProgramCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> _targetFactory = targetFactory;
    public bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default) => this._targetFactory().HasCardPolicy(owner, kind, effectiveKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePassiveRulePoliciesRuntime))]
internal sealed class CoreNativeJudgmentCorePassiveRulePoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePassiveRulePoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCorePassiveRulePoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePassiveRulePoliciesRuntime> _targetFactory = targetFactory;
    public bool ApplyDelayedJudgmentOutcomePolicies(global::CardGame.Core.JudgmentFrame frame, bool succeeded) => this._targetFactory().ApplyDelayedJudgmentOutcomePolicies(frame, succeeded);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSlashSuitDiscardRuntime))]
internal sealed class CoreNativeJudgmentCoreSlashSuitDiscardAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashSuitDiscardRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCoreSlashSuitDiscardOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashSuitDiscardRuntime> _targetFactory = targetFactory;
    public void CaptureProgramSlashSuitJudgment(global::CardGame.Core.JudgmentFrame judgment, global::CardGame.Core.Suit suit) => this._targetFactory().CaptureProgramSlashSuitJudgment(judgment, suit);
    public bool TryReturnProgramSlashSuitJudgment(global::CardGame.Core.JudgmentFrame judgment) => this._targetFactory().TryReturnProgramSlashSuitJudgment(judgment);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime))]
internal sealed class CoreNativeJudgmentCoreNativeEquipmentAndFactionQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCoreNativeEquipmentAndFactionQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime> _targetFactory = targetFactory;
    public bool HasBagua(global::CardGame.Core.CharacterState player) => this._targetFactory().HasBagua(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhouQunOlRuntime))]
internal sealed class CoreNativeJudgmentCharacterZhouQunOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouQunOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCharacterZhouQunOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouQunOlRuntime> _targetFactory = targetFactory;
    public bool TryReturnNingwuJudgment(global::CardGame.Core.JudgmentFrame j) => this._targetFactory().TryReturnNingwuJudgment(j);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhongHuiAnniversaryGodRuntime))]
internal sealed class CoreNativeJudgmentCharacterZhongHuiAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhongHuiAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCharacterZhongHuiAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhongHuiAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool TryReturnLishiJudgment(global::CardGame.Core.JudgmentFrame j) => this._targetFactory().TryReturnLishiJudgment(j);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOriginalUseLightningRuntime))]
internal sealed class CoreNativeJudgmentCoreOriginalUseLightningAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalUseLightningRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCoreOriginalUseLightningOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalUseLightningRuntime> _targetFactory = targetFactory;
    public void CompleteOriginalUseLightningJudgment(global::CardGame.Core.JudgmentFrame pending, bool hit) => this._targetFactory().CompleteOriginalUseLightningJudgment(pending, hit);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime))]
internal sealed class CoreNativeJudgmentCoreDyingWindowSkillPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCoreDyingWindowSkillPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime> _targetFactory = targetFactory;
    public bool IsBlackTrickTargetProhibited(global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind, bool? effectiveIsRed) => this._targetFactory().IsBlackTrickTargetProhibited(target, kind, effectiveIsRed);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSourceHuojieRuntime))]
internal sealed class CoreNativeJudgmentCoreSourceHuojieAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceHuojieRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCoreSourceHuojieOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceHuojieRuntime> _targetFactory = targetFactory;
    public bool IsSourceHuojieLightningJudgment(global::CardGame.Core.JudgmentFrame j) => this._targetFactory().IsSourceHuojieLightningJudgment(j);
    public bool ReturnSourceHuojieJudgment(global::CardGame.Core.JudgmentFrame j) => this._targetFactory().ReturnSourceHuojieJudgment(j);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentReplacementProgramsRuntime))]
internal sealed class CoreNativeJudgmentCoreJudgmentReplacementProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentReplacementProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCoreJudgmentReplacementProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentReplacementProgramsRuntime> _targetFactory = targetFactory;
    public bool MatchesProgramJudgmentReplacement(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTrigger trigger, int targetSeat, string reason) => this._targetFactory().MatchesProgramJudgmentReplacement(owner, trigger, targetSeat, reason);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetProgramJudgmentReplacementCards(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTrigger trigger, int judgmentSubjectSeat, string judgmentReason, bool ignoreCurrentColor = false) => this._targetFactory().GetProgramJudgmentReplacementCards(owner, trigger, judgmentSubjectSeat, judgmentReason, ignoreCurrentColor);
    public void BeginProgramJudgmentReplacementChoice(global::CardGame.Core.JudgmentFrame pending, global::CardGame.Core.JudgmentTriggerCandidate candidate) => this._targetFactory().BeginProgramJudgmentReplacementChoice(pending, candidate);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime))]
internal sealed class CoreNativeJudgmentCorePhysicalMovementFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCorePhysicalMovementFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> _targetFactory = targetFactory;
    public void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0) => this._targetFactory().MoveCard(card, from, to, reason, beforeFact, emitCardMovedEvents, disclosure);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime))]
internal sealed class CoreNativeJudgmentCoreNativeAttackCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCoreNativeAttackCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime> _targetFactory = targetFactory;
    public void CompleteAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().CompleteAttack(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime))]
internal sealed class CoreNativeJudgmentCoreNativeCardAppearanceAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCoreNativeCardAppearanceOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().EffectiveSuit(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreJudgmentCursorRuntimeRuntime))]
internal sealed class CoreNativeJudgmentCoreJudgmentCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreJudgmentCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCoreJudgmentCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreJudgmentCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.JudgmentFrame? ActiveJudgment { get => this._targetFactory().ActiveJudgment; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramJudgmentOutcomesRuntime))]
internal sealed class CoreNativeJudgmentCoreProgramJudgmentOutcomesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramJudgmentOutcomesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCoreProgramJudgmentOutcomesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramJudgmentOutcomesRuntime> _targetFactory = targetFactory;
    public bool TryReturnProgramJudgmentOutcome(global::CardGame.Core.JudgmentFrame judgment) => this._targetFactory().TryReturnProgramJudgmentOutcome(judgment);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageJudgmentSuitPaymentsRuntime))]
internal sealed class CoreNativeJudgmentCoreDamageJudgmentSuitPaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageJudgmentSuitPaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCoreDamageJudgmentSuitPaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageJudgmentSuitPaymentsRuntime> _targetFactory = targetFactory;
    public void CaptureDamageJudgmentSuitResult(global::CardGame.Core.JudgmentFrame judgment, global::CardGame.Core.Card card, global::CardGame.Core.Suit suit) => this._targetFactory().CaptureDamageJudgmentSuitResult(judgment, card, suit);
    public bool TryReturnDamageJudgmentSuitPayment(global::CardGame.Core.JudgmentFrame judgment) => this._targetFactory().TryReturnDamageJudgmentSuitPayment(judgment);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDodgeRuntime))]
internal sealed class CoreNativeJudgmentCoreNativeDodgeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDodgeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCoreNativeDodgeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDodgeRuntime> _targetFactory = targetFactory;
    public void CompleteSuccessfulDodgeResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().CompleteSuccessfulDodgeResponse(attack);
    public void CompleteFactionDefenseBaguaResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, bool succeeded) => this._targetFactory().CompleteFactionDefenseBaguaResponse(attack, succeeded);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangJiOlRuntime))]
internal sealed class CoreNativeJudgmentCharacterZhangJiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangJiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCharacterZhangJiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangJiOlRuntime> _targetFactory = targetFactory;
    public bool TryReturnZhangJiJudgment(global::CardGame.Core.JudgmentFrame j) => this._targetFactory().TryReturnZhangJiJudgment(j);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreNativeJudgmentCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId) => this._targetFactory().GetActiveProgramFrame(frameId);
    public void SetProgramCardSet(long frameId, string bind, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.SkillProgramCardSetVisibility visibility, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardLocation>? sourceLocations = default, global::CardGame.Core.Suit? frozenRevealedSuit = default, int? selectionActorSeat = default) => this._targetFactory().SetProgramCardSet(frameId, bind, cardIds, visibility, sourceLocations, frozenRevealedSuit, selectionActorSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDiscardedProvenanceRuntime))]
internal sealed class CoreNativeJudgmentCoreDiscardedProvenanceAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDiscardedProvenanceRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCoreDiscardedProvenanceOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDiscardedProvenanceRuntime> _targetFactory = targetFactory;
    public void CaptureProvenanceJudgmentOrigin(global::CardGame.Core.JudgmentFrame judgment, global::CardGame.Core.Card card, global::CardGame.Core.Suit effectiveSuit) => this._targetFactory().CaptureProvenanceJudgmentOrigin(judgment, card, effectiveSuit);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramRepeatedJudgmentRuntime))]
internal sealed class CoreNativeJudgmentCoreProgramRepeatedJudgmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramRepeatedJudgmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCoreProgramRepeatedJudgmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramRepeatedJudgmentRuntime> _targetFactory = targetFactory;
    public void ResumeProgramRepeatedJudgment(global::CardGame.Core.JudgmentFrame pending) => this._targetFactory().ResumeProgramRepeatedJudgment(pending);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime))]
internal sealed class CoreNativeJudgmentCoreNativeCardContinuationCursorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCoreNativeCardContinuationCursorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get => this._targetFactory().ActiveGroupCard; set => this._targetFactory().ActiveGroupCard = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangQiYingOlRuntime))]
internal sealed class CoreNativeJudgmentCharacterZhangQiYingOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangQiYingOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCharacterZhangQiYingOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangQiYingOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Suit EffectiveZhenyiJudgmentSuit(global::CardGame.Core.JudgmentFrame frame, global::CardGame.Core.Suit fallback) => this._targetFactory().EffectiveZhenyiJudgmentSuit(frame, fallback);
    public int EffectiveZhenyiJudgmentRank(global::CardGame.Core.JudgmentFrame frame, int fallback) => this._targetFactory().EffectiveZhenyiJudgmentRank(frame, fallback);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchLogRuntime))]
internal sealed class CoreNativeJudgmentCoreMatchLogAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCoreMatchLogOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> _targetFactory = targetFactory;
    public void AddLog(string type, string message, int? actor = default, int? target = default) => this._targetFactory().AddLog(type, message, actor, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime))]
internal sealed class CoreNativeJudgmentCoreCardActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCoreCardActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> _targetFactory = targetFactory;
    public bool TryBeginCardResponsePrograms(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState provider, int? requesterSeat, int opponentSeat, global::CardGame.Core.CardKind effectiveKind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardActionCost> costs, global::CardGame.Core.ProgramCardContinuation continuation, global::CardGame.Core.CardConversionSource? conversionSource = default, global::CardGame.Core.Suit? completedResponseUseSuit = default) => this._targetFactory().TryBeginCardResponsePrograms(attack, actor, provider, requesterSeat, opponentSeat, effectiveKind, costs, continuation, conversionSource, completedResponseUseSuit);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuoJiaMobileGodRuntime))]
internal sealed class CoreNativeJudgmentCharacterGuoJiaMobileGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoJiaMobileGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCharacterGuoJiaMobileGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoJiaMobileGodRuntime> _targetFactory = targetFactory;
    public bool TryReturnShenGuoJiaJudgment(global::CardGame.Core.JudgmentFrame judgment) => this._targetFactory().TryReturnShenGuoJiaJudgment(judgment);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime))]
internal sealed class CoreNativeJudgmentCoreSkillProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCoreSkillProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IndexedSkillProgramTrigger> EnabledUniqueProgramTriggers(global::CardGame.Core.CharacterState player, global::CardGame.Core.SkillProgramTriggerWindow window) => this._targetFactory().EnabledUniqueProgramTriggers(player, window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineBaguaResponsesRuntime))]
internal sealed class CoreNativeJudgmentCoreBaguaResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBaguaResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCoreBaguaResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBaguaResponsesRuntime> _targetFactory = targetFactory;
    public void PublishFailedBaguaResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().PublishFailedBaguaResponse(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageApplicationRuntime))]
internal sealed class CoreNativeJudgmentCoreNativeDamageApplicationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageApplicationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCoreNativeDamageApplicationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageApplicationRuntime> _targetFactory = targetFactory;
    public bool ApplyAttackDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().ApplyAttackDamage(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime))]
internal sealed class CoreNativeJudgmentCoreTableOrderAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCoreTableOrderOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> CurrentTableOrder() => this._targetFactory().CurrentTableOrder();
    public int TableStepSeat(int from, int offset) => this._targetFactory().TableStepSeat(from, offset);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime))]
internal sealed class CoreNativeJudgmentCoreActiveNativeAttackQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCoreActiveNativeAttackQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get => this._targetFactory().ActiveCardAttack; set => this._targetFactory().ActiveCardAttack = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDamagePointJudgmentsRuntime))]
internal sealed class CoreNativeJudgmentCoreOwnedDamagePointJudgmentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDamagePointJudgmentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCoreOwnedDamagePointJudgmentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDamagePointJudgmentsRuntime> _targetFactory = targetFactory;
    public bool IsOwnedDamagePointJudgmentResultClaimed(global::CardGame.Core.JudgmentFrame judgment) => this._targetFactory().IsOwnedDamagePointJudgmentResultClaimed(judgment);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuoJiaMouRuntime))]
internal sealed class CoreNativeJudgmentCharacterGuoJiaMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoJiaMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCharacterGuoJiaMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoJiaMouRuntime> _targetFactory = targetFactory;
    public bool IsMouGjLightningJudgment(global::CardGame.Core.JudgmentFrame j) => this._targetFactory().IsMouGjLightningJudgment(j);
    public bool ReturnMouGjJudgment(global::CardGame.Core.JudgmentFrame j) => this._targetFactory().ReturnMouGjJudgment(j);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime))]
internal sealed class CoreNativeJudgmentCoreActualTurnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCoreActualTurnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardKind GetJudgmentEffectiveCardKind(global::CardGame.Core.Card card) => this._targetFactory().GetJudgmentEffectiveCardKind(card);
    public void BeginDelayedJudgmentOrTurnStart(global::CardGame.Core.CharacterState current, int? excludedCardId = default) => this._targetFactory().BeginDelayedJudgmentOrTurnStart(current, excludedCardId);
    public bool HasJudgmentEffectiveCard(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind kind) => this._targetFactory().HasJudgmentEffectiveCard(player, kind);
    public void EndTurn() => this._targetFactory().EndTurn();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSourceCursesRuntime))]
internal sealed class CoreNativeJudgmentCoreSourceCursesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceCursesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCoreSourceCursesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceCursesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card? TakeSourceCurseForJudgment(global::CardGame.Core.JudgmentFrame frame) => this._targetFactory().TakeSourceCurseForJudgment(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentProgramsRuntime))]
internal sealed class CoreNativeJudgmentCoreJudgmentProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCoreJudgmentProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgmentProgramsRuntime> _targetFactory = targetFactory;
    public bool TryBeginProgramJudgmentWindow(global::CardGame.Core.JudgmentFrame pending, global::CardGame.Core.Card judgmentCard, global::CardGame.Core.Suit effectiveSuit, bool succeeded) => this._targetFactory().TryBeginProgramJudgmentWindow(pending, judgmentCard, effectiveSuit, succeeded);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime))]
internal sealed class CoreNativeJudgmentCoreNativeDamageHandlesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCoreNativeDamageHandlesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card? GetJudgmentCard(global::CardGame.Core.JudgmentFrame frame) => this._targetFactory().GetJudgmentCard(frame);
    public global::CardGame.Core.Card? GetDelayedJudgmentCard(global::CardGame.Core.JudgmentFrame frame) => this._targetFactory().GetDelayedJudgmentCard(frame);
    public global::CardGame.Core.NativeOwners.CardAttackHandle? GetJudgmentAttack(global::CardGame.Core.JudgmentFrame frame) => this._targetFactory().GetJudgmentAttack(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineZhenyiJudgmentWindowsRuntime))]
internal sealed class CoreNativeJudgmentCoreZhenyiJudgmentWindowsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineZhenyiJudgmentWindowsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCoreZhenyiJudgmentWindowsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineZhenyiJudgmentWindowsRuntime> _targetFactory = targetFactory;
    public bool TryBeginZhenyiBeforeResultWindow(global::CardGame.Core.JudgmentFrame pending, global::CardGame.Core.Card card, global::CardGame.Core.Suit suit) => this._targetFactory().TryBeginZhenyiBeforeResultWindow(pending, card, suit);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeEquipmentInvocationsRuntime))]
internal sealed class CoreNativeJudgmentCoreNativeEquipmentInvocationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentInvocationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeJudgmentCoreNativeEquipmentInvocationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentInvocationsRuntime> _targetFactory = targetFactory;
    public bool TryBeginBaguaEquipmentInvocation(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CharacterState defender, bool factionDefense = false) => this._targetFactory().TryBeginBaguaEquipmentInvocation(attack, defender, factionDefense);
}
