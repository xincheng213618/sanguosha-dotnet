#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineGameActivationProgressRuntime))]
internal sealed partial class CoreGameEngineGameActivationProgressEntries
{
    private readonly Func<EngineOwners.CoreGameEngineGameActivationProgressRuntime> _resolve;
    private CoreGameEngineGameActivationProgressEntries(Func<EngineOwners.CoreGameEngineGameActivationProgressRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGameActivationProgressEntries Create(Func<EngineOwners.CoreGameEngineGameActivationProgressRuntime> resolve) => new(resolve);
}
