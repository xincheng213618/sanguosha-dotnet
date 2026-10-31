#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterTangJiOlRuntime
{
    internal const int ProgramKanggeTurnDrawCap = 3;
    internal static string SuitName(Suit suit) => RuntimeSkills.RuntimeRules_tang_ji_ol_TangJiPrograms.SuitName(suit);
    SkillProgramStepOutcome ITangJiProgramHost.KanggeChooseTarget(ProgramSkillFrame frame, SkillProgramEffect effect) => KanggeProgramChooseTarget(frame, effect);
    SkillProgramStepOutcome ITangJiProgramHost.KanggeGainDraw(ProgramSkillFrame frame, SkillProgramEffect effect) => KanggeProgramGainDraw(frame, effect);
    SkillProgramStepOutcome ITangJiProgramHost.KanggeHealVictim(ProgramSkillFrame frame, SkillProgramEffect effect) => KanggeProgramHealVictim(frame, effect);
    SkillProgramStepOutcome ITangJiProgramHost.KanggeDeathPrice(ProgramSkillFrame frame, SkillProgramEffect effect) => KanggeProgramDeathPrice(frame, effect);
    SkillProgramStepOutcome ITangJiProgramHost.JieliePreventAndGift(ProgramSkillFrame frame, SkillProgramEffect effect) => JielieProgramPreventAndGift(frame, effect);
}
