#nullable enable

namespace CardGame.Core.EngineOwners;
internal interface ICoreDiscardManualDiscardOperations : IProgramRuntimeOperations
{
    ManualDiscardRuntime Get();
}
