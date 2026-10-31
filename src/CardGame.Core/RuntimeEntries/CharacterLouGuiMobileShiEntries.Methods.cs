#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLouGuiMobileShiEntries
{
    internal void AnswerShiLg(PromptChoice selected) => _resolve().AnswerShiLg(selected);
    internal void ApplyShiLgGuanSha(ProgramSkillFrame f) => _resolve().ApplyShiLgGuanSha(f);
    internal void ApplyShiLgJiYuCost(ProgramSkillFrame f, int cardId) => _resolve().ApplyShiLgJiYuCost(f, cardId);
    internal SkillProgramStepOutcome BeginShiLgGuanSha(ProgramSkillFrame input) => _resolve().BeginShiLgGuanSha(input);
    internal SkillProgramStepOutcome BeginShiLgJiYu(ProgramSkillFrame input) => _resolve().BeginShiLgJiYu(input);
    internal bool CanStartShiLgJiYu(CharacterState owner, string skill) => _resolve().CanStartShiLgJiYu(owner, skill);
    internal PromptChoice ChooseShiLgGuanShaAi(PendingDecision decision, ProgramSkillFrame f) => _resolve().ChooseShiLgGuanShaAi(decision, f);
    internal PromptChoice ChooseShiLgJiYuAi(PendingDecision decision, ProgramSkillFrame f) => _resolve().ChooseShiLgJiYuAi(decision, f);
    internal bool DrainShiLg(ProgramSkillFrame f) => _resolve().DrainShiLg(f);
    internal void FinishShiLgOperation(ProgramSkillFrame f) => _resolve().FinishShiLgOperation(f);
    internal PromptChoice MakeShiLgChoice(ProgramSkillFrame f, string branch, string label) => _resolve().MakeShiLgChoice(f, branch, label);
    internal void PublishShiLg(ProgramSkillFrame f) => _resolve().PublishShiLg(f);
    internal bool ResumeShiLg(long id) => _resolve().ResumeShiLg(id);
    internal bool ReturnShiLgMovement(ProgramSkillFrame f) => _resolve().ReturnShiLgMovement(f);
    internal IReadOnlyList<PromptChoice> ShiLgChoices(ProgramSkillFrame f) => _resolve().ShiLgChoices(f);
    internal int ShiLgGuanshaHandLimitBonus(int seat) => _resolve().ShiLgGuanshaHandLimitBonus(seat);
    internal bool ShiLgJiYuGainsAllUsed(int seat) => _resolve().ShiLgJiYuGainsAllUsed(seat);
    internal int ShiLgJiYuUsesThisPhase(int seat) => _resolve().ShiLgJiYuUsesThisPhase(seat);
    internal bool HasShiLgCapability { get => _resolve().HasShiLgCapability; }
}
