#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineDeferredCardsDeferredPublicPileDepositsAccess : IProgramRuntimeOperations
{
    ref readonly List<DeferredPublicPileDeposit> _deferredPublicPileDeposits { get; }
}
