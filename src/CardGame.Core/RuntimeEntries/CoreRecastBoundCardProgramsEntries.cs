#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreRecastBoundCardProgramsEntries
{
    private readonly Func<EngineOwners.CoreRecastBoundCardProgramsRuntime> _resolve;
    private CoreRecastBoundCardProgramsEntries(Func<EngineOwners.CoreRecastBoundCardProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreRecastBoundCardProgramsEntries Create(Func<EngineOwners.CoreRecastBoundCardProgramsRuntime> resolve) => new(resolve);
}
