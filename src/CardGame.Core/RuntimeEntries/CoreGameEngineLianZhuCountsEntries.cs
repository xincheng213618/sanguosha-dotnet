#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineLianZhuCountsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLianZhuCountsRuntime> _resolve;
    private CoreGameEngineLianZhuCountsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLianZhuCountsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineLianZhuCountsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLianZhuCountsRuntime> resolve) => new(resolve);
}
