#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYangHongMobileShiEntries
{
    internal void AdvanceShiYhRetaliation(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AdvanceShiYhRetaliation(f);
    internal void AnswerShiYh(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerShiYh(selected);
    internal void ApplyShiYhMove(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PromptChoice choice) => _resolve().ApplyShiYhMove(f, choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShiYhJijian(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginShiYhJijian(input);
    internal void BeginShiYhRetaliation(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginShiYhRetaliation(f);
    internal void BeginShiYhSlash(global::CardGame.Core.ProgramSkillFrame f, int winnerSeat, int loserSeat) => _resolve().BeginShiYhSlash(f, winnerSeat, loserSeat);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShiYhYuanmo(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginShiYhYuanmo(input);
    internal bool CanOfferShiYhYuanmo(global::CardGame.Core.ProgramTriggerLegalityContext c) => _resolve().CanOfferShiYhYuanmo(c);
    internal global::CardGame.Core.PromptChoice ChooseShiYhJijianAi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseShiYhJijianAi(decision, f);
    internal global::CardGame.Core.PromptChoice ChooseShiYhYuanmoAi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseShiYhYuanmoAi(decision, f);
    internal void ContinueShiYh(long id) => _resolve().ContinueShiYh(id);
    internal void ContinueShiYhContest(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueShiYhContest(f);
    internal bool DrainShiYh(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainShiYh(f);
    internal void FinishShiYhOperation(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishShiYhOperation(f);
    internal void LaunchShiYhContest(global::CardGame.Core.ProgramSkillFrame f) => _resolve().LaunchShiYhContest(f);
    internal global::CardGame.Core.PromptChoice MakeShiYhChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, string label, global::System.Collections.Generic.IReadOnlyList<int>? cards = default, global::System.Collections.Generic.IReadOnlyList<int>? targets = default) => _resolve().MakeShiYhChoice(f, branch, label, cards, targets);
    internal void PublishShiYh(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishShiYh(f);
    internal bool ResumeShiYh(long id) => _resolve().ResumeShiYh(id);
    internal bool ReturnShiYhMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnShiYhMovement(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShiYhChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShiYhChoices(f);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.PromptChoice> ShiYhFieldMoveChoices(global::CardGame.Core.ProgramSkillFrame? f) => _resolve().ShiYhFieldMoveChoices(f);
    internal bool ShiYhFieldMovesExist() => _resolve().ShiYhFieldMovesExist();
    internal global::System.Collections.Generic.HashSet<int> ShiYhRangeCoverage(int seat) => _resolve().ShiYhRangeCoverage(seat);
    internal bool HasShiYhCapability { get => _resolve().HasShiYhCapability; }
}
