#nullable enable
using global::CardGame.Core;
using System.Globalization;

using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal static class CoreGameEngineColdDamageDefinitions
{
internal static readonly CardMoveReason ColdDamageDiscardReason = new("damage.cold.discard");
}
