#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreCommandResultProjectionEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreCommandResultProjectionRuntime> _resolve;
    private CoreCommandResultProjectionEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreCommandResultProjectionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreCommandResultProjectionEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreCommandResultProjectionRuntime> resolve) => new(resolve);
}
