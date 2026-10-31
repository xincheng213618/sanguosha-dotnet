#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HeQiProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHeQiOlEntries> getCharacterHeQiOlRuntime) : IHeQiProgramHost
{
    public SkillProgramStepOutcome BeginHeQi(ProgramSkillFrame f, int target) => getCharacterHeQiOlRuntime().BeginHeQi(f, target);
}
