#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheQianlongEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheQianlongRuntime> _resolve;
    private CoreGameEngineShanheQianlongEntries(Func<EngineOwners.CoreGameEngineShanheQianlongRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheQianlongEntries Create(Func<EngineOwners.CoreGameEngineShanheQianlongRuntime> resolve) => new(resolve);
}
