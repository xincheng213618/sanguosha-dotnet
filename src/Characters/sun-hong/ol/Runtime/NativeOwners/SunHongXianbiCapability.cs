#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class SunHongXianbiCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterSunHongOlEntries> getCharacterSunHongOlRuntime) : ISunHongXianbiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterSunHongOlRuntime().BeginXianbi(frame);
}
