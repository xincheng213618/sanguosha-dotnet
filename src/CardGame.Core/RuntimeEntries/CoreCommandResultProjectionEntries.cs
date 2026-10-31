#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreCommandResultProjectionEntries
{
    private readonly Func<EngineOwners.CoreCommandResultProjectionRuntime> _resolve;
    private CoreCommandResultProjectionEntries(Func<EngineOwners.CoreCommandResultProjectionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreCommandResultProjectionEntries Create(Func<EngineOwners.CoreCommandResultProjectionRuntime> resolve) => new(resolve);
}
