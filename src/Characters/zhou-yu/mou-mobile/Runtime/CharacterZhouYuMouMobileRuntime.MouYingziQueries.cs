#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouYuMouMobileRuntime
{
    internal int MouZyMobileHandLimit(int owner) => HasMouZhouYuMobileCapability && MouZyMobileTurn()is { } turn ? _coreProgramEventHistoryOperations.ProgramEventHistory<MouZhouYuMobileYingziIssuedEvent>().Where(e => e.Source.OwnerSeat == owner && e.Turn == turn).Sum(e => e.Count) : 0;
}
