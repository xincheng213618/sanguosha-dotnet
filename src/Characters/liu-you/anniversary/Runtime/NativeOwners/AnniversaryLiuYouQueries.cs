#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryLiuYouQueries(Func<RuntimeEntries.CharacterLiuYouAnniversaryEntries> getCharacterLiuYouAnniversaryRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState) : IAnniversaryLiuYouQueries
{
    public bool Target(int owner, int target) => getCharacterLiuYouAnniversaryRuntime().LiuYouTarget(owner, target);
    public int Hp(int owner) => matchParticipantsState.ReferenceToPlayers[owner].Hp;
}
