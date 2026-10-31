#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheLeizhenEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheLeizhenRuntime> _resolve;
    private CoreGameEngineShanheLeizhenEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheLeizhenRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheLeizhenEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheLeizhenRuntime> resolve) => new(resolve);
}
