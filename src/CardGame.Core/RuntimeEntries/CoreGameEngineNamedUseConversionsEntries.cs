#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineNamedUseConversionsRuntime))]
internal sealed partial class CoreGameEngineNamedUseConversionsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineNamedUseConversionsRuntime> _resolve;
    private CoreGameEngineNamedUseConversionsEntries(Func<EngineOwners.CoreGameEngineNamedUseConversionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNamedUseConversionsEntries Create(Func<EngineOwners.CoreGameEngineNamedUseConversionsRuntime> resolve) => new(resolve);
}
