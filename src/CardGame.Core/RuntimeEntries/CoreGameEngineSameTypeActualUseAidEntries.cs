#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSameTypeActualUseAidEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSameTypeActualUseAidRuntime> _resolve;
    private CoreGameEngineSameTypeActualUseAidEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSameTypeActualUseAidRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSameTypeActualUseAidEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSameTypeActualUseAidRuntime> resolve) => new(resolve);
}
