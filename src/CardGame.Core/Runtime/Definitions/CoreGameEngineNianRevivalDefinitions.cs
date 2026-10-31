#nullable enable
using global::CardGame.Core;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal static class CoreGameEngineNianRevivalDefinitions
{
internal static readonly CardMoveReason NianRevivalDiscard = new("activity.nian.revival-discard");
internal static readonly CardMoveReason NianRevivalDraw = new("activity.nian.revival-draw");
}
