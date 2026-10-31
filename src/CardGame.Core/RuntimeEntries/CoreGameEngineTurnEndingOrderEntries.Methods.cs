#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTurnEndingOrderEntries
{
    internal bool TryResolveTurnEndingOrder(PromptChoice choice) => _resolve().TryResolveTurnEndingOrder(choice);
}
