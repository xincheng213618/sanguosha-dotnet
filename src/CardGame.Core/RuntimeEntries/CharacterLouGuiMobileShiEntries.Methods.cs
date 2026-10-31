#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLouGuiMobileShiEntries
{
    internal void AnswerShiLg(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerShiLg(selected);
    internal void ApplyShiLgGuanSha(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ApplyShiLgGuanSha(f);
    internal void ApplyShiLgJiYuCost(global::CardGame.Core.ProgramSkillFrame f, int cardId) => _resolve().ApplyShiLgJiYuCost(f, cardId);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShiLgGuanSha(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginShiLgGuanSha(input);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShiLgJiYu(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginShiLgJiYu(input);
    internal bool CanStartShiLgJiYu(global::CardGame.Core.CharacterState owner, string skill) => _resolve().CanStartShiLgJiYu(owner, skill);
    internal global::CardGame.Core.PromptChoice ChooseShiLgGuanShaAi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseShiLgGuanShaAi(decision, f);
    internal global::CardGame.Core.PromptChoice ChooseShiLgJiYuAi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseShiLgJiYuAi(decision, f);
    internal bool DrainShiLg(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainShiLg(f);
    internal void FinishShiLgOperation(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishShiLgOperation(f);
    internal global::CardGame.Core.PromptChoice MakeShiLgChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, string label) => _resolve().MakeShiLgChoice(f, branch, label);
    internal void PublishShiLg(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishShiLg(f);
    internal bool ResumeShiLg(long id) => _resolve().ResumeShiLg(id);
    internal bool ReturnShiLgMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnShiLgMovement(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShiLgChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShiLgChoices(f);
    internal int ShiLgGuanshaHandLimitBonus(int seat) => _resolve().ShiLgGuanshaHandLimitBonus(seat);
    internal bool ShiLgJiYuGainsAllUsed(int seat) => _resolve().ShiLgJiYuGainsAllUsed(seat);
    internal int ShiLgJiYuUsesThisPhase(int seat) => _resolve().ShiLgJiYuUsesThisPhase(seat);
    internal bool HasShiLgCapability { get => _resolve().HasShiLgCapability; }
}
