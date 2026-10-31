#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LiuPiCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLiuPiOlEntries> getCharacterLiuPiOlRuntime) : ILiuPiProgramHost
{
    public SkillProgramStepOutcome Exchange(ProgramSkillFrame f) => getCharacterLiuPiOlRuntime().BeginLiuPiExchange(f);
}
