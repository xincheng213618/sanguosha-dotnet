#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ZhuJunCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhuJunOlEntries> getCharacterZhuJunOlRuntime) : IZhuJunHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterZhuJunOlRuntime().BeginZhuJunCuipo(f);
}
