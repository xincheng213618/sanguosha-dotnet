#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAdvancedPrerequisitesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedPrerequisitesRuntime> _resolve;
    private CoreGameEngineAdvancedPrerequisitesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedPrerequisitesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAdvancedPrerequisitesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedPrerequisitesRuntime> resolve) => new(resolve);
}
