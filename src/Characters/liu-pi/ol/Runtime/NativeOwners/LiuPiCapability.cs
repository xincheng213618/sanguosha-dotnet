#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LiuPiCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterLiuPiOlRuntime>> getCharacterLiuPiOlRuntime) : ILiuPiProgramHost
{
    public SkillProgramStepOutcome Exchange(ProgramSkillFrame f) => getCharacterLiuPiOlRuntime().Value.BeginLiuPiExchange(f);
}
