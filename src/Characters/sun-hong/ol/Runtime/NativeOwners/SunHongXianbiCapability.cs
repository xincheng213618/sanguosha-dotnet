#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class SunHongXianbiCapability(Func<RuntimeEntries.CharacterSunHongOlEntries> getCharacterSunHongOlRuntime) : ISunHongXianbiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterSunHongOlRuntime().BeginXianbi(frame);
}
