#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class GameStartingDrawNativeReceipts(Func<RuntimeEntries.CharacterTangTangOlEntries> getCharacterTangTangOlRuntime) : IGameStartingDrawNativeReceipts
{
    public IReadOnlyList<TangTangGain>? CurrentTangTangGains(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => getCharacterTangTangOlRuntime().CurrentTangTangGains(candidate, context);
}
