#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ZhuJunCapability(Func<RuntimeEntries.CharacterZhuJunOlEntries> getCharacterZhuJunOlRuntime) : IZhuJunHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterZhuJunOlRuntime().BeginZhuJunCuipo(f);
}
