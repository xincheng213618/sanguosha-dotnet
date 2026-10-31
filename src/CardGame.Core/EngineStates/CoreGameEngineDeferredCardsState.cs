#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineDeferredCardsState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineDeferredCardsDeferredPublicPileDepositsAccess
{
    private global::System.Collections.Generic.List<global::CardGame.Core.DeferredPublicPileDeposit> _deferredPublicPileDeposits = [];
    internal ref readonly global::System.Collections.Generic.List<global::CardGame.Core.DeferredPublicPileDeposit> ReferenceToDeferredPublicPileDeposits => ref _deferredPublicPileDeposits;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.DeferredPublicPileDeposit> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineDeferredCardsDeferredPublicPileDepositsAccess._deferredPublicPileDeposits => ref _deferredPublicPileDeposits;
}
