#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangLiaoOlSpRuntime
{
    internal bool HasDiaolingAwakened(int seat) => _coreProgramEventHistoryOperations.ProgramEventHistory<DiaolingAwakenedEvent>().Any(e => e.OwnerSeat == seat);
}
