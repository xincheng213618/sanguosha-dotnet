#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWeiZiOlRuntime
{
    internal SkillProgramStepOutcome LiejieProgramSourceDiscard(ProgramSkillFrame frame, SkillProgramEffect effect) => _coreLiejieSourceDiscardsOperations.Value.BeginLiejieSourceDiscard(frame, effect);
    [ProgramAiChoice(SkillProgramEffectOp.LiejieSourceDiscard)]
    internal PromptChoice SelectAiLiejieSourceDiscardChoice(PendingDecision decision) => _coreLiejieSourceDiscardsOperations.Value.SelectAiLiejieDiscardChoice(decision);
}
