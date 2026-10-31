#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFriendDuelResponsesRuntime))]
internal sealed class CoreHpChangeProgramsCoreFriendDuelResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendDuelResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCoreFriendDuelResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendDuelResponsesRuntime> _targetFactory = targetFactory;
    public void ContinueFriendDuelResponse(long id) => this._targetFactory().ContinueFriendDuelResponse(id);
    public bool IsFriendResponseParent(global::CardGame.Core.FriendDuelResponseFrame f) => this._targetFactory().IsFriendResponseParent(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreHpChangeProgramsCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeTop(next);
    public void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame) => this._targetFactory().PushRuntimeFrame(frame);
    public void AdvanceRuntimeTop<TFrame>()
        where TFrame : global::CardGame.Core.ResolutionFrame => this._targetFactory().AdvanceRuntimeTop<TFrame>();
    public void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind) => this._targetFactory().PopResolutionFrame(frameId, expectedKind);
    public void AdvanceRuntimeProgram(long frameId) => this._targetFactory().AdvanceRuntimeProgram(frameId);
    public void ReturnRuntimeProgramMovement(long frameId) => this._targetFactory().ReturnRuntimeProgramMovement(frameId);
    public void AdvanceRuntimeFrame(long frameId) => this._targetFactory().AdvanceRuntimeFrame(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategoryMarkContinuationsRuntime))]
internal sealed class CoreHpChangeProgramsCoreRecipientCategoryMarkContinuationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategoryMarkContinuationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCoreRecipientCategoryMarkContinuationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategoryMarkContinuationsRuntime> _targetFactory = targetFactory;
    public bool IsOriginalDyingSuspendedByRecipientCategoryMark(global::CardGame.Core.DyingFrame dying) => this._targetFactory().IsOriginalDyingSuspendedByRecipientCategoryMark(dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime))]
internal sealed class CoreHpChangeProgramsCharacterGuanYuWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCharacterGuanYuWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime> _targetFactory = targetFactory;
    public bool IsWumiaoFloodHpReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.HpChangedTriggerWindowFrame frame) => this._targetFactory().IsWumiaoFloodHpReturn(use, frame);
    public void ContinueWumiaoFlood(long id) => this._targetFactory().ContinueWumiaoFlood(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardSupplyCompletionRuntime))]
internal sealed class CoreHpChangeProgramsCoreCardSupplyCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardSupplyCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCoreCardSupplyCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardSupplyCompletionRuntime> _targetFactory = targetFactory;
    public bool ReturnCardSupplyCompletionHpChange(global::CardGame.Core.HpChangedTriggerWindowFrame child) => this._targetFactory().ReturnCardSupplyCompletionHpChange(child);
    public bool IsCardSupplyCompletionHealthChild(global::CardGame.Core.ProgramCardTriggerWindowFrame window, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().IsCardSupplyCompletionHealthChild(window, child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunJianMouOlRuntime))]
internal sealed class CoreHpChangeProgramsCharacterSunJianMouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunJianMouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCharacterSunJianMouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunJianMouOlRuntime> _targetFactory = targetFactory;
    public void CaptureMouSunJianOlHp(global::CardGame.Core.HpChangeContext change) => this._targetFactory().CaptureMouSunJianOlHp(change);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeGroupRecoveryRuntime))]
internal sealed class CoreHpChangeProgramsCoreNativeGroupRecoveryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeGroupRecoveryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCoreNativeGroupRecoveryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeGroupRecoveryRuntime> _targetFactory = targetFactory;
    public void CompleteGroupRecoveryTarget() => this._targetFactory().CompleteGroupRecoveryTarget();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterJiangWeiMobileGodRuntime))]
internal sealed class CoreHpChangeProgramsCharacterJiangWeiMobileGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiangWeiMobileGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCharacterJiangWeiMobileGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiangWeiMobileGodRuntime> _targetFactory = targetFactory;
    public bool IsOriginalDyingSuspendedByMobileJwShenpei(global::CardGame.Core.DyingFrame dying) => this._targetFactory().IsOriginalDyingSuspendedByMobileJwShenpei(dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineVirtualBasicOffersRuntime))]
internal sealed class CoreHpChangeProgramsCoreVirtualBasicOffersAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineVirtualBasicOffersRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCoreVirtualBasicOffersOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineVirtualBasicOffersRuntime> _targetFactory = targetFactory;
    public void FinishVirtualBasicUse(long id) => this._targetFactory().FinishVirtualBasicUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSoleIncomingRangeDrawRuntime))]
internal sealed class CoreHpChangeProgramsCoreSoleIncomingRangeDrawAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleIncomingRangeDrawRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCoreSoleIncomingRangeDrawOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleIncomingRangeDrawRuntime> _targetFactory = targetFactory;
    public void ContinueSoleIncomingRangeDraw(long id) => this._targetFactory().ContinueSoleIncomingRangeDraw(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime))]
internal sealed class CoreHpChangeProgramsCoreDyingCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCoreDyingCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DyingFrame? ActiveDying { get => this._targetFactory().ActiveDying; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime))]
internal sealed class CoreHpChangeProgramsCoreDyingSuitsAndHistoricalUseObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCoreDyingSuitsAndHistoricalUseObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime> _targetFactory = targetFactory;
    public bool IsOriginalDyingSuspendedByDyingSuits(global::CardGame.Core.DyingFrame dying) => this._targetFactory().IsOriginalDyingSuspendedByDyingSuits(dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineColorFireAttacksRuntime))]
internal sealed class CoreHpChangeProgramsCoreColorFireAttacksAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineColorFireAttacksRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCoreColorFireAttacksOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineColorFireAttacksRuntime> _targetFactory = targetFactory;
    public void ContinueColorFireAttackPayment(long useId) => this._targetFactory().ContinueColorFireAttackPayment(useId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentRecastsRuntime))]
internal sealed class CoreHpChangeProgramsCoreEquipmentRecastsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentRecastsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCoreEquipmentRecastsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentRecastsRuntime> _targetFactory = targetFactory;
    public void ContinueEquipmentRecast(long frameId) => this._targetFactory().ContinueEquipmentRecast(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFactionRequestCostRecoveriesRuntime))]
internal sealed class CoreHpChangeProgramsCoreFactionRequestCostRecoveriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFactionRequestCostRecoveriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCoreFactionRequestCostRecoveriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFactionRequestCostRecoveriesRuntime> _targetFactory = targetFactory;
    public void ContinuePaidFactionRequestCost(long producerId) => this._targetFactory().ContinuePaidFactionRequestCost(producerId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime))]
internal sealed class CoreHpChangeProgramsCoreGuanduCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCoreGuanduCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime> _targetFactory = targetFactory;
    public bool IsGuanduCardEffectHpReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.HpChangedTriggerWindowFrame frame) => this._targetFactory().IsGuanduCardEffectHpReturn(use, frame);
    public void ContinueGuanduCardEffect(long id) => this._targetFactory().ContinueGuanduCardEffect(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhaoZhongOlRuntime))]
internal sealed class CoreHpChangeProgramsCharacterZhaoZhongOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoZhongOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCharacterZhaoZhongOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoZhongOlRuntime> _targetFactory = targetFactory;
    public void CaptureYintianRecoveryQualification(global::CardGame.Core.HpChangeContext change) => this._targetFactory().CaptureYintianRecoveryQualification(change);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectYintianRecoveryCandidates(global::CardGame.Core.HpChangeContext change) => this._targetFactory().CollectYintianRecoveryCandidates(change);
    public global::CardGame.Core.ProgramSkillWindowContext CreateYintianRecoveryContext(global::CardGame.Core.HpChangeContext change, global::CardGame.Core.ProgramTriggerCandidate candidate) => this._targetFactory().CreateYintianRecoveryContext(change, candidate);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangSkillsBRuntime))]
internal sealed class CoreHpChangeProgramsCoreWushuangSkillsBAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangSkillsBRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCoreWushuangSkillsBOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangSkillsBRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> CollectWushuangBChunlaoLossCandidates(global::CardGame.Core.HpChangeContext change) => this._targetFactory().CollectWushuangBChunlaoLossCandidates(change);
    public global::CardGame.Core.ProgramSkillWindowContext CreateWushuangBChunlaoLossContext(global::CardGame.Core.HpChangeContext change, global::CardGame.Core.ProgramTriggerCandidate c) => this._targetFactory().CreateWushuangBChunlaoLossContext(change, c);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDongBaiOlRuntime))]
internal sealed class CoreHpChangeProgramsCharacterDongBaiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongBaiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCharacterDongBaiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongBaiOlRuntime> _targetFactory = targetFactory;
    public void ObserveDongBaiHpDecreased(int seat, int before, int after) => this._targetFactory().ObserveDongBaiHpDecreased(seat, before, after);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreShenwuNpcProgramsRuntime))]
internal sealed class CoreHpChangeProgramsCoreShenwuNpcProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreShenwuNpcProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCoreShenwuNpcProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreShenwuNpcProgramsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> CollectShenwuRecoveryCandidates(global::CardGame.Core.HpChangeContext change) => this._targetFactory().CollectShenwuRecoveryCandidates(change);
    public global::CardGame.Core.ProgramSkillWindowContext CreateShenwuRecoveryContext(global::CardGame.Core.HpChangeContext change, global::CardGame.Core.ProgramTriggerCandidate candidate) => this._targetFactory().CreateShenwuRecoveryContext(change, candidate);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineQimeiProgramsRuntime))]
internal sealed class CoreHpChangeProgramsCoreQimeiProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineQimeiProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCoreQimeiProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineQimeiProgramsRuntime> _targetFactory = targetFactory;
    public void CaptureQimeiHpQualification(global::CardGame.Core.HpChangeContext change) => this._targetFactory().CaptureQimeiHpQualification(change);
    public bool IsQimeiEqualityTrigger(global::CardGame.Core.SkillProgramTrigger t) => this._targetFactory().IsQimeiEqualityTrigger(t);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectQimeiHpCandidates(global::CardGame.Core.HpChangeContext c) => this._targetFactory().CollectQimeiHpCandidates(c);
    public global::CardGame.Core.ProgramSkillWindowContext CreateQimeiHpContext(global::CardGame.Core.HpChangeContext change, global::CardGame.Core.ProgramTriggerCandidate c) => this._targetFactory().CreateQimeiHpContext(change, c);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime))]
internal sealed class CoreHpChangeProgramsCoreDamageColorDuelProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCoreDamageColorDuelProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card GetTrickRepresentation(long frameId, int cardId, bool requireProcessing = false) => this._targetFactory().GetTrickRepresentation(frameId, cardId, requireProcessing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionHealthRuntime))]
internal sealed class CoreHpChangeProgramsCoreResponseCompletionHealthAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionHealthRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCoreResponseCompletionHealthOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionHealthRuntime> _targetFactory = targetFactory;
    public bool ReturnCardResponseCompletionHpChange(global::CardGame.Core.HpChangedTriggerWindowFrame child) => this._targetFactory().ReturnCardResponseCompletionHpChange(child);
    public bool IsResponseCompletionHealthChild(global::CardGame.Core.ProgramCardTriggerWindowFrame window, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().IsResponseCompletionHealthChild(window, child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardUseCompletionRuntime))]
internal sealed class CoreHpChangeProgramsCoreNativeCardUseCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCoreNativeCardUseCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseCompletionRuntime> _targetFactory = targetFactory;
    public void FinishCardUse(long frameId, global::CardGame.Core.Card card, global::CardGame.Core.CardKind? playedCardKind = default) => this._targetFactory().FinishCardUse(frameId, card, playedCardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramRecoveryReceiptsRuntime))]
internal sealed class CoreHpChangeProgramsCoreProgramRecoveryReceiptsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramRecoveryReceiptsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCoreProgramRecoveryReceiptsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramRecoveryReceiptsRuntime> _targetFactory = targetFactory;
    public void ClearProgramRecoveryApplication(global::CardGame.Core.HpChangeContext change) => this._targetFactory().ClearProgramRecoveryApplication(change);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoPiAnniversaryGodRuntime))]
internal sealed class CoreHpChangeProgramsCharacterCaoPiAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoPiAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCharacterCaoPiAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoPiAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool KemingSkillEchoFrameRidesOn(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ResolutionFrame parent) => this._targetFactory().KemingSkillEchoFrameRidesOn(child, parent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWangCanOlRuntime))]
internal sealed class CoreHpChangeProgramsCharacterWangCanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangCanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCharacterWangCanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangCanOlRuntime> _targetFactory = targetFactory;
    public void ContinueDenglouNativePayment(long id) => this._targetFactory().ContinueDenglouNativePayment(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineForeignSelectedCardSlashReturnsRuntime))]
internal sealed class CoreHpChangeProgramsCoreForeignSelectedCardSlashReturnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignSelectedCardSlashReturnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCoreForeignSelectedCardSlashReturnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignSelectedCardSlashReturnsRuntime> _targetFactory = targetFactory;
    public bool IsSelectedForeignCardSlashHpChange(long resumeFrameId, global::CardGame.Core.HpChangeContext change) => this._targetFactory().IsSelectedForeignCardSlashHpChange(resumeFrameId, change);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreHpChangeProgramsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectProgramTriggerCandidates(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, int occurrenceIndex = 0) => this._targetFactory().CollectProgramTriggerCandidates(owner, window, occurrenceIndex);
    public global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate) => this._targetFactory().GetProgramTrigger(candidate);
    public global::CardGame.Core.SkillProgramTriggerFacts CaptureProgramTriggerFacts(global::CardGame.Core.CharacterState owner) => this._targetFactory().CaptureProgramTriggerFacts(owner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTurnRedSlashBenefitsRuntime))]
internal sealed class CoreHpChangeProgramsCoreTurnRedSlashBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnRedSlashBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCoreTurnRedSlashBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnRedSlashBenefitsRuntime> _targetFactory = targetFactory;
    public bool HasTurnRedSlashCapability { get => this._targetFactory().HasTurnRedSlashCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNianActivityProgramsRuntime))]
internal sealed class CoreHpChangeProgramsCoreNianActivityProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNianActivityProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCoreNianActivityProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNianActivityProgramsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> CollectNianRecoveryCandidates(global::CardGame.Core.HpChangeContext change) => this._targetFactory().CollectNianRecoveryCandidates(change);
    public global::CardGame.Core.ProgramSkillWindowContext CreateNianRecoveryContext(global::CardGame.Core.HpChangeContext change, global::CardGame.Core.ProgramTriggerCandidate candidate) => this._targetFactory().CreateNianRecoveryContext(change, candidate);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime))]
internal sealed class CoreHpChangeProgramsCoreRecoveryReplacementAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCoreRecoveryReplacementOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> _targetFactory = targetFactory;
    public bool TryBeginQueuedRecoveryReplacement(long? parentFrameId, global::CardGame.Core.PostEventContinuation continuation, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default) => this._targetFactory().TryBeginQueuedRecoveryReplacement(parentFrameId, continuation, cardId, cardKind);
    public void ContinueRecoveryPaidCardUse(long useId) => this._targetFactory().ContinueRecoveryPaidCardUse(useId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXiZhiCaiOlRuntime))]
internal sealed class CoreHpChangeProgramsCharacterXiZhiCaiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiZhiCaiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCharacterXiZhiCaiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiZhiCaiOlRuntime> _targetFactory = targetFactory;
    public void CaptureXianfuRecoveryQualification(global::CardGame.Core.HpChangeContext change) => this._targetFactory().CaptureXianfuRecoveryQualification(change);
    public bool IsXianfuEchoTrigger(global::CardGame.Core.SkillProgramTrigger trigger) => this._targetFactory().IsXianfuEchoTrigger(trigger);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectXianfuHpCandidates(global::CardGame.Core.HpChangeContext change) => this._targetFactory().CollectXianfuHpCandidates(change);
    public global::CardGame.Core.ProgramSkillWindowContext CreateXianfuHpContext(global::CardGame.Core.HpChangeContext change, global::CardGame.Core.ProgramTriggerCandidate c) => this._targetFactory().CreateXianfuHpContext(change, c);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime))]
internal sealed class CoreHpChangeProgramsCoreDragonBoatCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCoreDragonBoatCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime> _targetFactory = targetFactory;
    public bool IsDragonBoatCardEffectHpReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.HpChangedTriggerWindowFrame hp) => this._targetFactory().IsDragonBoatCardEffectHpReturn(use, hp);
    public void ContinueDragonBoatCardEffect(long id) => this._targetFactory().ContinueDragonBoatCardEffect(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEndingHistoricalUsesRuntime))]
internal sealed class CoreHpChangeProgramsCoreEndingHistoricalUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEndingHistoricalUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCoreEndingHistoricalUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEndingHistoricalUsesRuntime> _targetFactory = targetFactory;
    public void ContinueHistoricalEndingPayment(long id) => this._targetFactory().ContinueHistoricalEndingPayment(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramWindowCursorRuntime))]
internal sealed class CoreHpChangeProgramsProgramWindowCursorAdapter(global::System.Func<global::CardGame.Core.ProgramWindowCursorRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsProgramWindowCursorOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramWindowCursorRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramWindowCursorRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime))]
internal sealed class CoreHpChangeProgramsCoreCardMovementProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCoreCardMovementProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> _targetFactory = targetFactory;
    public bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default) => this._targetFactory().TryBeginCardsMovedProgramWindow(instructionFrameId, subjectSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime))]
internal sealed class CoreHpChangeProgramsCoreNativeCardContinuationCursorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCoreNativeCardContinuationCursorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get => this._targetFactory().ActiveGroupCard; set => this._targetFactory().ActiveGroupCard = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWenyiDamageHealthRuntime))]
internal sealed class CoreHpChangeProgramsCoreWenyiDamageHealthAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenyiDamageHealthRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCoreWenyiDamageHealthOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenyiDamageHealthRuntime> _targetFactory = targetFactory;
    public void ReturnWenyiDamageHealth(global::CardGame.Core.HpChangedTriggerWindowFrame health) => this._targetFactory().ReturnWenyiDamageHealth(health);
    public bool IsWenyiDamageHealthReturn(global::CardGame.Core.DamageFrame damage, global::CardGame.Core.HpChangedTriggerWindowFrame health) => this._targetFactory().IsWenyiDamageHealthReturn(damage, health);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuaTuoAnniversaryGodRuntime))]
internal sealed class CoreHpChangeProgramsCharacterHuaTuoAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaTuoAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCharacterHuaTuoAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaTuoAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool IsSkillInvocationHealthChild(global::CardGame.Core.SkillInvocationFrame f, global::CardGame.Core.HpChangedTriggerWindowFrame hp) => this._targetFactory().IsSkillInvocationHealthChild(f, hp);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardEffectDrawsRuntime))]
internal sealed class CoreHpChangeProgramsCoreNativeCardEffectDrawsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardEffectDrawsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCoreNativeCardEffectDrawsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardEffectDrawsRuntime> _targetFactory = targetFactory;
    public void ContinueNativeDrawTwo(long id) => this._targetFactory().ContinueNativeDrawTwo(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreHpChangeProgramsCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangFeiWumiaoRuntime))]
internal sealed class CoreHpChangeProgramsCharacterZhangFeiWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCharacterZhangFeiWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiWumiaoRuntime> _targetFactory = targetFactory;
    public bool IsOriginalDyingSuspendedByWuZishengMovement(global::CardGame.Core.DyingFrame dying) => this._targetFactory().IsOriginalDyingSuspendedByWuZishengMovement(dying);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangGaiMouMobileRuntime))]
internal sealed class CoreHpChangeProgramsCharacterHuangGaiMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangGaiMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCharacterHuangGaiMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangGaiMouMobileRuntime> _targetFactory = targetFactory;
    public void CaptureMouKurouNativeLoss(global::CardGame.Core.HpChangeContext change) => this._targetFactory().CaptureMouKurouNativeLoss(change);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime))]
internal sealed class CoreHpChangeProgramsCharacterDaQiaoXiaoQiaoHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCharacterDaQiaoXiaoQiaoHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.HappyQiaoPool? CurrentHappyQiaoPool(int seat, long? parentFrameId = default, bool healthWrite = false) => this._targetFactory().CurrentHappyQiaoPool(seat, parentFrameId, healthWrite);
    public void ReturnHappyQiaoDamageHealth(global::CardGame.Core.HpChangedTriggerWindowFrame health) => this._targetFactory().ReturnHappyQiaoDamageHealth(health);
    public void ReturnHappyQiaoCardMaximumHealth(global::CardGame.Core.HpChangedTriggerWindowFrame health) => this._targetFactory().ReturnHappyQiaoCardMaximumHealth(health);
    public bool IsHappyQiaoDamageHealthReturn(global::CardGame.Core.DamageFrame damage, global::CardGame.Core.HpChangedTriggerWindowFrame health) => this._targetFactory().IsHappyQiaoDamageHealthReturn(damage, health);
    public bool IsHappyQiaoCardMaximumHealthReturn(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.HpChangedTriggerWindowFrame health) => this._targetFactory().IsHappyQiaoCardMaximumHealthReturn(parent, health);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicPaymentRuntime))]
internal sealed class CoreHpChangeProgramsCoreDrawFundedDistinctBasicPaymentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicPaymentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCoreDrawFundedDistinctBasicPaymentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicPaymentRuntime> _targetFactory = targetFactory;
    public void BeginDrawFundedDistinctBasicChild(long ownerFrameId, long childFrameId) => this._targetFactory().BeginDrawFundedDistinctBasicChild(ownerFrameId, childFrameId);
    public void ResumeDrawFundedDistinctBasicHpChange(global::CardGame.Core.HpChangedTriggerWindowFrame completed) => this._targetFactory().ResumeDrawFundedDistinctBasicHpChange(completed);
    public bool IsDrawFundedDistinctBasicHpParent(global::CardGame.Core.ResolutionFrame? parent, global::CardGame.Core.HpChangedTriggerWindowFrame child) => this._targetFactory().IsDrawFundedDistinctBasicHpParent(parent, child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWenhePoisonRuntime))]
internal sealed class CoreHpChangeProgramsCoreWenhePoisonAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenhePoisonRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCoreWenhePoisonOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenhePoisonRuntime> _targetFactory = targetFactory;
    public bool IsWenhePoisonHealthChild(global::CardGame.Core.ResolutionFrame? parent, global::CardGame.Core.HpChangedTriggerWindowFrame child) => this._targetFactory().IsWenhePoisonHealthChild(parent, child);
    public void AssertWenhePoisonFireAttackPayment(global::CardGame.Core.CardUseFrame use) => this._targetFactory().AssertWenhePoisonFireAttackPayment(use);
    public bool IsPendingWenhePoisonWindow(global::CardGame.Core.CardsMovedTriggerWindowFrame frame) => this._targetFactory().IsPendingWenhePoisonWindow(frame);
    public void AssertWenhePoisonMovementWindow(global::CardGame.Core.CardsMovedTriggerWindowFrame frame) => this._targetFactory().AssertWenhePoisonMovementWindow(frame);
    public bool IsWenhePoisonFireAttackMovement(global::CardGame.Core.CardUseFrame parent, global::CardGame.Core.CardsMovedTriggerWindowFrame child) => this._targetFactory().IsWenhePoisonFireAttackMovement(parent, child);
    public bool IsOriginalDyingSuspendedByWenhePoison(global::CardGame.Core.DyingFrame original) => this._targetFactory().IsOriginalDyingSuspendedByWenhePoison(original);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunShangxiangSpOlRuntime))]
internal sealed class CoreHpChangeProgramsCharacterSunShangxiangSpOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunShangxiangSpOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCharacterSunShangxiangSpOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunShangxiangSpOlRuntime> _targetFactory = targetFactory;
    public void CaptureSpSunShangxiangRecovery(global::CardGame.Core.HpChangeContext change) => this._targetFactory().CaptureSpSunShangxiangRecovery(change);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectSpSunShangxiangRecoveryCandidates(global::CardGame.Core.HpChangeContext change) => this._targetFactory().CollectSpSunShangxiangRecoveryCandidates(change);
    public global::CardGame.Core.ProgramSkillWindowContext CreateSpSunShangxiangRecoveryContext(global::CardGame.Core.HpChangeContext change, global::CardGame.Core.ProgramTriggerCandidate c) => this._targetFactory().CreateSpSunShangxiangRecoveryContext(change, c);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineUnrespondableCounterspellsRuntime))]
internal sealed class CoreHpChangeProgramsCoreUnrespondableCounterspellsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnrespondableCounterspellsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCoreUnrespondableCounterspellsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnrespondableCounterspellsRuntime> _targetFactory = targetFactory;
    public void ContinuePolicyCounterspellPayment(long windowId) => this._targetFactory().ContinuePolicyCounterspellPayment(windowId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangQuanOlRuntime))]
internal sealed class CoreHpChangeProgramsCharacterHuangQuanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangQuanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCharacterHuangQuanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangQuanOlRuntime> _targetFactory = targetFactory;
    public void CaptureDianhuRecoveryQualification(global::CardGame.Core.HpChangeContext change) => this._targetFactory().CaptureDianhuRecoveryQualification(change);
    public bool IsDianhuDrawTrigger(global::CardGame.Core.SkillProgramTrigger t) => this._targetFactory().IsDianhuDrawTrigger(t);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectDianhuHpCandidates(global::CardGame.Core.HpChangeContext change) => this._targetFactory().CollectDianhuHpCandidates(change);
    public global::CardGame.Core.ProgramSkillWindowContext CreateDianhuHpContext(global::CardGame.Core.HpChangeContext change, global::CardGame.Core.ProgramTriggerCandidate c) => this._targetFactory().CreateDianhuHpContext(change, c);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoSongOlRuntime))]
internal sealed class CoreHpChangeProgramsCharacterCaoSongOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoSongOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCharacterCaoSongOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoSongOlRuntime> _targetFactory = targetFactory;
    public bool IsAmountSupportHealthChild(global::CardGame.Core.ResolutionFrame? parent, global::CardGame.Core.HpChangedTriggerWindowFrame hp) => this._targetFactory().IsAmountSupportHealthChild(parent, hp);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHuxinMirrorRuntime))]
internal sealed class CoreHpChangeProgramsCoreHuxinMirrorAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHuxinMirrorRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCoreHuxinMirrorOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHuxinMirrorRuntime> _targetFactory = targetFactory;
    public void ContinueHuxinMirrorPlacement(long id) => this._targetFactory().ContinueHuxinMirrorPlacement(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime))]
internal sealed class CoreHpChangeProgramsCharacterHuanShuJunOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCharacterHuanShuJunOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime> _targetFactory = targetFactory;
    public bool IsWenyiHpTrigger(global::CardGame.Core.SkillProgramTrigger t) => this._targetFactory().IsWenyiHpTrigger(t);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectWenyiHpCandidates(global::CardGame.Core.HpChangeContext change) => this._targetFactory().CollectWenyiHpCandidates(change);
    public global::CardGame.Core.ProgramSkillWindowContext CreateWenyiHpContext(global::CardGame.Core.HpChangeContext change, global::CardGame.Core.ProgramTriggerCandidate c) => this._targetFactory().CreateWenyiHpContext(change, c);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuoSiOlRuntime))]
internal sealed class CoreHpChangeProgramsCharacterGuoSiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoSiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCharacterGuoSiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoSiOlRuntime> _targetFactory = targetFactory;
    public void ContinueGuoSiNativePayment(long id) => this._targetFactory().ContinueGuoSiNativePayment(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime))]
internal sealed class CoreHpChangeProgramsCoreResolutionIdentityBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCoreResolutionIdentityBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> _targetFactory = targetFactory;
    public long _resolutionSequence { get => this._targetFactory()._resolutionSequence; set => this._targetFactory()._resolutionSequence = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWenheCardsRuntime))]
internal sealed class CoreHpChangeProgramsCoreWenheCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenheCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHpChangeProgramsCoreWenheCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenheCardsRuntime> _targetFactory = targetFactory;
    public bool IsWenheCardEffectHpReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.HpChangedTriggerWindowFrame hp) => this._targetFactory().IsWenheCardEffectHpReturn(use, hp);
    public void ContinueWenheCardEffect(long id) => this._targetFactory().ContinueWenheCardEffect(id);
}
