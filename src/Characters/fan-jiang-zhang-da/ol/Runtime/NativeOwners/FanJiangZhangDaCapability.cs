#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class FanJiangZhangDaCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterFanJiangZhangDaOlEntries> getCharacterFanJiangZhangDaOlRuntime) : IFanJiangZhangDaHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterFanJiangZhangDaOlRuntime().BeginJuesheng(f);
}
