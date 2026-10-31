#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSourceDingxiRuntime))]
internal sealed partial class CoreGameEngineSourceDingxiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceDingxiRuntime> _resolve;
    private CoreGameEngineSourceDingxiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceDingxiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSourceDingxiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceDingxiRuntime> resolve) => new(resolve);
}
