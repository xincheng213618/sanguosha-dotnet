#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class RuiJiProgramCapability(Func<RuntimeEntries.CharacterRuiJiOlEntries> getCharacterRuiJiOlRuntime) : IRuiJiProgramHost
{
    public SkillProgramStepOutcome QiaoliWeaponDuel(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterRuiJiOlRuntime().QiaoliProgramDuelLaunch(frame, unrespondable: false);
    public SkillProgramStepOutcome QiaoliArmorDuel(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterRuiJiOlRuntime().QiaoliProgramDuelLaunch(frame, unrespondable: true);
    public SkillProgramStepOutcome QiaoliWeaponDamageDraw(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterRuiJiOlRuntime().QiaoliProgramWeaponDamageDraw(frame, effect);
    public SkillProgramStepOutcome QiaoliEndingEquipmentGain(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterRuiJiOlRuntime().QiaoliProgramEndingEquipmentGain(frame, effect);
    public SkillProgramStepOutcome QingliangChooseOption(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterRuiJiOlRuntime().QingliangProgramChooseOption(frame, effect);
}
