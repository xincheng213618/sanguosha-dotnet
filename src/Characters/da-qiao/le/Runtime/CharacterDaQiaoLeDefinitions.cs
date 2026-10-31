#nullable enable
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal static class CharacterDaQiaoLeDefinitions
{
internal static readonly IReadOnlyList<CardKind> ZixiSlots = Array.AsReadOnly(new[] { CardKind.Indulgence, CardKind.SupplyShortage, CardKind.Lightning });
}
