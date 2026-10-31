#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheMarketOpeningEntries
{
    internal bool IsExactShanheMarketOpeningWindow(ProgramLifecycleTriggerWindowFrame window) => _resolve().IsExactShanheMarketOpeningWindow(window);
    internal bool TryBeginShanheMarketOpening(ProgramSkillFrame execution, ShanheMarketPurchaseFrame market, IReadOnlyList<string> instanceIds) => _resolve().TryBeginShanheMarketOpening(execution, market, instanceIds);
}
