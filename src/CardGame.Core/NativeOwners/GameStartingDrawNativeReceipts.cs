#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class GameStartingDrawNativeReceipts(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterTangTangOlRuntime>> getCharacterTangTangOlRuntime) : IGameStartingDrawNativeReceipts
{
    public IReadOnlyList<TangTangGain>? CurrentTangTangGains(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => getCharacterTangTangOlRuntime().Value.CurrentTangTangGains(candidate, context);
}
