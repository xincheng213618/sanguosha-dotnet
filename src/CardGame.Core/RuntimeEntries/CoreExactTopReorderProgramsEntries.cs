#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreExactTopReorderProgramsRuntime))]
internal sealed partial class CoreExactTopReorderProgramsEntries
{
    private readonly Func<EngineOwners.CoreExactTopReorderProgramsRuntime> _resolve;
    private CoreExactTopReorderProgramsEntries(Func<EngineOwners.CoreExactTopReorderProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreExactTopReorderProgramsEntries Create(Func<EngineOwners.CoreExactTopReorderProgramsRuntime> resolve) => new(resolve);
}
