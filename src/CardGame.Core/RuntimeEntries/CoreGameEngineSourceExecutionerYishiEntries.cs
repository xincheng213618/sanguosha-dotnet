#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSourceExecutionerYishiRuntime))]
internal sealed partial class CoreGameEngineSourceExecutionerYishiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceExecutionerYishiRuntime> _resolve;
    private CoreGameEngineSourceExecutionerYishiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceExecutionerYishiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSourceExecutionerYishiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceExecutionerYishiRuntime> resolve) => new(resolve);
}
