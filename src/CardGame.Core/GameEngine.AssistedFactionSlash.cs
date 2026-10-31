namespace CardGame.Core;

public sealed partial class GameEngine
{
internal sealed record AssistedFactionSlashCard(Card Card, CardKind Kind, CardConversionSource? Source);
}
