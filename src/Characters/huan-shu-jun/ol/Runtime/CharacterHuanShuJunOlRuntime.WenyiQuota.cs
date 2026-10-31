#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuanShuJunOlRuntime
{
    internal int WenyiRemaining(int owner) => Math.Max(0, 1 + _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MiaoxinWenyiRefreshedEvent>().Count(e => e.OwnerSeat == owner) - _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WenyiPaidEvent>().Count(e => e.OwnerSeat == owner));
}
