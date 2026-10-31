#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAdjacentRedTargetsRuntime))]
internal sealed partial class CoreGameEngineAdjacentRedTargetsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdjacentRedTargetsRuntime> _resolve;
    private CoreGameEngineAdjacentRedTargetsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdjacentRedTargetsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAdjacentRedTargetsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdjacentRedTargetsRuntime> resolve) => new(resolve);
}
