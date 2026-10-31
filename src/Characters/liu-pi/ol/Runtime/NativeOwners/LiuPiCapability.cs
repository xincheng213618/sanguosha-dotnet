#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LiuPiCapability(Func<RuntimeEntries.CharacterLiuPiOlEntries> getCharacterLiuPiOlRuntime) : ILiuPiProgramHost
{
    public SkillProgramStepOutcome Exchange(ProgramSkillFrame f) => getCharacterLiuPiOlRuntime().BeginLiuPiExchange(f);
}
