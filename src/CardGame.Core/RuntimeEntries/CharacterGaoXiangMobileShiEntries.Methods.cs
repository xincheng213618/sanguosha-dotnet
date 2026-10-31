#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGaoXiangMobileShiEntries
{
    internal void AnswerShiZx(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerShiZx(selected);
    internal void AnswerShiZxSeat(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PromptChoice selected) => _resolve().AnswerShiZxSeat(f, selected);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShiZxChiyuan(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginShiZxChiyuan(input);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShiZxErdi(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginShiZxErdi(input);
    internal bool CanOfferShiZxChiyuanActivation(global::CardGame.Core.CharacterState owner, string skill) => _resolve().CanOfferShiZxChiyuanActivation(owner, skill);
    internal global::CardGame.Core.PromptChoice ChooseShiZxChiyuanAi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseShiZxChiyuanAi(decision, f);
    internal global::CardGame.Core.PromptChoice ChooseShiZxErdiAi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseShiZxErdiAi(decision, f);
    internal void ContinueShiZxPrompt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueShiZxPrompt(f);
    internal bool DrainShiZx(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainShiZx(f);
    internal void FinishShiZxOperation(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishShiZxOperation(f);
    internal void IssueShiZxSlash(global::CardGame.Core.ProgramSkillFrame f, int targetSeat, bool revenge) => _resolve().IssueShiZxSlash(f, targetSeat, revenge);
    internal global::CardGame.Core.PromptChoice MakeShiZxChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, string label) => _resolve().MakeShiZxChoice(f, branch, label);
    internal global::CardGame.Core.PromptChoice MakeShiZxDiscardChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, int cardId, global::System.Collections.Generic.IReadOnlyList<int> selected) => _resolve().MakeShiZxDiscardChoice(f, branch, cardId, selected);
    internal global::CardGame.Core.PromptChoice MakeShiZxSeatChoice(global::CardGame.Core.ProgramSkillFrame f, int seat) => _resolve().MakeShiZxSeatChoice(f, seat);
    internal void PublishShiZx(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ShiZxStage stage, (int Seat, int? Card)[] options, global::System.Collections.Generic.IReadOnlyList<int> selected) => _resolve().PublishShiZx(f, stage, options, selected);
    internal void PublishShiZxChoices(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ShiZxStage stage, (int Seat, int? Card)[] options) => _resolve().PublishShiZxChoices(f, stage, options);
    internal void PublishShiZxDiscard(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ShiZxStage stage) => _resolve().PublishShiZxDiscard(f, stage);
    internal bool ResumeShiZx(long id) => _resolve().ResumeShiZx(id);
    internal bool ReturnShiZxMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnShiZxMovement(f);
    internal void SettleShiZxChiyuanSlash(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SettleShiZxChiyuanSlash(f);
    internal void SettleShiZxErdi(global::CardGame.Core.ProgramSkillFrame f, int ownerCount, int theirCount) => _resolve().SettleShiZxErdi(f, ownerCount, theirCount);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShiZxCurrentChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShiZxCurrentChoices(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShiZxDiscardChoices(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ShiZxReceipt r, int seat) => _resolve().ShiZxDiscardChoices(f, r, seat);
    internal global::System.Collections.Generic.IEnumerable<int> ShiZxErdiTargetSeats(int ownerSeat) => _resolve().ShiZxErdiTargetSeats(ownerSeat);
    internal global::System.Collections.Generic.HashSet<int> ShiZxRangeCoverage(int seat) => _resolve().ShiZxRangeCoverage(seat);
    internal global::System.Collections.Generic.IEnumerable<int> ShiZxReceiverSeats(int ownerSeat) => _resolve().ShiZxReceiverSeats(ownerSeat);
    internal void ShiZxRecover(global::CardGame.Core.ProgramSkillFrame f, int seat, int amount) => _resolve().ShiZxRecover(f, seat, amount);
    internal global::System.Collections.Generic.IEnumerable<int> ShiZxSlashTargetSeats(int ownerSeat, int receiverSeat) => _resolve().ShiZxSlashTargetSeats(ownerSeat, receiverSeat);
}
