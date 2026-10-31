#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineAdjacentRedTargetsRuntime))]
internal sealed partial class CoreGameEngineAdjacentRedTargetsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineAdjacentRedTargetsRuntime> _resolve;
    private CoreGameEngineAdjacentRedTargetsEntries(Func<EngineOwners.CoreGameEngineAdjacentRedTargetsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAdjacentRedTargetsEntries Create(Func<EngineOwners.CoreGameEngineAdjacentRedTargetsRuntime> resolve) => new(resolve);
}
