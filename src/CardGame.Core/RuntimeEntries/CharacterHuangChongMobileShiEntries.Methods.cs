#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuangChongMobileShiEntries
{
    internal void AnswerShiHc(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerShiHc(selected);
    internal void AnswerShiHcCujunTarget(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ShiHcReceipt r, string branch) => _resolve().AnswerShiHcCujunTarget(f, r, branch);
    internal void AnswerShiHcSiliBranch(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ShiHcReceipt r, string branch) => _resolve().AnswerShiHcSiliBranch(f, r, branch);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShiHcCujun(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginShiHcCujun(input);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShiHcSili(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginShiHcSili(input);
    internal bool CanOfferShiHcCujun(global::CardGame.Core.ProgramTriggerLegalityContext c) => _resolve().CanOfferShiHcCujun(c);
    internal bool CanOfferShiHcSili(global::CardGame.Core.ProgramTriggerLegalityContext c) => _resolve().CanOfferShiHcSili(c);
    internal global::CardGame.Core.PromptChoice ChooseShiHcCujunAi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseShiHcCujunAi(decision, f);
    internal global::CardGame.Core.PromptChoice ChooseShiHcSiliAi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseShiHcSiliAi(decision, f);
    internal void ContinueShiHcChoosers(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueShiHcChoosers(f);
    internal void ContinueShiHcPrompt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueShiHcPrompt(f);
    internal bool DrainShiHc(long id) => _resolve().DrainShiHc(id);
    internal global::CardGame.Core.Card? FindShiHcSlashToGain() => _resolve().FindShiHcSlashToGain();
    internal void FinishShiHcOperation(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishShiHcOperation(f);
    internal string GetShiHcSkillName(string skillId) => _resolve().GetShiHcSkillName(skillId);
    internal bool IsShiHcSiliSlashUnrespondable(long frameId) => _resolve().IsShiHcSiliSlashUnrespondable(frameId);
    internal global::CardGame.Core.PromptChoice MakeShiHcBranchChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, string label) => _resolve().MakeShiHcBranchChoice(f, branch, label);
    internal global::CardGame.Core.PromptChoice MakeShiHcSeatChoice(global::CardGame.Core.ProgramSkillFrame f, string prefix, int seat, string label) => _resolve().MakeShiHcSeatChoice(f, prefix, seat, label);
    internal void PublishShiHcChoices(global::CardGame.Core.ProgramSkillFrame f, int answeringSeat, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> choices, string prompt) => _resolve().PublishShiHcChoices(f, answeringSeat, choices, prompt);
    internal global::CardGame.Core.SkillProgramStepOutcome PublishShiHcSiliBranch(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishShiHcSiliBranch(f);
    internal bool ResumeShiHcCujun(long id) => _resolve().ResumeShiHcCujun(id);
    internal bool ResumeShiHcSili(long id) => _resolve().ResumeShiHcSili(id);
    internal bool ReturnShiHcMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnShiHcMovement(f);
    internal global::CardGame.Core.SkillProgramStepOutcome SettleShiHcBeishui(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SettleShiHcBeishui(f);
    internal void SettleShiHcClaim(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SettleShiHcClaim(f);
    internal void SettleShiHcCujun(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SettleShiHcCujun(f);
    internal global::CardGame.Core.SkillProgramStepOutcome SettleShiHcSiliDraw(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SettleShiHcSiliDraw(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShiHcChooserChoices(global::CardGame.Core.ProgramSkillFrame f, int chooser) => _resolve().ShiHcChooserChoices(f, chooser);
    internal int ShiHcCujunSlashBonus(int seat) => _resolve().ShiHcCujunSlashBonus(seat);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShiHcCurrentChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShiHcCurrentChoices(f);
    internal int ShiHcHandCount(int seat) => _resolve().ShiHcHandCount(seat);
    internal global::CardGame.Core.NativeOwners.ShiHcLedgerView ShiHcLedger() => _resolve().ShiHcLedger();
    internal global::System.Collections.Generic.IEnumerable<int> ShiHcRangeSeats(int seat) => _resolve().ShiHcRangeSeats(seat);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShiHcSiliBranchChoices(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ShiHcReceipt r) => _resolve().ShiHcSiliBranchChoices(f, r);
    internal bool HasShiHcCapability { get => _resolve().HasShiHcCapability; }
}
