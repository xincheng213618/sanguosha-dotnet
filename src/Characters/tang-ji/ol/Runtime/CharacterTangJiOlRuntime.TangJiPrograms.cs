#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterTangJiOlRuntime
{
    internal const int ProgramKanggeTurnDrawCap = 3;
    internal static string SuitName(Suit suit) => global::CardGame.Core.RuntimeSkills.RuntimeRules_tang_ji_ol_TangJiPrograms.SuitName(suit);
    SkillProgramStepOutcome global::CardGame.Core.ITangJiProgramHost.KanggeChooseTarget(ProgramSkillFrame frame, SkillProgramEffect effect) => KanggeProgramChooseTarget(frame, effect);
    SkillProgramStepOutcome global::CardGame.Core.ITangJiProgramHost.KanggeGainDraw(ProgramSkillFrame frame, SkillProgramEffect effect) => KanggeProgramGainDraw(frame, effect);
    SkillProgramStepOutcome global::CardGame.Core.ITangJiProgramHost.KanggeHealVictim(ProgramSkillFrame frame, SkillProgramEffect effect) => KanggeProgramHealVictim(frame, effect);
    SkillProgramStepOutcome global::CardGame.Core.ITangJiProgramHost.KanggeDeathPrice(ProgramSkillFrame frame, SkillProgramEffect effect) => KanggeProgramDeathPrice(frame, effect);
    SkillProgramStepOutcome global::CardGame.Core.ITangJiProgramHost.JieliePreventAndGift(ProgramSkillFrame frame, SkillProgramEffect effect) => JielieProgramPreventAndGift(frame, effect);
}
