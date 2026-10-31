#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTurnEndingOrderEntries
{
    internal bool TryResolveTurnEndingOrder(global::CardGame.Core.PromptChoice choice) => _resolve().TryResolveTurnEndingOrder(choice);
}
