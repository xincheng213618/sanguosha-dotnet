#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineLifetimeSlashUsesRuntime
{
    internal int LifetimeSlashUseCount(int seat)
    {
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(seat))
            throw new ArgumentOutOfRangeException("seat");
        var changes = _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramCardUseActorReplacedEvent>().GroupBy(fact => fact.CardUseFrameId).ToDictionary(group => group.Key, group => group.ToArray());
        var count = 0;
        foreach (var group in _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseDeclaredEvent>().Where(fact => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(fact.CardKind)).GroupBy(fact => fact.ResolutionId))
        {
            var declaration = group.First();
            if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(declaration.SourceSeat) || group.Any(fact => fact.SourceSeat != declaration.SourceSeat || fact.CardId != declaration.CardId || fact.CardKind != declaration.CardKind))
                throw new InvalidOperationException("A lifetime Slash use requires one consistent native declaration.");
            var actor = declaration.SourceSeat;
            if (changes.TryGetValue(declaration.ResolutionId, out var replacements))
            {
                int? provider = null;
                foreach (var replacement in replacements)
                {
                    if (replacement.PreviousActorSeat != actor || replacement.OwnerSeat != actor || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(replacement.ActorSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(replacement.ProviderSeat) || provider is { } originalProvider && originalProvider != replacement.ProviderSeat)
                        throw new InvalidOperationException("A lifetime Slash use lost its native actor-replacement chain.");
                    actor = replacement.ActorSeat;
                    provider = replacement.ProviderSeat;
                }
            }

            if (actor == seat)
                count = checked(count + 1);
        }

        return count;
    }
}
