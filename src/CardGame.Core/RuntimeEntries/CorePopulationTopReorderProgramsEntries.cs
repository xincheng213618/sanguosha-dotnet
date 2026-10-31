#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CorePopulationTopReorderProgramsRuntime))]
internal sealed partial class CorePopulationTopReorderProgramsEntries
{
    private readonly Func<EngineOwners.CorePopulationTopReorderProgramsRuntime> _resolve;
    private CorePopulationTopReorderProgramsEntries(Func<EngineOwners.CorePopulationTopReorderProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CorePopulationTopReorderProgramsEntries Create(Func<EngineOwners.CorePopulationTopReorderProgramsRuntime> resolve) => new(resolve);
}
