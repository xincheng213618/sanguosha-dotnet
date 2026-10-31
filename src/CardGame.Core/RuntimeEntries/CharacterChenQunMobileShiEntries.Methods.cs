#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChenQunMobileShiEntries
{
    internal void AnswerMobileShiChenQun(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerMobileShiChenQun(choice);
    internal void BindMobileShiChenQunEffect(global::CardGame.Core.ProgramSkillFrame frame, string effect) => _resolve().BindMobileShiChenQunEffect(frame, effect);
    internal bool CanOfferMobileShiChenQun(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferMobileShiChenQun(candidate, trigger, context);
    internal global::CardGame.Core.PromptChoice ChooseMobileShiChenQunAi(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ChooseMobileShiChenQunAi(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome ClaimMobileShiChenQunFaenDebt(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ClaimMobileShiChenQunFaenDebt(supplied);
    internal void IssueMobileShiChenQunFaenDebt(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().IssueMobileShiChenQunFaenDebt(supplied);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MobileShiChenQunChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().MobileShiChenQunChoices(frame);
    internal (global::CardGame.Core.ProgramCardTriggerWindowFrame Window, global::CardGame.Core.MobileShiChenQunUseRecordedEvent Use)? MobileShiChenQunCurrentUse(global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().MobileShiChenQunCurrentUse(context);
    internal global::CardGame.Core.MobileShiChenQunFaenDebtIssuedEvent? MobileShiChenQunDebt(int owner, string instance, string hash, int sequence) => _resolve().MobileShiChenQunDebt(owner, instance, hash, sequence);
    internal global::System.Collections.Generic.IReadOnlyList<int> MobileShiChenQunMaximumLossSeats() => _resolve().MobileShiChenQunMaximumLossSeats();
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiChenQunPublicStates(global::CardGame.Core.CharacterState owner, string skillId) => _resolve().MobileShiChenQunPublicStates(owner, skillId);
    internal void ObserveMobileShiChenQunUse(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveMobileShiChenQunUse(payload);
    internal void PublishMobileShiChenQunChoice(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishMobileShiChenQunChoice(frame);
    internal void RecordMobileShiChenQunUse(long native, long? action, int actor) => _resolve().RecordMobileShiChenQunUse(native, action, actor);
    internal void RequireMobileShiChenQunFrame(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().RequireMobileShiChenQunFrame(frame);
    internal bool HasMobileShiChenQunFaen { get => _resolve().HasMobileShiChenQunFaen; }
}
