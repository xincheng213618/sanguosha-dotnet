#nullable enable
using global::CardGame.Core;
using System.Globalization;

using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal static class CoreGameEngineDragonBoatCardsDefinitions
{
internal static readonly IReadOnlyList<CardKind> DragonBoatOrdinaryTrickKinds = Array.AsReadOnly(new[] { CardKind.SameBoat, CardKind.StriveUpstream, CardKind.AgainstCurrent });
}
