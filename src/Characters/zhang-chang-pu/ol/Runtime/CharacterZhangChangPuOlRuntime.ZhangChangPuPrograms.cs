#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangChangPuOlRuntime
{
    void global::CardGame.Core.IZhangChangPuProgramHost.YanjiaoRevealTopCards(ProgramSkillFrame frame, SkillProgramEffect effect) => YanjiaoRevealProgramTopCards(frame, effect);
    SkillProgramStepOutcome global::CardGame.Core.IZhangChangPuProgramHost.YanjiaoSplitRevealedCards(ProgramSkillFrame frame, SkillProgramEffect effect) => YanjiaoSplitRevealedCards(frame, effect);
    void global::CardGame.Core.IZhangChangPuProgramHost.ShenShenDrawAndArmBonus(ProgramSkillFrame frame, SkillProgramEffect effect) => ShenShenDrawAndArmBonus(frame, effect);
}
