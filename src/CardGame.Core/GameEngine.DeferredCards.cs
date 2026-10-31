namespace CardGame.Core;
public sealed record DeferredPublicPileDeposit(long FrameId, int OwnerSeat, int ProviderSeat, string SkillId, string SkillInstanceId, IReadOnlyList<int> CardIds, int CreatedTurn);
public sealed record DeferredPublicPileDepositedEvent(DeferredPublicPileDeposit Deposit) : IGameEvent;
public sealed record DeferredPublicPileObtainedEvent(int OwnerSeat, int ProviderSeat, IReadOnlyList<int> CardIds) : IGameEvent;
public sealed partial class GameEngine
{
    private ref readonly global::System.Collections.Generic.List<global::CardGame.Core.DeferredPublicPileDeposit> _deferredPublicPileDeposits => ref _deferredCards.ReferenceToDeferredPublicPileDeposits;
}
