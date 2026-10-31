#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ZhuLingProgramCapability(Func<RuntimeEntries.CharacterZhuLingOlEntries> getCharacterZhuLingOlRuntime) : IZhuLingProgramHost
{
    public SkillProgramStepOutcome ZhuLingZhanyiChooseCategory(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterZhuLingOlRuntime().ZhanyiProgramChooseCategory(frame, effect);
    public SkillProgramStepOutcome ZhuLingZhanyiEquipmentPunish(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterZhuLingOlRuntime().ZhanyiProgramEquipmentPunish(frame, effect);
}
