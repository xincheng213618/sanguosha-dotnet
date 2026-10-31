#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineOwnedExactCountAndNamedTargetsRuntime))]
internal sealed partial class CoreGameEngineOwnedExactCountAndNamedTargetsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineOwnedExactCountAndNamedTargetsRuntime> _resolve;
    private CoreGameEngineOwnedExactCountAndNamedTargetsEntries(Func<EngineOwners.CoreGameEngineOwnedExactCountAndNamedTargetsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOwnedExactCountAndNamedTargetsEntries Create(Func<EngineOwners.CoreGameEngineOwnedExactCountAndNamedTargetsRuntime> resolve) => new(resolve);
}
