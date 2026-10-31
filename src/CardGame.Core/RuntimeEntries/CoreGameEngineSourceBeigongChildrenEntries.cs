#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSourceBeigongChildrenRuntime))]
internal sealed partial class CoreGameEngineSourceBeigongChildrenEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSourceBeigongChildrenRuntime> _resolve;
    private CoreGameEngineSourceBeigongChildrenEntries(Func<EngineOwners.CoreGameEngineSourceBeigongChildrenRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSourceBeigongChildrenEntries Create(Func<EngineOwners.CoreGameEngineSourceBeigongChildrenRuntime> resolve) => new(resolve);
}
