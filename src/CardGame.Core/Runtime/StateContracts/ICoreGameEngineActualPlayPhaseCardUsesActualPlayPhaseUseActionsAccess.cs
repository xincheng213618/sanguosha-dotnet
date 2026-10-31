#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineActualPlayPhaseCardUsesActualPlayPhaseUseActionsAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.HashSet<long> _actualPlayPhaseUseActions { get; }
}
