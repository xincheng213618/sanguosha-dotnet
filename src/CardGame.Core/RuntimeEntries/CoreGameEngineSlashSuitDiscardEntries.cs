#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSlashSuitDiscardEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashSuitDiscardRuntime> _resolve;
    private CoreGameEngineSlashSuitDiscardEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashSuitDiscardRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSlashSuitDiscardEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashSuitDiscardRuntime> resolve) => new(resolve);
}
