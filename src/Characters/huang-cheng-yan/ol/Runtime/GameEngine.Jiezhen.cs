namespace CardGame.Core;

public sealed partial class GameEngine
{
internal sealed record JiezhenConversionState(int OwnerSeat, IReadOnlyList<string> ReplacedSkillIds,
        string BazhenGrantId);
}
