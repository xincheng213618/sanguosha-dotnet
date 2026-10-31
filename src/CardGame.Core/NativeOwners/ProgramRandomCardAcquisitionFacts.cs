#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramRandomCardAcquisitionFacts(EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreGameEngineTableOrderEntries> getCoreGameEngineTableOrderRuntime) : IProgramRandomCardAcquisitionFacts
{
    public IReadOnlyList<CharacterState> Players => matchParticipantsState.ReferenceToPlayers;

    public int RelativeTableOrder(int from, int target) => getCoreGameEngineTableOrderRuntime().RelativeTableOrder(from, target);
}
