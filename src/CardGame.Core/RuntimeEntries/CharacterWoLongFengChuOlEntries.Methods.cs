#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWoLongFengChuOlEntries
{
    internal void BeginEquipmentSlotNamedUse(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.CardKind kind, global::CardGame.Core.EquipmentSlotNamedUseIntent intent, global::CardGame.Core.PendingDecision original, global::CardGame.Core.LegalAction? action, global::CardGame.Core.PromptChoice? choice) => _resolve().BeginEquipmentSlotNamedUse(actor, source, kind, intent, original, action, choice);
    internal bool CanOfferLuanfeng(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferLuanfeng(candidate, trigger, context);
    internal bool ContinuePaidWoLongFengChu(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ContinuePaidWoLongFengChu(frame);
    internal bool DrainEquipmentSlotNamedChildren(long id) => _resolve().DrainEquipmentSlotNamedChildren(id);
    internal bool EquipmentSlotNamedChoiceIsPayable(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.CardKind kind, global::CardGame.Core.EquipmentSlotNamedUseIntent intent, global::System.Collections.Generic.IReadOnlyList<int> targets, global::CardGame.Core.EquipmentSlot slot) => _resolve().EquipmentSlotNamedChoiceIsPayable(actor, source, kind, intent, targets, slot);
    internal bool EquipmentSlotNamedNameUsed(int seat, global::CardGame.Core.CardKind kind) => _resolve().EquipmentSlotNamedNameUsed(seat, kind);
    internal bool EquipmentSlotNamedPlayStillLegal(global::CardGame.Core.CharacterState actor, global::CardGame.Core.EquipmentSlotNamedUseDraft draft) => _resolve().EquipmentSlotNamedPlayStillLegal(actor, draft);
    internal bool EquipmentSlotNamedYin(int seat, string skill, string instance) => _resolve().EquipmentSlotNamedYin(seat, skill, instance);
    internal bool HideInternalEquipmentSlotPayment(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramActivation activation) => _resolve().HideInternalEquipmentSlotPayment(owner, activation);
    internal void InitializeEquipmentSlotNamedNativeTier() => _resolve().InitializeEquipmentSlotNamedNativeTier();
    internal bool IsEquipmentSlotNamedActionPayable(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action) => _resolve().IsEquipmentSlotNamedActionPayable(actor, action);
    internal bool IsEquipmentSlotNamedForcedSlashPayable(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.CardKind kind, global::CardGame.Core.CharacterState target, bool qinglong) => _resolve().IsEquipmentSlotNamedForcedSlashPayable(actor, source, kind, target, qinglong);
    internal bool IsEquipmentSlotNamedSkill(string skill) => _resolve().IsEquipmentSlotNamedSkill(skill);
    internal bool IsLuanfengRecoveryParent(global::CardGame.Core.ProgramSkillFrame producer, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsLuanfengRecoveryParent(producer, recovery);
    internal void ObserveEquipmentSlotNamedNativeIssue(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveEquipmentSlotNamedNativeIssue(payload);
    internal global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardConversionSource Source, global::CardGame.Core.SkillProgramViewAs Rule, string Hash)> PaidEquipmentSlotNamedZeroRules(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind? output, bool responseUse, bool dyingUse) => _resolve().PaidEquipmentSlotNamedZeroRules(owner, output, responseUse, dyingUse);
    internal void PayEquipmentSlotNamedUse(global::CardGame.Core.PromptChoice choice) => _resolve().PayEquipmentSlotNamedUse(choice);
    internal void PublishEquipmentSlotNamedChoice(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishEquipmentSlotNamedChoice(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveLuanfeng(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ResolveLuanfeng(supplied);
    internal bool ResumeEquipmentSlotNamedUse(long id) => _resolve().ResumeEquipmentSlotNamedUse(id);
    internal bool ResumeLuanfeng(long id) => _resolve().ResumeLuanfeng(id);
    internal bool ReturnEquipmentSlotNamedUseMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnEquipmentSlotNamedUseMovement(frame);
    internal bool ReturnLuanfengMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnLuanfengMovement(frame);
    internal global::CardGame.Core.PromptChoice SelectAiEquipmentSlotNamedUse(global::CardGame.Core.PendingDecision prompt, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiEquipmentSlotNamedUse(prompt, frame);
    internal bool TryAdvanceWoLongFengChuSubtree() => _resolve().TryAdvanceWoLongFengChuSubtree();
    internal bool TryBeginEquipmentSlotNamedForcedSlash(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.CardKind kind, global::CardGame.Core.PromptChoice choice, bool qinglong) => _resolve().TryBeginEquipmentSlotNamedForcedSlash(actor, source, kind, choice, qinglong);
    internal bool TryBeginEquipmentSlotNamedPlay(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action) => _resolve().TryBeginEquipmentSlotNamedPlay(actor, action);
    internal bool TryBeginEquipmentSlotNamedResponse(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.PromptChoice choice, bool dyingUse) => _resolve().TryBeginEquipmentSlotNamedResponse(actor, source, choice, dyingUse);
    internal bool TryConsumePaidEquipmentSlotNamedConversion(global::CardGame.Core.CardConversionSource source) => _resolve().TryConsumePaidEquipmentSlotNamedConversion(source);
    internal bool TryGetEquipmentSlotNamedConversionAvailability(global::CardGame.Core.CharacterState actor, global::CardGame.Core.IndexedSkillProgramInstance instance, global::CardGame.Core.SkillProgramViewAs rule, bool responseUse, bool dyingUse, scoped out bool available) => _resolve().TryGetEquipmentSlotNamedConversionAvailability(actor, instance, rule, responseUse, dyingUse, out available);
    internal global::CardGame.Core.EquipmentSlotNamedUsePayment? UnconsumedEquipmentSlotPayment(global::CardGame.Core.CardConversionSource source) => _resolve().UnconsumedEquipmentSlotPayment(source);
    internal bool WoLongFengChuFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().WoLongFengChuFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? WoLongFengChuObserverRoot() => _resolve().WoLongFengChuObserverRoot();
}
