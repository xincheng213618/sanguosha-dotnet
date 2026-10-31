#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheFengxianEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheFengxianRuntime> _resolve;
    private CoreGameEngineShanheFengxianEntries(Func<EngineOwners.CoreGameEngineShanheFengxianRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheFengxianEntries Create(Func<EngineOwners.CoreGameEngineShanheFengxianRuntime> resolve) => new(resolve);
}
