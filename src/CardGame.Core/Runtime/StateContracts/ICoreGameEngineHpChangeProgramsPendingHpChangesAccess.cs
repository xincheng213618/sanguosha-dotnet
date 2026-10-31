#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineHpChangeProgramsPendingHpChangesAccess : IProgramRuntimeOperations
{
    ref readonly List<HpChangeContext> _pendingHpChanges { get; }
}
