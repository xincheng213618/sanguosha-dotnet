#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryLiuYouQueries(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterLiuYouAnniversaryRuntime>> getCharacterLiuYouAnniversaryRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState) : IAnniversaryLiuYouQueries
{
    public bool Target(int owner, int target) => getCharacterLiuYouAnniversaryRuntime().Value.LiuYouTarget(owner, target);
    public int Hp(int owner) => matchParticipantsState.ReferenceToPlayers[owner].Hp;
}
