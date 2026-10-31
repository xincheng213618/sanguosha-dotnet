#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuanShuJunOlRuntime
{
    internal int WenyiRemaining(int owner) => Math.Max(0, 1 + _coreProgramEventHistoryOperations.ProgramEventHistory<MiaoxinWenyiRefreshedEvent>().Count(e => e.OwnerSeat == owner) - _coreProgramEventHistoryOperations.ProgramEventHistory<WenyiPaidEvent>().Count(e => e.OwnerSeat == owner));
}
