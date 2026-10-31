#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYangHongMobileShiEntries
{
    internal void AdvanceShiYhRetaliation(ProgramSkillFrame f) => _resolve().AdvanceShiYhRetaliation(f);
    internal void AnswerShiYh(PromptChoice selected) => _resolve().AnswerShiYh(selected);
    internal void ApplyShiYhMove(ProgramSkillFrame f, PromptChoice choice) => _resolve().ApplyShiYhMove(f, choice);
    internal SkillProgramStepOutcome BeginShiYhJijian(ProgramSkillFrame input) => _resolve().BeginShiYhJijian(input);
    internal void BeginShiYhRetaliation(ProgramSkillFrame f) => _resolve().BeginShiYhRetaliation(f);
    internal void BeginShiYhSlash(ProgramSkillFrame f, int winnerSeat, int loserSeat) => _resolve().BeginShiYhSlash(f, winnerSeat, loserSeat);
    internal SkillProgramStepOutcome BeginShiYhYuanmo(ProgramSkillFrame input) => _resolve().BeginShiYhYuanmo(input);
    internal bool CanOfferShiYhYuanmo(ProgramTriggerLegalityContext c) => _resolve().CanOfferShiYhYuanmo(c);
    internal PromptChoice ChooseShiYhJijianAi(PendingDecision decision, ProgramSkillFrame f) => _resolve().ChooseShiYhJijianAi(decision, f);
    internal PromptChoice ChooseShiYhYuanmoAi(PendingDecision decision, ProgramSkillFrame f) => _resolve().ChooseShiYhYuanmoAi(decision, f);
    internal void ContinueShiYh(long id) => _resolve().ContinueShiYh(id);
    internal void ContinueShiYhContest(ProgramSkillFrame f) => _resolve().ContinueShiYhContest(f);
    internal bool DrainShiYh(ProgramSkillFrame f) => _resolve().DrainShiYh(f);
    internal void FinishShiYhOperation(ProgramSkillFrame f) => _resolve().FinishShiYhOperation(f);
    internal void LaunchShiYhContest(ProgramSkillFrame f) => _resolve().LaunchShiYhContest(f);
    internal PromptChoice MakeShiYhChoice(ProgramSkillFrame f, string branch, string label, IReadOnlyList<int>? cards = default, IReadOnlyList<int>? targets = default) => _resolve().MakeShiYhChoice(f, branch, label, cards, targets);
    internal void PublishShiYh(ProgramSkillFrame f) => _resolve().PublishShiYh(f);
    internal bool ResumeShiYh(long id) => _resolve().ResumeShiYh(id);
    internal bool ReturnShiYhMovement(ProgramSkillFrame f) => _resolve().ReturnShiYhMovement(f);
    internal IReadOnlyList<PromptChoice> ShiYhChoices(ProgramSkillFrame f) => _resolve().ShiYhChoices(f);
    internal IEnumerable<PromptChoice> ShiYhFieldMoveChoices(ProgramSkillFrame? f) => _resolve().ShiYhFieldMoveChoices(f);
    internal bool ShiYhFieldMovesExist() => _resolve().ShiYhFieldMovesExist();
    internal HashSet<int> ShiYhRangeCoverage(int seat) => _resolve().ShiYhRangeCoverage(seat);
    internal bool HasShiYhCapability { get => _resolve().HasShiYhCapability; }
}
