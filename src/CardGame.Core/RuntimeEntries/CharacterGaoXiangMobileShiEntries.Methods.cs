#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGaoXiangMobileShiEntries
{
    internal void AnswerShiZx(PromptChoice selected) => _resolve().AnswerShiZx(selected);
    internal void AnswerShiZxSeat(ProgramSkillFrame f, PromptChoice selected) => _resolve().AnswerShiZxSeat(f, selected);
    internal SkillProgramStepOutcome BeginShiZxChiyuan(ProgramSkillFrame input) => _resolve().BeginShiZxChiyuan(input);
    internal SkillProgramStepOutcome BeginShiZxErdi(ProgramSkillFrame input) => _resolve().BeginShiZxErdi(input);
    internal bool CanOfferShiZxChiyuanActivation(CharacterState owner, string skill) => _resolve().CanOfferShiZxChiyuanActivation(owner, skill);
    internal PromptChoice ChooseShiZxChiyuanAi(PendingDecision decision, ProgramSkillFrame f) => _resolve().ChooseShiZxChiyuanAi(decision, f);
    internal PromptChoice ChooseShiZxErdiAi(PendingDecision decision, ProgramSkillFrame f) => _resolve().ChooseShiZxErdiAi(decision, f);
    internal void ContinueShiZxPrompt(ProgramSkillFrame f) => _resolve().ContinueShiZxPrompt(f);
    internal bool DrainShiZx(ProgramSkillFrame f) => _resolve().DrainShiZx(f);
    internal void FinishShiZxOperation(ProgramSkillFrame f) => _resolve().FinishShiZxOperation(f);
    internal void IssueShiZxSlash(ProgramSkillFrame f, int targetSeat, bool revenge) => _resolve().IssueShiZxSlash(f, targetSeat, revenge);
    internal PromptChoice MakeShiZxChoice(ProgramSkillFrame f, string branch, string label) => _resolve().MakeShiZxChoice(f, branch, label);
    internal PromptChoice MakeShiZxDiscardChoice(ProgramSkillFrame f, string branch, int cardId, IReadOnlyList<int> selected) => _resolve().MakeShiZxDiscardChoice(f, branch, cardId, selected);
    internal PromptChoice MakeShiZxSeatChoice(ProgramSkillFrame f, int seat) => _resolve().MakeShiZxSeatChoice(f, seat);
    internal void PublishShiZx(ProgramSkillFrame f, ShiZxStage stage, (int Seat, int? Card)[] options, IReadOnlyList<int> selected) => _resolve().PublishShiZx(f, stage, options, selected);
    internal void PublishShiZxChoices(ProgramSkillFrame f, ShiZxStage stage, (int Seat, int? Card)[] options) => _resolve().PublishShiZxChoices(f, stage, options);
    internal void PublishShiZxDiscard(ProgramSkillFrame f, ShiZxStage stage) => _resolve().PublishShiZxDiscard(f, stage);
    internal bool ResumeShiZx(long id) => _resolve().ResumeShiZx(id);
    internal bool ReturnShiZxMovement(ProgramSkillFrame f) => _resolve().ReturnShiZxMovement(f);
    internal void SettleShiZxChiyuanSlash(ProgramSkillFrame f) => _resolve().SettleShiZxChiyuanSlash(f);
    internal void SettleShiZxErdi(ProgramSkillFrame f, int ownerCount, int theirCount) => _resolve().SettleShiZxErdi(f, ownerCount, theirCount);
    internal IReadOnlyList<PromptChoice> ShiZxCurrentChoices(ProgramSkillFrame f) => _resolve().ShiZxCurrentChoices(f);
    internal IReadOnlyList<PromptChoice> ShiZxDiscardChoices(ProgramSkillFrame f, ShiZxReceipt r, int seat) => _resolve().ShiZxDiscardChoices(f, r, seat);
    internal IEnumerable<int> ShiZxErdiTargetSeats(int ownerSeat) => _resolve().ShiZxErdiTargetSeats(ownerSeat);
    internal HashSet<int> ShiZxRangeCoverage(int seat) => _resolve().ShiZxRangeCoverage(seat);
    internal IEnumerable<int> ShiZxReceiverSeats(int ownerSeat) => _resolve().ShiZxReceiverSeats(ownerSeat);
    internal void ShiZxRecover(ProgramSkillFrame f, int seat, int amount) => _resolve().ShiZxRecover(f, seat, amount);
    internal IEnumerable<int> ShiZxSlashTargetSeats(int ownerSeat, int receiverSeat) => _resolve().ShiZxSlashTargetSeats(ownerSeat, receiverSeat);
}
