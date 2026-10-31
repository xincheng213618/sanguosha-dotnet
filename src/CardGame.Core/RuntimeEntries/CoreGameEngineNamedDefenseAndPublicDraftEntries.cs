#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineNamedDefenseAndPublicDraftRuntime))]
internal sealed partial class CoreGameEngineNamedDefenseAndPublicDraftEntries
{
    private readonly Func<EngineOwners.CoreGameEngineNamedDefenseAndPublicDraftRuntime> _resolve;
    private CoreGameEngineNamedDefenseAndPublicDraftEntries(Func<EngineOwners.CoreGameEngineNamedDefenseAndPublicDraftRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNamedDefenseAndPublicDraftEntries Create(Func<EngineOwners.CoreGameEngineNamedDefenseAndPublicDraftRuntime> resolve) => new(resolve);
}
