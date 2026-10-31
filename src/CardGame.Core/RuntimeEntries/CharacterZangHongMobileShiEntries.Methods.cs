#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZangHongMobileShiEntries
{
    internal void AnswerShiZh(PromptChoice selected) => _resolve().AnswerShiZh(selected);
    internal SkillProgramStepOutcome BeginShiZhJuegu(ProgramSkillFrame input) => _resolve().BeginShiZhJuegu(input);
    internal SkillProgramStepOutcome BeginShiZhLiezhi(ProgramSkillFrame input) => _resolve().BeginShiZhLiezhi(input);
    internal bool CanOfferShiZhLiezhiActivation(CharacterState owner, string skill) => _resolve().CanOfferShiZhLiezhiActivation(owner, skill);
    internal PromptChoice ChooseShiZhJueguAi(PendingDecision decision, ProgramSkillFrame f) => _resolve().ChooseShiZhJueguAi(decision, f);
    internal PromptChoice ChooseShiZhLiezhiAi(PendingDecision decision, ProgramSkillFrame f) => _resolve().ChooseShiZhLiezhiAi(decision, f);
    internal void ContinueShiZh(ProgramSkillFrame f) => _resolve().ContinueShiZh(f);
    internal bool DrainShiZh(ProgramSkillFrame f) => _resolve().DrainShiZh(f);
    internal void FinishShiZhOperation(ProgramSkillFrame f) => _resolve().FinishShiZhOperation(f);
    internal void PublishShiZhLiezhi(ProgramSkillFrame f, (CardKind Kind, int Target)[] options) => _resolve().PublishShiZhLiezhi(f, options);
    internal bool ResumeShiZh(long id) => _resolve().ResumeShiZh(id);
    internal bool ReturnShiZhMovement(ProgramSkillFrame f) => _resolve().ReturnShiZhMovement(f);
    internal IReadOnlyList<PromptChoice> ShiZhDiscardChoices(ProgramSkillFrame f) => _resolve().ShiZhDiscardChoices(f);
    internal IEnumerable<(CardKind Kind, int Target)> ShiZhLiezhiOptions(int owner) => _resolve().ShiZhLiezhiOptions(owner);
}
