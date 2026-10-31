#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChenQunMobileShiEntries
{
    internal void AnswerMobileShiChenQun(PromptChoice choice) => _resolve().AnswerMobileShiChenQun(choice);
    internal void BindMobileShiChenQunEffect(ProgramSkillFrame frame, string effect) => _resolve().BindMobileShiChenQunEffect(frame, effect);
    internal bool CanOfferMobileShiChenQun(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferMobileShiChenQun(candidate, trigger, context);
    internal PromptChoice ChooseMobileShiChenQunAi(ProgramSkillFrame frame) => _resolve().ChooseMobileShiChenQunAi(frame);
    internal SkillProgramStepOutcome ClaimMobileShiChenQunFaenDebt(ProgramSkillFrame supplied) => _resolve().ClaimMobileShiChenQunFaenDebt(supplied);
    internal void IssueMobileShiChenQunFaenDebt(ProgramSkillFrame supplied) => _resolve().IssueMobileShiChenQunFaenDebt(supplied);
    internal IReadOnlyList<PromptChoice> MobileShiChenQunChoices(ProgramSkillFrame frame) => _resolve().MobileShiChenQunChoices(frame);
    internal (ProgramCardTriggerWindowFrame Window, MobileShiChenQunUseRecordedEvent Use)? MobileShiChenQunCurrentUse(ProgramSkillWindowContext context) => _resolve().MobileShiChenQunCurrentUse(context);
    internal MobileShiChenQunFaenDebtIssuedEvent? MobileShiChenQunDebt(int owner, string instance, string hash, int sequence) => _resolve().MobileShiChenQunDebt(owner, instance, hash, sequence);
    internal IReadOnlyList<int> MobileShiChenQunMaximumLossSeats() => _resolve().MobileShiChenQunMaximumLossSeats();
    internal IReadOnlyList<ProgramPublicRuleStateSnapshot> MobileShiChenQunPublicStates(CharacterState owner, string skillId) => _resolve().MobileShiChenQunPublicStates(owner, skillId);
    internal void ObserveMobileShiChenQunUse(IGameEvent payload) => _resolve().ObserveMobileShiChenQunUse(payload);
    internal void PublishMobileShiChenQunChoice(ProgramSkillFrame frame) => _resolve().PublishMobileShiChenQunChoice(frame);
    internal void RecordMobileShiChenQunUse(long native, long? action, int actor) => _resolve().RecordMobileShiChenQunUse(native, action, actor);
    internal void RequireMobileShiChenQunFrame(ProgramSkillFrame frame) => _resolve().RequireMobileShiChenQunFrame(frame);
    internal bool HasMobileShiChenQunFaen { get => _resolve().HasMobileShiChenQunFaen; }
}
