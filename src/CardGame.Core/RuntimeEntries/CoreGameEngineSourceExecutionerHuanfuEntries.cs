#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSourceExecutionerHuanfuRuntime))]
internal sealed partial class CoreGameEngineSourceExecutionerHuanfuEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceExecutionerHuanfuRuntime> _resolve;
    private CoreGameEngineSourceExecutionerHuanfuEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceExecutionerHuanfuRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSourceExecutionerHuanfuEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceExecutionerHuanfuRuntime> resolve) => new(resolve);
}
