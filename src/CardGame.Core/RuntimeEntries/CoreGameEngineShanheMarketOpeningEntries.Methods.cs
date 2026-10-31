#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheMarketOpeningEntries
{
    internal bool HasShanheMarketPurchaseObserver(long id) => _resolve().HasShanheMarketPurchaseObserver(id);
    internal bool IsExactShanheMarketOpeningWindow(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window) => _resolve().IsExactShanheMarketOpeningWindow(window);
    internal bool TryBeginShanheMarketOpening(global::CardGame.Core.ProgramSkillFrame execution, global::CardGame.Core.ShanheMarketPurchaseFrame market, global::System.Collections.Generic.IReadOnlyList<string> instanceIds) => _resolve().TryBeginShanheMarketOpening(execution, market, instanceIds);
}
