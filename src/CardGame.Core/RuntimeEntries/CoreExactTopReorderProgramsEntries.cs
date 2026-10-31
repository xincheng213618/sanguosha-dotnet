#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreExactTopReorderProgramsRuntime))]
internal sealed partial class CoreExactTopReorderProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreExactTopReorderProgramsRuntime> _resolve;
    private CoreExactTopReorderProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreExactTopReorderProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreExactTopReorderProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreExactTopReorderProgramsRuntime> resolve) => new(resolve);
}
