#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWeiZiOlEntries
{
    internal SkillProgramStepOutcome LiejieProgramSourceDiscard(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().LiejieProgramSourceDiscard(frame, effect);
    internal void ResolveWeiZiProgramChoice(PromptChoice selected) => _resolve().ResolveWeiZiProgramChoice(selected);
    internal void ResolveYuanziDrawChoice(PromptChoice selected) => _resolve().ResolveYuanziDrawChoice(selected);
    internal PromptChoice SelectAiLiejieSourceDiscardChoice(PendingDecision decision) => _resolve().SelectAiLiejieSourceDiscardChoice(decision);
    internal SkillProgramStepOutcome YuanziProgramDamageDraw(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().YuanziProgramDamageDraw(frame, effect);
    internal SkillProgramStepOutcome YuanziProgramResolve(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().YuanziProgramResolve(frame, effect);
}
