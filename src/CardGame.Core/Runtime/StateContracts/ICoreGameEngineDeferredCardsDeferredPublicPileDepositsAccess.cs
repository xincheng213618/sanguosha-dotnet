#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineDeferredCardsDeferredPublicPileDepositsAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.DeferredPublicPileDeposit> _deferredPublicPileDeposits { get; }
}
