namespace CardGame.Core;

public sealed partial class GameEngine
{
internal sealed record FactionProviderRewardReceipt(long OwnerFrameId, int DrawCount);
}
