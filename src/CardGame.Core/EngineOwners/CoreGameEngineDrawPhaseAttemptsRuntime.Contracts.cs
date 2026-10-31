#nullable enable

namespace CardGame.Core.EngineOwners;
internal interface ICoreDrawPhaseAttemptsOwnedDrawPhaseOperations : IProgramRuntimeOperations
{
    OwnedDrawPhaseRuntime Get();
}
