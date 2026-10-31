#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineCardDeclarationsRuntime))]
internal sealed partial class CoreGameEngineCardDeclarationsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCardDeclarationsRuntime> _resolve;
    private CoreGameEngineCardDeclarationsEntries(Func<EngineOwners.CoreGameEngineCardDeclarationsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCardDeclarationsEntries Create(Func<EngineOwners.CoreGameEngineCardDeclarationsRuntime> resolve) => new(resolve);
}
