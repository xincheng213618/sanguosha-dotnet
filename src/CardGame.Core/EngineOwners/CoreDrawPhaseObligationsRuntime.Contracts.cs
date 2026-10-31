#nullable enable

namespace CardGame.Core.EngineOwners;
internal interface ICoreDrawPhaseObligationsOwnedDrawPhaseOperations : IProgramRuntimeOperations
{
    OwnedDrawPhaseRuntime Get();
}
