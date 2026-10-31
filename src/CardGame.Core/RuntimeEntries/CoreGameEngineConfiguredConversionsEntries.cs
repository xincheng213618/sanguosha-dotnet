#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineConfiguredConversionsRuntime))]
internal sealed partial class CoreGameEngineConfiguredConversionsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineConfiguredConversionsRuntime> _resolve;
    private CoreGameEngineConfiguredConversionsEntries(Func<EngineOwners.CoreGameEngineConfiguredConversionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineConfiguredConversionsEntries Create(Func<EngineOwners.CoreGameEngineConfiguredConversionsRuntime> resolve) => new(resolve);
}
