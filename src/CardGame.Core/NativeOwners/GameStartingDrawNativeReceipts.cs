#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class GameStartingDrawNativeReceipts(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterTangTangOlEntries> getCharacterTangTangOlRuntime) : IGameStartingDrawNativeReceipts
{
    public IReadOnlyList<TangTangGain>? CurrentTangTangGains(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => getCharacterTangTangOlRuntime().CurrentTangTangGains(candidate, context);
}
