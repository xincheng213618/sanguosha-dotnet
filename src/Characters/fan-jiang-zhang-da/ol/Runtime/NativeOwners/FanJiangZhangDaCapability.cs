#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class FanJiangZhangDaCapability(Func<RuntimeEntries.CharacterFanJiangZhangDaOlEntries> getCharacterFanJiangZhangDaOlRuntime) : IFanJiangZhangDaHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterFanJiangZhangDaOlRuntime().BeginJuesheng(f);
}
