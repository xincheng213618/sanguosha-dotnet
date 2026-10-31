#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSourceFactionPreventionsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSourceFactionPreventionsRuntime> _resolve;
    private CoreGameEngineSourceFactionPreventionsEntries(Func<EngineOwners.CoreGameEngineSourceFactionPreventionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSourceFactionPreventionsEntries Create(Func<EngineOwners.CoreGameEngineSourceFactionPreventionsRuntime> resolve) => new(resolve);
}
