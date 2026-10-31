namespace CardGame.Core;

public sealed partial class GameEngine
{
internal sealed record QinLangUseOption(LegalActionKind Kind,IReadOnlyList<int> TargetSeats,int? TargetCardId,CardKind? RequiredKind,string Description);
}
