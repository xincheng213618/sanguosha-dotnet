#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuangChongMobileShiEntries
{
    internal void AnswerShiHc(PromptChoice selected) => _resolve().AnswerShiHc(selected);
    internal void AnswerShiHcCujunTarget(ProgramSkillFrame f, ShiHcReceipt r, string branch) => _resolve().AnswerShiHcCujunTarget(f, r, branch);
    internal void AnswerShiHcSiliBranch(ProgramSkillFrame f, ShiHcReceipt r, string branch) => _resolve().AnswerShiHcSiliBranch(f, r, branch);
    internal SkillProgramStepOutcome BeginShiHcCujun(ProgramSkillFrame input) => _resolve().BeginShiHcCujun(input);
    internal SkillProgramStepOutcome BeginShiHcSili(ProgramSkillFrame input) => _resolve().BeginShiHcSili(input);
    internal bool CanOfferShiHcCujun(ProgramTriggerLegalityContext c) => _resolve().CanOfferShiHcCujun(c);
    internal bool CanOfferShiHcSili(ProgramTriggerLegalityContext c) => _resolve().CanOfferShiHcSili(c);
    internal PromptChoice ChooseShiHcCujunAi(PendingDecision decision, ProgramSkillFrame f) => _resolve().ChooseShiHcCujunAi(decision, f);
    internal PromptChoice ChooseShiHcSiliAi(PendingDecision decision, ProgramSkillFrame f) => _resolve().ChooseShiHcSiliAi(decision, f);
    internal void ContinueShiHcChoosers(ProgramSkillFrame f) => _resolve().ContinueShiHcChoosers(f);
    internal void ContinueShiHcPrompt(ProgramSkillFrame f) => _resolve().ContinueShiHcPrompt(f);
    internal bool DrainShiHc(long id) => _resolve().DrainShiHc(id);
    internal Card? FindShiHcSlashToGain() => _resolve().FindShiHcSlashToGain();
    internal void FinishShiHcOperation(ProgramSkillFrame f) => _resolve().FinishShiHcOperation(f);
    internal string GetShiHcSkillName(string skillId) => _resolve().GetShiHcSkillName(skillId);
    internal bool IsShiHcSiliSlashUnrespondable(long frameId) => _resolve().IsShiHcSiliSlashUnrespondable(frameId);
    internal PromptChoice MakeShiHcBranchChoice(ProgramSkillFrame f, string branch, string label) => _resolve().MakeShiHcBranchChoice(f, branch, label);
    internal PromptChoice MakeShiHcSeatChoice(ProgramSkillFrame f, string prefix, int seat, string label) => _resolve().MakeShiHcSeatChoice(f, prefix, seat, label);
    internal void PublishShiHcChoices(ProgramSkillFrame f, int answeringSeat, IReadOnlyList<PromptChoice> choices, string prompt) => _resolve().PublishShiHcChoices(f, answeringSeat, choices, prompt);
    internal SkillProgramStepOutcome PublishShiHcSiliBranch(ProgramSkillFrame f) => _resolve().PublishShiHcSiliBranch(f);
    internal bool ResumeShiHcCujun(long id) => _resolve().ResumeShiHcCujun(id);
    internal bool ResumeShiHcSili(long id) => _resolve().ResumeShiHcSili(id);
    internal bool ReturnShiHcMovement(ProgramSkillFrame f) => _resolve().ReturnShiHcMovement(f);
    internal SkillProgramStepOutcome SettleShiHcBeishui(ProgramSkillFrame f) => _resolve().SettleShiHcBeishui(f);
    internal void SettleShiHcClaim(ProgramSkillFrame f) => _resolve().SettleShiHcClaim(f);
    internal void SettleShiHcCujun(ProgramSkillFrame f) => _resolve().SettleShiHcCujun(f);
    internal SkillProgramStepOutcome SettleShiHcSiliDraw(ProgramSkillFrame f) => _resolve().SettleShiHcSiliDraw(f);
    internal IReadOnlyList<PromptChoice> ShiHcChooserChoices(ProgramSkillFrame f, int chooser) => _resolve().ShiHcChooserChoices(f, chooser);
    internal int ShiHcCujunSlashBonus(int seat) => _resolve().ShiHcCujunSlashBonus(seat);
    internal IReadOnlyList<PromptChoice> ShiHcCurrentChoices(ProgramSkillFrame f) => _resolve().ShiHcCurrentChoices(f);
    internal int ShiHcHandCount(int seat) => _resolve().ShiHcHandCount(seat);
    internal NativeOwners.ShiHcLedgerView ShiHcLedger() => _resolve().ShiHcLedger();
    internal IEnumerable<int> ShiHcRangeSeats(int seat) => _resolve().ShiHcRangeSeats(seat);
    internal IReadOnlyList<PromptChoice> ShiHcSiliBranchChoices(ProgramSkillFrame f, ShiHcReceipt r) => _resolve().ShiHcSiliBranchChoices(f, r);
    internal bool HasShiHcCapability { get => _resolve().HasShiHcCapability; }
}
