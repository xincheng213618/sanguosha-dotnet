#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineHpChangeProgramsPendingHpChangesAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.HpChangeContext> _pendingHpChanges { get; }
}
