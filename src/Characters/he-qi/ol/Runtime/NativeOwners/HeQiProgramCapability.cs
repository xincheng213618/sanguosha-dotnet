#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HeQiProgramCapability(Func<RuntimeEntries.CharacterHeQiOlEntries> getCharacterHeQiOlRuntime) : IHeQiProgramHost
{
    public SkillProgramStepOutcome BeginHeQi(ProgramSkillFrame f, int target) => getCharacterHeQiOlRuntime().BeginHeQi(f, target);
}
