#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouYuMouMobileRuntime
{
    internal int MouZyMobileHandLimit(int owner) => HasMouZhouYuMobileCapability && MouZyMobileTurn()is { } turn ? _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouZhouYuMobileYingziIssuedEvent>().Where(e => e.Source.OwnerSeat == owner && e.Turn == turn).Sum(e => e.Count) : 0;
}
