#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWeiZiOlEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome LiejieProgramSourceDiscard(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().LiejieProgramSourceDiscard(frame, effect);
    internal void ResolveWeiZiProgramChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveWeiZiProgramChoice(selected);
    internal void ResolveYuanziDrawChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveYuanziDrawChoice(selected);
    internal global::CardGame.Core.PromptChoice SelectAiLiejieSourceDiscardChoice(global::CardGame.Core.PendingDecision decision) => _resolve().SelectAiLiejieSourceDiscardChoice(decision);
    internal global::CardGame.Core.SkillProgramStepOutcome YuanziProgramDamageDraw(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().YuanziProgramDamageDraw(frame, effect);
    internal global::CardGame.Core.SkillProgramStepOutcome YuanziProgramResolve(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().YuanziProgramResolve(frame, effect);
}
