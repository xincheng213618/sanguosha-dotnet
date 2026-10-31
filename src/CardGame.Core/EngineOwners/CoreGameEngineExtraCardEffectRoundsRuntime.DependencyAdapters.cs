#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDaQiaoLeRuntime))]
internal sealed class CoreExtraCardEffectRoundsCharacterDaQiaoLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraCardEffectRoundsCharacterDaQiaoLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoLeRuntime> _targetFactory = targetFactory;
    public bool IsZixiExtraEffectRound(long id) => this._targetFactory().IsZixiExtraEffectRound(id);
    public bool HasUnstartedZixiExtraEffect(long id) => this._targetFactory().HasUnstartedZixiExtraEffect(id);
    public void CaptureZixiExtraEffectAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().CaptureZixiExtraEffectAttack(attack);
    public void CompleteZixiExtraEffectRound(long id) => this._targetFactory().CompleteZixiExtraEffectRound(id);
    public bool TryBeginZixiExtraEffectRound(long id, bool afterAttack) => this._targetFactory().TryBeginZixiExtraEffectRound(id, afterAttack);
    public void AssertZixiExtraEffectRounds() => this._targetFactory().AssertZixiExtraEffectRounds();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime))]
internal sealed class CoreExtraCardEffectRoundsCoreNativeAttackCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraCardEffectRoundsCoreNativeAttackCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime> _targetFactory = targetFactory;
    public void CompleteAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().CompleteAttack(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime))]
internal sealed class CoreExtraCardEffectRoundsCoreNativeNullificationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraCardEffectRoundsCoreNativeNullificationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> _targetFactory = targetFactory;
    public void BeginNullificationWindow(long resolutionId, global::CardGame.Core.Card effectCard, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.LegalActionKind actionKind, int? targetCardId = default, global::CardGame.Core.CardKind? requiredCardKind = default, global::CardGame.Core.CardKind? playedCardKind = default) => this._targetFactory().BeginNullificationWindow(resolutionId, effectCard, sourceSeat, targetSeats, actionKind, targetCardId, requiredCardKind, playedCardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreExtraCardEffectRoundsCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraCardEffectRoundsCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeFrame(expectedFrameId, next);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CoreExtraCardEffectRoundsCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraCardEffectRoundsCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime))]
internal sealed class CoreExtraCardEffectRoundsCoreCardAttackStateAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraCardEffectRoundsCoreCardAttackStateOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> _targetFactory = targetFactory;
    public void UpdateCardContinuations(long id, global::System.Func<global::CardGame.Core.CardResolutionContinuations, global::CardGame.Core.CardResolutionContinuations> update) => this._targetFactory().UpdateCardContinuations(id, update);
    public void UpdateCardAttackState(long id, global::System.Func<global::CardGame.Core.CardAttackState?, global::CardGame.Core.CardAttackState> update) => this._targetFactory().UpdateCardAttackState(id, update);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterFeiYiOlRuntime))]
internal sealed class CoreExtraCardEffectRoundsCharacterFeiYiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterFeiYiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraCardEffectRoundsCharacterFeiYiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterFeiYiOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ExtraCardEffectState? NativeExtraEffectState(global::CardGame.Core.CardUseFrame use) => this._targetFactory().NativeExtraEffectState(use);
    public void UpdateNativeExtraEffectState(long id, global::System.Func<global::CardGame.Core.ExtraCardEffectState, global::CardGame.Core.ExtraCardEffectState> update) => this._targetFactory().UpdateNativeExtraEffectState(id, update);
    public bool IsFeiYiExtraEffectRound(long id) => this._targetFactory().IsFeiYiExtraEffectRound(id);
    public bool HasUnstartedFeiYiExtraEffect(long id) => this._targetFactory().HasUnstartedFeiYiExtraEffect(id);
    public bool TryBeginFeiYiExtraEffectRound(long id, bool afterAttack) => this._targetFactory().TryBeginFeiYiExtraEffectRound(id, afterAttack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangYueyingOlMouRuntime))]
internal sealed class CoreExtraCardEffectRoundsCharacterHuangYueyingOlMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangYueyingOlMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraCardEffectRoundsCharacterHuangYueyingOlMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangYueyingOlMouRuntime> _targetFactory = targetFactory;
    public bool IsMouBingcaiExtraRound(long id) => this._targetFactory().IsMouBingcaiExtraRound(id);
    public bool HasUnstartedMouBingcaiExtraEffect(long id) => this._targetFactory().HasUnstartedMouBingcaiExtraEffect(id);
    public void CaptureMouBingcaiExtraEffectAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().CaptureMouBingcaiExtraEffectAttack(attack);
    public void CompleteMouBingcaiExtraEffectRound(long id) => this._targetFactory().CompleteMouBingcaiExtraEffectRound(id);
    public bool TryBeginMouBingcaiExtraEffectRound(long id, bool afterAttack) => this._targetFactory().TryBeginMouBingcaiExtraEffectRound(id, afterAttack);
    public bool ContinueMouBingcaiExtraFangtian(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().ContinueMouBingcaiExtraFangtian(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardTargetEffectsRuntime))]
internal sealed class CoreExtraCardEffectRoundsCoreProgramCardTargetEffectsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardTargetEffectsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraCardEffectRoundsCoreProgramCardTargetEffectsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardTargetEffectsRuntime> _targetFactory = targetFactory;
    public bool IsCardEffectIneffective(long resolutionId, int targetSeat) => this._targetFactory().IsCardEffectIneffective(resolutionId, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSoleIncomingRangeDrawRuntime))]
internal sealed class CoreExtraCardEffectRoundsCoreSoleIncomingRangeDrawAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleIncomingRangeDrawRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraCardEffectRoundsCoreSoleIncomingRangeDrawOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleIncomingRangeDrawRuntime> _targetFactory = targetFactory;
    public void AssertSoleIncomingEffectRounds(global::CardGame.Core.CardUseFrame use) => this._targetFactory().AssertSoleIncomingEffectRounds(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanduExtraEffectsRuntime))]
internal sealed class CoreExtraCardEffectRoundsCoreGuanduExtraEffectsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduExtraEffectsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraCardEffectRoundsCoreGuanduExtraEffectsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduExtraEffectsRuntime> _targetFactory = targetFactory;
    public bool GuanduRepeatTargetsRemainLegal(global::CardGame.Core.CardUseFrame use) => this._targetFactory().GuanduRepeatTargetsRemainLegal(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CoreExtraCardEffectRoundsCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraCardEffectRoundsCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public void ClearPendingDecision() => this._targetFactory().ClearPendingDecision();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime))]
internal sealed class CoreExtraCardEffectRoundsCoreNativeSlashUseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraCardEffectRoundsCoreNativeSlashUseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime> _targetFactory = targetFactory;
    public void ContinueSlashDefense(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().ContinueSlashDefense(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheSchoolTrickRuntime))]
internal sealed class CoreExtraCardEffectRoundsCoreShanheSchoolTrickAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSchoolTrickRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraCardEffectRoundsCoreShanheSchoolTrickOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSchoolTrickRuntime> _targetFactory = targetFactory;
    public bool HasUnstartedShanheSchoolTrick(long id) => this._targetFactory().HasUnstartedShanheSchoolTrick(id);
    public void CompleteShanheSchoolTrick(long id) => this._targetFactory().CompleteShanheSchoolTrick(id);
    public bool TryBeginShanheSchoolTrick(long id, bool afterAttack) => this._targetFactory().TryBeginShanheSchoolTrick(id, afterAttack);
    public void AssertShanheSchoolTrickRounds() => this._targetFactory().AssertShanheSchoolTrickRounds();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime))]
internal sealed class CoreExtraCardEffectRoundsCoreCardUseLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraCardEffectRoundsCoreCardUseLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId) => this._targetFactory().LifecycleCardUse(frameId);
    public void UpdateLifecycleCardUse(long frameId, global::System.Func<global::CardGame.Core.CardUseFrame, global::CardGame.Core.CardUseFrame> update) => this._targetFactory().UpdateLifecycleCardUse(frameId, update);
    public void ContinueSimpleCardUse(long frameId, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => this._targetFactory().ContinueSimpleCardUse(frameId, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime))]
internal sealed class CoreExtraCardEffectRoundsCoreActiveNativeAttackQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraCardEffectRoundsCoreActiveNativeAttackQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActiveNativeAttackQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get => this._targetFactory().ActiveCardAttack; set => this._targetFactory().ActiveCardAttack = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime))]
internal sealed class CoreExtraCardEffectRoundsCoreDamageColorDuelProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraCardEffectRoundsCoreDamageColorDuelProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card GetTrickRepresentation(long frameId, int cardId, bool requireProcessing = false) => this._targetFactory().GetTrickRepresentation(frameId, cardId, requireProcessing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterJiangJiMouRuntime))]
internal sealed class CoreExtraCardEffectRoundsCharacterJiangJiMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiangJiMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraCardEffectRoundsCharacterJiangJiMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiangJiMouRuntime> _targetFactory = targetFactory;
    public bool IsMouJjEffectRound(long id) => this._targetFactory().IsMouJjEffectRound(id);
    public bool HasUnstartedMouJjEffect(long id) => this._targetFactory().HasUnstartedMouJjEffect(id);
    public void CompleteMouJjEffect(long id) => this._targetFactory().CompleteMouJjEffect(id);
    public bool TryBeginMouJjEffect(long id, bool afterAttack) => this._targetFactory().TryBeginMouJjEffect(id, afterAttack);
    public void AssertMouJjEffects() => this._targetFactory().AssertMouJjEffects();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangQuanMouRuntime))]
internal sealed class CoreExtraCardEffectRoundsCharacterHuangQuanMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangQuanMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraCardEffectRoundsCharacterHuangQuanMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangQuanMouRuntime> _targetFactory = targetFactory;
    public bool IsMouHqRound(long id) => this._targetFactory().IsMouHqRound(id);
    public bool HasUnstartedMouHqRound(long id) => this._targetFactory().HasUnstartedMouHqRound(id);
    public void CaptureMouHqAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().CaptureMouHqAttack(attack);
    public void CompleteMouHqRound(long id) => this._targetFactory().CompleteMouHqRound(id);
    public bool TryBeginMouHqRound(long id, bool afterAttack) => this._targetFactory().TryBeginMouHqRound(id, afterAttack);
    public bool ContinueMouHqFangtian(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().ContinueMouHqFangtian(attack);
    public void AssertMouHqRounds() => this._targetFactory().AssertMouHqRounds();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNamedAmbushInvalidationRuntime))]
internal sealed class CoreExtraCardEffectRoundsCoreNamedAmbushInvalidationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNamedAmbushInvalidationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraCardEffectRoundsCoreNamedAmbushInvalidationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNamedAmbushInvalidationRuntime> _targetFactory = targetFactory;
    public bool IsNamedAmbushUseIneffective(global::CardGame.Core.CardUseFrame use) => this._targetFactory().IsNamedAmbushUseIneffective(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNextCardTargetAdjustmentsRuntime))]
internal sealed class CoreExtraCardEffectRoundsCoreNextCardTargetAdjustmentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextCardTargetAdjustmentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraCardEffectRoundsCoreNextCardTargetAdjustmentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextCardTargetAdjustmentsRuntime> _targetFactory = targetFactory;
    public bool HasRemainingAdjustedSimpleTargets(long frameId) => this._targetFactory().HasRemainingAdjustedSimpleTargets(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSoleIncomingEffectRepeatRuntime))]
internal sealed class CoreExtraCardEffectRoundsCoreSoleIncomingEffectRepeatAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleIncomingEffectRepeatRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraCardEffectRoundsCoreSoleIncomingEffectRepeatOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleIncomingEffectRepeatRuntime> _targetFactory = targetFactory;
    public bool IsSoleIncomingExtraRound(long id) => this._targetFactory().IsSoleIncomingExtraRound(id);
    public bool HasUnstartedSoleIncomingEffect(long id) => this._targetFactory().HasUnstartedSoleIncomingEffect(id);
    public void CaptureSoleIncomingAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().CaptureSoleIncomingAttack(attack);
    public void CompleteSoleIncomingExtraRound(long id) => this._targetFactory().CompleteSoleIncomingExtraRound(id);
    public bool TryBeginSoleIncomingExtraRound(long id, bool afterAttack) => this._targetFactory().TryBeginSoleIncomingExtraRound(id, afterAttack);
    public bool ContinueSoleIncomingFangtian(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().ContinueSoleIncomingFangtian(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSequentialTrickTargetsRuntime))]
internal sealed class CoreExtraCardEffectRoundsCoreSequentialTrickTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSequentialTrickTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraCardEffectRoundsCoreSequentialTrickTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSequentialTrickTargetsRuntime> _targetFactory = targetFactory;
    public bool HasRemainingSequentialTrickTargets(long frameId) => this._targetFactory().HasRemainingSequentialTrickTargets(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime))]
internal sealed class CoreExtraCardEffectRoundsCoreNativeCardContinuationCursorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraCardEffectRoundsCoreNativeCardContinuationCursorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.DuelHandle? ActiveDuel { get => this._targetFactory().ActiveDuel; set => this._targetFactory().ActiveDuel = value; }
    public global::CardGame.Core.NativeOwners.FangtianHalberdHandle? ActiveFangtianHalberd { get => this._targetFactory().ActiveFangtianHalberd; set => this._targetFactory().ActiveFangtianHalberd = value; }
    public global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get => this._targetFactory().ActiveGroupCard; set => this._targetFactory().ActiveGroupCard = value; }
    public global::CardGame.Core.NativeOwners.BorrowedSwordHandle? ActiveBorrowedSword { get => this._targetFactory().ActiveBorrowedSword; set => this._targetFactory().ActiveBorrowedSword = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunQuanOlMagicRuntime))]
internal sealed class CoreExtraCardEffectRoundsCharacterSunQuanOlMagicAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanOlMagicRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreExtraCardEffectRoundsCharacterSunQuanOlMagicOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanOlMagicRuntime> _targetFactory = targetFactory;
    public bool IsMoQuanyuExtraRound(long id) => this._targetFactory().IsMoQuanyuExtraRound(id);
    public bool HasUnstartedMoQuanyuExtraRound(long id) => this._targetFactory().HasUnstartedMoQuanyuExtraRound(id);
    public void CaptureMoQuanyuExtraAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().CaptureMoQuanyuExtraAttack(attack);
    public void CompleteMoQuanyuExtraRound(long id) => this._targetFactory().CompleteMoQuanyuExtraRound(id);
    public bool TryBeginMoQuanyuExtraRound(long id, bool afterAttack) => this._targetFactory().TryBeginMoQuanyuExtraRound(id, afterAttack);
    public bool ContinueMoQuanyuFangtian(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().ContinueMoQuanyuFangtian(attack);
}
