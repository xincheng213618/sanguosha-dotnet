#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWeiZiOlRuntime
{
    // Prompt router: every Wei Zi prompt rides one op, so the shared program
    // dispatch hands each accepted choice back through this single entry.
    [ProgramAnswer("yuanzi-draw", "yuanzi-decline", "liejie-pick", "liejie-finish", "liejie-decline")]
    internal void ResolveWeiZiProgramChoice(PromptChoice selected)
    {
        switch (selected.Parameters.GetValueOrDefault("program-action"))
        {
            case "yuanzi-draw" or "yuanzi-decline":
                ResolveYuanziDrawChoice(selected);
                return;
            case "liejie-pick" or "liejie-finish" or "liejie-decline":
                _coreLiejieSourceDiscardsOperations.ResolveLiejieDiscardChoice(selected);
                return;
            default:
                throw new InvalidOperationException("The Wei Zi choice lost its action parameter.");
        }
    }

    SkillProgramStepOutcome IWeiZiProgramHost.YuanziResolve(ProgramSkillFrame frame, SkillProgramEffect effect) => YuanziProgramResolve(frame, effect);
    SkillProgramStepOutcome IWeiZiProgramHost.YuanziDamageDraw(ProgramSkillFrame frame, SkillProgramEffect effect) => YuanziProgramDamageDraw(frame, effect);
    SkillProgramStepOutcome IWeiZiProgramHost.LiejieSourceDiscard(ProgramSkillFrame frame, SkillProgramEffect effect) => LiejieProgramSourceDiscard(frame, effect);
}
