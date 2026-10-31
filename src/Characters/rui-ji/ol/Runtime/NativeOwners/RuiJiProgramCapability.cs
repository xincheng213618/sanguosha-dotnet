#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class RuiJiProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterRuiJiOlRuntime>> getCharacterRuiJiOlRuntime) : IRuiJiProgramHost
{
    public SkillProgramStepOutcome QiaoliWeaponDuel(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterRuiJiOlRuntime().Value.QiaoliProgramDuelLaunch(frame, unrespondable: false);
    public SkillProgramStepOutcome QiaoliArmorDuel(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterRuiJiOlRuntime().Value.QiaoliProgramDuelLaunch(frame, unrespondable: true);
    public SkillProgramStepOutcome QiaoliWeaponDamageDraw(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterRuiJiOlRuntime().Value.QiaoliProgramWeaponDamageDraw(frame, effect);
    public SkillProgramStepOutcome QiaoliEndingEquipmentGain(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterRuiJiOlRuntime().Value.QiaoliProgramEndingEquipmentGain(frame, effect);
    public SkillProgramStepOutcome QingliangChooseOption(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterRuiJiOlRuntime().Value.QingliangProgramChooseOption(frame, effect);
}
