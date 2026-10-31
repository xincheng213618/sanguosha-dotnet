#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWeiZiOlRuntime
{
    internal SkillProgramStepOutcome LiejieProgramSourceDiscard(ProgramSkillFrame frame, SkillProgramEffect effect) => _coreLiejieSourceDiscardsOperations.BeginLiejieSourceDiscard(frame, effect);
    [ProgramAiChoice(SkillProgramEffectOp.LiejieSourceDiscard)]
    internal PromptChoice SelectAiLiejieSourceDiscardChoice(PendingDecision decision) => _coreLiejieSourceDiscardsOperations.SelectAiLiejieDiscardChoice(decision);
}
