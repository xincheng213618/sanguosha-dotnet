#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterRuiJiOlRuntime
{
    internal const string QiaoliWeaponBinding = "qiaoli-weapon";
    internal const string QiaoliArmorBinding = "qiaoli-armor";
    internal const string QingliangRevealBinding = "qingliang-reveal";
    SkillProgramStepOutcome global::CardGame.Core.IRuiJiProgramHost.QiaoliWeaponDuel(ProgramSkillFrame frame, SkillProgramEffect effect) => QiaoliProgramDuelLaunch(frame, unrespondable: false);
    SkillProgramStepOutcome global::CardGame.Core.IRuiJiProgramHost.QiaoliArmorDuel(ProgramSkillFrame frame, SkillProgramEffect effect) => QiaoliProgramDuelLaunch(frame, unrespondable: true);
    SkillProgramStepOutcome global::CardGame.Core.IRuiJiProgramHost.QiaoliWeaponDamageDraw(ProgramSkillFrame frame, SkillProgramEffect effect) => QiaoliProgramWeaponDamageDraw(frame, effect);
    SkillProgramStepOutcome global::CardGame.Core.IRuiJiProgramHost.QiaoliEndingEquipmentGain(ProgramSkillFrame frame, SkillProgramEffect effect) => QiaoliProgramEndingEquipmentGain(frame, effect);
    SkillProgramStepOutcome global::CardGame.Core.IRuiJiProgramHost.QingliangChooseOption(ProgramSkillFrame frame, SkillProgramEffect effect) => QingliangProgramChooseOption(frame, effect);
}
