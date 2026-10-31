#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAdvancedPrerequisitesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineAdvancedPrerequisitesRuntime> _resolve;
    private CoreGameEngineAdvancedPrerequisitesEntries(Func<EngineOwners.CoreGameEngineAdvancedPrerequisitesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAdvancedPrerequisitesEntries Create(Func<EngineOwners.CoreGameEngineAdvancedPrerequisitesRuntime> resolve) => new(resolve);
}
