#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNamedUseConversionsRuntime))]
internal sealed partial class CoreGameEngineNamedUseConversionsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNamedUseConversionsRuntime> _resolve;
    private CoreGameEngineNamedUseConversionsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNamedUseConversionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNamedUseConversionsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNamedUseConversionsRuntime> resolve) => new(resolve);
}
