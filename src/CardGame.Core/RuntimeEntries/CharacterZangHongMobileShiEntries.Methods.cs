#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZangHongMobileShiEntries
{
    internal void AnswerShiZh(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerShiZh(selected);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShiZhJuegu(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginShiZhJuegu(input);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShiZhLiezhi(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginShiZhLiezhi(input);
    internal bool CanOfferShiZhLiezhiActivation(global::CardGame.Core.CharacterState owner, string skill) => _resolve().CanOfferShiZhLiezhiActivation(owner, skill);
    internal global::CardGame.Core.PromptChoice ChooseShiZhJueguAi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseShiZhJueguAi(decision, f);
    internal global::CardGame.Core.PromptChoice ChooseShiZhLiezhiAi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseShiZhLiezhiAi(decision, f);
    internal void ContinueShiZh(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueShiZh(f);
    internal bool DrainShiZh(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainShiZh(f);
    internal void FinishShiZhOperation(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishShiZhOperation(f);
    internal void PublishShiZhLiezhi(global::CardGame.Core.ProgramSkillFrame f, (global::CardGame.Core.CardKind Kind, int Target)[] options) => _resolve().PublishShiZhLiezhi(f, options);
    internal bool ResumeShiZh(long id) => _resolve().ResumeShiZh(id);
    internal bool ReturnShiZhMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnShiZhMovement(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShiZhDiscardChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShiZhDiscardChoices(f);
    internal global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardKind Kind, int Target)> ShiZhLiezhiOptions(int owner) => _resolve().ShiZhLiezhiOptions(owner);
}
