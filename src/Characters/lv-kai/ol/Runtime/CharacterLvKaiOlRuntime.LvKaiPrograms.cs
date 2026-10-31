#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLvKaiOlRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.ILvKaiProgramHost.TunanUseRevealedCard(ProgramSkillFrame frame, SkillProgramEffect effect) => TunanProgramUseRevealedCard(frame, effect);
    SkillProgramStepOutcome global::CardGame.Core.ILvKaiProgramHost.BijingMarkHandCards(ProgramSkillFrame frame, SkillProgramEffect effect) => BijingProgramMarkHandCards(frame, effect);
    SkillProgramStepOutcome global::CardGame.Core.ILvKaiProgramHost.BijingRecastMarkedCards(ProgramSkillFrame frame, SkillProgramEffect effect) => BijingProgramRecastMarkedCards(frame, effect);
    SkillProgramStepOutcome global::CardGame.Core.ILvKaiProgramHost.BijingPunishDiscardPhase(ProgramSkillFrame frame, SkillProgramEffect effect) => BijingProgramPunishDiscardPhase(frame, effect);
}
