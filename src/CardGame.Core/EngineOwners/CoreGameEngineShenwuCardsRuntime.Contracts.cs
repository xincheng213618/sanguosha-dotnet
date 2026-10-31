#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreShenwuCardsCoreProgramLifecycleOperations : IProgramRuntimeOperations
{
    ProgramInteractionRouter ProgramInteractions { get; }
}
