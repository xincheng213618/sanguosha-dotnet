using System.Globalization;

namespace CardGame.Core;

public sealed partial class GameEngine
{
internal sealed record SourceDingxiUsePlan(int CardId, CardKind Kind, LegalActionKind Action, CardKind? Required);
}
