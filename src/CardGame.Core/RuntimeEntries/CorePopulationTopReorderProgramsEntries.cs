#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CorePopulationTopReorderProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePopulationTopReorderProgramsRuntime> _resolve;
    private CorePopulationTopReorderProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CorePopulationTopReorderProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CorePopulationTopReorderProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CorePopulationTopReorderProgramsRuntime> resolve) => new(resolve);
}
