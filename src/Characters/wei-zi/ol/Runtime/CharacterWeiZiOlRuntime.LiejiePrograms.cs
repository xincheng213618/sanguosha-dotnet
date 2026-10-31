#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWeiZiOlRuntime
{
    internal SkillProgramStepOutcome LiejieProgramSourceDiscard(ProgramSkillFrame frame, SkillProgramEffect effect) => _coreLiejieSourceDiscardsOperations.BeginLiejieSourceDiscard(frame, effect);
    [ProgramAiChoice(SkillProgramEffectOp.LiejieSourceDiscard)]
    internal PromptChoice SelectAiLiejieSourceDiscardChoice(PendingDecision decision) => _coreLiejieSourceDiscardsOperations.SelectAiLiejieDiscardChoice(decision);
}
