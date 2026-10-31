#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreExactTopReorderProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreExactTopReorderProgramsRuntime> _resolve;
    private CoreExactTopReorderProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreExactTopReorderProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreExactTopReorderProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreExactTopReorderProgramsRuntime> resolve) => new(resolve);
}
