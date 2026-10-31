#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNamedDefenseAndPublicDraftEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNamedDefenseAndPublicDraftRuntime> _resolve;
    private CoreGameEngineNamedDefenseAndPublicDraftEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNamedDefenseAndPublicDraftRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNamedDefenseAndPublicDraftEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNamedDefenseAndPublicDraftRuntime> resolve) => new(resolve);
}
