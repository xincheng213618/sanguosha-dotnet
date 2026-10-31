#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheFengxianEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheFengxianRuntime> _resolve;
    private CoreGameEngineShanheFengxianEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheFengxianRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheFengxianEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheFengxianRuntime> resolve) => new(resolve);
}
