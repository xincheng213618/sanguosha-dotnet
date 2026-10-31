#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LiuQiProgramCapability(Func<RuntimeEntries.CharacterLiuQiOlEntries> getCharacterLiuQiOlRuntime) : ILiuQiProgramHost
{
    public SkillProgramStepOutcome BeginLiuQi(ProgramSkillFrame f, SkillProgramEffect e) => getCharacterLiuQiOlRuntime().BeginLiuQi(f, e);
}
