#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LiuQiProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLiuQiOlEntries> getCharacterLiuQiOlRuntime) : ILiuQiProgramHost
{
    public SkillProgramStepOutcome BeginLiuQi(ProgramSkillFrame f, SkillProgramEffect e) => getCharacterLiuQiOlRuntime().BeginLiuQi(f, e);
}
