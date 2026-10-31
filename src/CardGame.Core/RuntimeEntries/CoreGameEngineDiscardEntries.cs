#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDiscardRuntime))]
internal sealed partial class CoreGameEngineDiscardEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDiscardRuntime> _resolve;
    private CoreGameEngineDiscardEntries(Func<EngineOwners.CoreGameEngineDiscardRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDiscardEntries Create(Func<EngineOwners.CoreGameEngineDiscardRuntime> resolve) => new(resolve);
}
