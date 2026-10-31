#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSameTypeActualUseAidRuntime))]
internal sealed partial class CoreGameEngineSameTypeActualUseAidEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSameTypeActualUseAidRuntime> _resolve;
    private CoreGameEngineSameTypeActualUseAidEntries(Func<EngineOwners.CoreGameEngineSameTypeActualUseAidRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSameTypeActualUseAidEntries Create(Func<EngineOwners.CoreGameEngineSameTypeActualUseAidRuntime> resolve) => new(resolve);
}
