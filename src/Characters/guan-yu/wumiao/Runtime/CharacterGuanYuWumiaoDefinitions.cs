#nullable enable
using global::CardGame.Core;
using System.Globalization;

using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal static class CharacterGuanYuWumiaoDefinitions
{
internal static readonly CardMoveReason WumiaoFloodDiscardReason = new("card.effect.wumiao-flood.discard");
}
