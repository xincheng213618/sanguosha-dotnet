#nullable enable
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal static class CoreGameEngineGuanduCardsDefinitions
{
internal static readonly IReadOnlyList<CardKind> GuanduOrdinaryTrickKinds = Array.AsReadOnly(new[] { CardKind.StoreGrain, CardKind.Reinforcements, CardKind.UnitedEffort });
}
