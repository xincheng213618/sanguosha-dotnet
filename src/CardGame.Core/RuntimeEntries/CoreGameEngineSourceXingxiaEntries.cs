#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSourceXingxiaRuntime))]
internal sealed partial class CoreGameEngineSourceXingxiaEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceXingxiaRuntime> _resolve;
    private CoreGameEngineSourceXingxiaEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceXingxiaRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSourceXingxiaEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceXingxiaRuntime> resolve) => new(resolve);
}
