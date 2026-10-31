#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheBadaoEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheBadaoRuntime> _resolve;
    private CoreGameEngineShanheBadaoEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheBadaoRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheBadaoEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheBadaoRuntime> resolve) => new(resolve);
}
