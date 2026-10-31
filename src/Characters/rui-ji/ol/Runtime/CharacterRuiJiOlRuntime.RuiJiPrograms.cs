#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterRuiJiOlRuntime
{
    internal const string QiaoliWeaponBinding = "qiaoli-weapon";
    internal const string QiaoliArmorBinding = "qiaoli-armor";
    internal const string QingliangRevealBinding = "qingliang-reveal";
    SkillProgramStepOutcome IRuiJiProgramHost.QiaoliWeaponDuel(ProgramSkillFrame frame, SkillProgramEffect effect) => QiaoliProgramDuelLaunch(frame, unrespondable: false);
    SkillProgramStepOutcome IRuiJiProgramHost.QiaoliArmorDuel(ProgramSkillFrame frame, SkillProgramEffect effect) => QiaoliProgramDuelLaunch(frame, unrespondable: true);
    SkillProgramStepOutcome IRuiJiProgramHost.QiaoliWeaponDamageDraw(ProgramSkillFrame frame, SkillProgramEffect effect) => QiaoliProgramWeaponDamageDraw(frame, effect);
    SkillProgramStepOutcome IRuiJiProgramHost.QiaoliEndingEquipmentGain(ProgramSkillFrame frame, SkillProgramEffect effect) => QiaoliProgramEndingEquipmentGain(frame, effect);
    SkillProgramStepOutcome IRuiJiProgramHost.QingliangChooseOption(ProgramSkillFrame frame, SkillProgramEffect effect) => QingliangProgramChooseOption(frame, effect);
}
