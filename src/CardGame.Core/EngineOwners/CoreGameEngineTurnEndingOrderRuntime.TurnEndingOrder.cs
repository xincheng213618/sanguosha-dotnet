#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineTurnEndingOrderRuntime
{
    internal bool TryResolveTurnEndingOrder(PromptChoice choice) => _programPhaseBoundaryOperations.Get().TryResolveTurnEndingOrder(choice);
}
