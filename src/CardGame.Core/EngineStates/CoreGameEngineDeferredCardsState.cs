#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineDeferredCardsState : Contracts.ICoreGameEngineDeferredCardsDeferredPublicPileDepositsAccess
{
    private List<DeferredPublicPileDeposit> _deferredPublicPileDeposits = [];
    internal ref readonly List<DeferredPublicPileDeposit> ReferenceToDeferredPublicPileDeposits => ref _deferredPublicPileDeposits;

    ref readonly List<DeferredPublicPileDeposit> Contracts.ICoreGameEngineDeferredCardsDeferredPublicPileDepositsAccess._deferredPublicPileDeposits => ref _deferredPublicPileDeposits;
}
