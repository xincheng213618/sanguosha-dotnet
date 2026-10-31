#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRicePileRuntime))]
internal sealed partial class CoreGameEngineRicePileEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRicePileRuntime> _resolve;
    private CoreGameEngineRicePileEntries(Func<EngineOwners.CoreGameEngineRicePileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRicePileEntries Create(Func<EngineOwners.CoreGameEngineRicePileRuntime> resolve) => new(resolve);
}
