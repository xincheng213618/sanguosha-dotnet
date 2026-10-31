#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CorePindianOwnedPileSourcesRuntime))]
internal sealed partial class CorePindianOwnedPileSourcesEntries
{
    private readonly Func<EngineOwners.CorePindianOwnedPileSourcesRuntime> _resolve;
    private CorePindianOwnedPileSourcesEntries(Func<EngineOwners.CorePindianOwnedPileSourcesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CorePindianOwnedPileSourcesEntries Create(Func<EngineOwners.CorePindianOwnedPileSourcesRuntime> resolve) => new(resolve);
}
