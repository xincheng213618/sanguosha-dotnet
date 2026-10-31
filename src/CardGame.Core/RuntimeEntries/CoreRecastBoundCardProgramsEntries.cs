#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreRecastBoundCardProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreRecastBoundCardProgramsRuntime> _resolve;
    private CoreRecastBoundCardProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreRecastBoundCardProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreRecastBoundCardProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreRecastBoundCardProgramsRuntime> resolve) => new(resolve);
}
