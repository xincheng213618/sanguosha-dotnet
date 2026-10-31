#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangLiaoOlSpRuntime
{
    internal bool HasDiaolingAwakened(int seat) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DiaolingAwakenedEvent>().Any(e => e.OwnerSeat == seat);
}
