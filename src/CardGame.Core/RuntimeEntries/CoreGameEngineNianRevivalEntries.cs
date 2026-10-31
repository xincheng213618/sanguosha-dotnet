#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNianRevivalRuntime))]
internal sealed partial class CoreGameEngineNianRevivalEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNianRevivalRuntime> _resolve;
    private CoreGameEngineNianRevivalEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNianRevivalRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNianRevivalEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNianRevivalRuntime> resolve) => new(resolve);
}
