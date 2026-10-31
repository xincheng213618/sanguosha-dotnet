#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ZhuLingProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhuLingOlRuntime>> getCharacterZhuLingOlRuntime) : IZhuLingProgramHost
{
    public SkillProgramStepOutcome ZhuLingZhanyiChooseCategory(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterZhuLingOlRuntime().Value.ZhanyiProgramChooseCategory(frame, effect);
    public SkillProgramStepOutcome ZhuLingZhanyiEquipmentPunish(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterZhuLingOlRuntime().Value.ZhanyiProgramEquipmentPunish(frame, effect);
}
