#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGameActivationProgressRuntime))]
internal sealed partial class CoreGameEngineGameActivationProgressEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGameActivationProgressRuntime> _resolve;
    private CoreGameEngineGameActivationProgressEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGameActivationProgressRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGameActivationProgressEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGameActivationProgressRuntime> resolve) => new(resolve);
}
