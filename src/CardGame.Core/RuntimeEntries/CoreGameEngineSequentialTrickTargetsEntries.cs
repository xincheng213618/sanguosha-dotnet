#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSequentialTrickTargetsRuntime))]
internal sealed partial class CoreGameEngineSequentialTrickTargetsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSequentialTrickTargetsRuntime> _resolve;
    private CoreGameEngineSequentialTrickTargetsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSequentialTrickTargetsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSequentialTrickTargetsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSequentialTrickTargetsRuntime> resolve) => new(resolve);
}
