#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuanShuJunOlRuntime
{
    internal int WenyiRemaining(int owner) => Math.Max(0, 1 + _coreProgramEventHistoryOperations.ProgramEventHistory<MiaoxinWenyiRefreshedEvent>().Count(e => e.OwnerSeat == owner) - _coreProgramEventHistoryOperations.ProgramEventHistory<WenyiPaidEvent>().Count(e => e.OwnerSeat == owner));
}
