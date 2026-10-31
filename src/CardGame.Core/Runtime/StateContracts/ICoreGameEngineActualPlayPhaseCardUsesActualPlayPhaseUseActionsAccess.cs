#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineActualPlayPhaseCardUsesActualPlayPhaseUseActionsAccess : IProgramRuntimeOperations
{
    ref readonly HashSet<long> _actualPlayPhaseUseActions { get; }
}
