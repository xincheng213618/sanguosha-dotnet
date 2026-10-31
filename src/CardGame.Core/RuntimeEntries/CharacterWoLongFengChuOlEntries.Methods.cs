#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWoLongFengChuOlEntries
{
    internal void BeginEquipmentSlotNamedUse(CharacterState actor, CardConversionSource source, CardKind kind, EquipmentSlotNamedUseIntent intent, PendingDecision original, LegalAction? action, PromptChoice? choice) => _resolve().BeginEquipmentSlotNamedUse(actor, source, kind, intent, original, action, choice);
    internal bool CanOfferLuanfeng(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferLuanfeng(candidate, trigger, context);
    internal bool ContinuePaidWoLongFengChu(ProgramSkillFrame frame) => _resolve().ContinuePaidWoLongFengChu(frame);
    internal bool DrainEquipmentSlotNamedChildren(long id) => _resolve().DrainEquipmentSlotNamedChildren(id);
    internal bool EquipmentSlotNamedChoiceIsPayable(CharacterState actor, CardConversionSource source, CardKind kind, EquipmentSlotNamedUseIntent intent, IReadOnlyList<int> targets, EquipmentSlot slot) => _resolve().EquipmentSlotNamedChoiceIsPayable(actor, source, kind, intent, targets, slot);
    internal bool EquipmentSlotNamedNameUsed(int seat, CardKind kind) => _resolve().EquipmentSlotNamedNameUsed(seat, kind);
    internal bool EquipmentSlotNamedPlayStillLegal(CharacterState actor, EquipmentSlotNamedUseDraft draft) => _resolve().EquipmentSlotNamedPlayStillLegal(actor, draft);
    internal bool EquipmentSlotNamedYin(int seat, string skill, string instance) => _resolve().EquipmentSlotNamedYin(seat, skill, instance);
    internal bool HideInternalEquipmentSlotPayment(CharacterState owner, SkillProgramActivation activation) => _resolve().HideInternalEquipmentSlotPayment(owner, activation);
    internal void InitializeEquipmentSlotNamedNativeTier() => _resolve().InitializeEquipmentSlotNamedNativeTier();
    internal bool IsEquipmentSlotNamedActionPayable(CharacterState actor, LegalAction action) => _resolve().IsEquipmentSlotNamedActionPayable(actor, action);
    internal bool IsEquipmentSlotNamedForcedSlashPayable(CharacterState actor, CardConversionSource source, CardKind kind, CharacterState target, bool qinglong) => _resolve().IsEquipmentSlotNamedForcedSlashPayable(actor, source, kind, target, qinglong);
    internal bool IsEquipmentSlotNamedSkill(string skill) => _resolve().IsEquipmentSlotNamedSkill(skill);
    internal bool IsLuanfengRecoveryParent(ProgramSkillFrame producer, RecoveryReplacementFrame recovery) => _resolve().IsLuanfengRecoveryParent(producer, recovery);
    internal void ObserveEquipmentSlotNamedNativeIssue(IGameEvent payload) => _resolve().ObserveEquipmentSlotNamedNativeIssue(payload);
    internal IEnumerable<(CardConversionSource Source, SkillProgramViewAs Rule, string Hash)> PaidEquipmentSlotNamedZeroRules(CharacterState owner, CardKind? output, bool responseUse, bool dyingUse) => _resolve().PaidEquipmentSlotNamedZeroRules(owner, output, responseUse, dyingUse);
    internal void PayEquipmentSlotNamedUse(PromptChoice choice) => _resolve().PayEquipmentSlotNamedUse(choice);
    internal void PublishEquipmentSlotNamedChoice(ProgramSkillFrame frame) => _resolve().PublishEquipmentSlotNamedChoice(frame);
    internal SkillProgramStepOutcome ResolveLuanfeng(ProgramSkillFrame supplied) => _resolve().ResolveLuanfeng(supplied);
    internal bool ResumeEquipmentSlotNamedUse(long id) => _resolve().ResumeEquipmentSlotNamedUse(id);
    internal bool ResumeLuanfeng(long id) => _resolve().ResumeLuanfeng(id);
    internal bool ReturnEquipmentSlotNamedUseMovement(ProgramSkillFrame frame) => _resolve().ReturnEquipmentSlotNamedUseMovement(frame);
    internal bool ReturnLuanfengMovement(ProgramSkillFrame frame) => _resolve().ReturnLuanfengMovement(frame);
    internal PromptChoice SelectAiEquipmentSlotNamedUse(PendingDecision prompt, ProgramSkillFrame frame) => _resolve().SelectAiEquipmentSlotNamedUse(prompt, frame);
    internal bool TryAdvanceWoLongFengChuSubtree() => _resolve().TryAdvanceWoLongFengChuSubtree();
    internal bool TryBeginEquipmentSlotNamedForcedSlash(CharacterState actor, CardConversionSource source, CardKind kind, PromptChoice choice, bool qinglong) => _resolve().TryBeginEquipmentSlotNamedForcedSlash(actor, source, kind, choice, qinglong);
    internal bool TryBeginEquipmentSlotNamedPlay(CharacterState actor, LegalAction action) => _resolve().TryBeginEquipmentSlotNamedPlay(actor, action);
    internal bool TryBeginEquipmentSlotNamedResponse(CharacterState actor, CardConversionSource source, PromptChoice choice, bool dyingUse) => _resolve().TryBeginEquipmentSlotNamedResponse(actor, source, choice, dyingUse);
    internal bool TryConsumePaidEquipmentSlotNamedConversion(CardConversionSource source) => _resolve().TryConsumePaidEquipmentSlotNamedConversion(source);
    internal bool TryGetEquipmentSlotNamedConversionAvailability(CharacterState actor, IndexedSkillProgramInstance instance, SkillProgramViewAs rule, bool responseUse, bool dyingUse, scoped out bool available) => _resolve().TryGetEquipmentSlotNamedConversionAvailability(actor, instance, rule, responseUse, dyingUse, out available);
    internal EquipmentSlotNamedUsePayment? UnconsumedEquipmentSlotPayment(CardConversionSource source) => _resolve().UnconsumedEquipmentSlotPayment(source);
    internal bool WoLongFengChuFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().WoLongFengChuFirstChild(root, child);
    internal ProgramSkillFrame? WoLongFengChuObserverRoot() => _resolve().WoLongFengChuObserverRoot();
}
