#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineExtendedViewAsRuntime))]
internal sealed partial class CoreGameEngineExtendedViewAsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineExtendedViewAsRuntime> _resolve;
    private CoreGameEngineExtendedViewAsEntries(Func<EngineOwners.CoreGameEngineExtendedViewAsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineExtendedViewAsEntries Create(Func<EngineOwners.CoreGameEngineExtendedViewAsRuntime> resolve) => new(resolve);
}
