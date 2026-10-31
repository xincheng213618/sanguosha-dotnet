#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineConfiguredConversionsRuntime))]
internal sealed partial class CoreGameEngineConfiguredConversionsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConfiguredConversionsRuntime> _resolve;
    private CoreGameEngineConfiguredConversionsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConfiguredConversionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineConfiguredConversionsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConfiguredConversionsRuntime> resolve) => new(resolve);
}
