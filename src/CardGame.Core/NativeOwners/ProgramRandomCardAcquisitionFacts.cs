#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramRandomCardAcquisitionFacts(global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries> getCoreGameEngineTableOrderRuntime) : IProgramRandomCardAcquisitionFacts
{
    public IReadOnlyList<CharacterState> Players => matchParticipantsState.ReferenceToPlayers;

    public int RelativeTableOrder(int from, int target) => getCoreGameEngineTableOrderRuntime().RelativeTableOrder(from, target);
}
