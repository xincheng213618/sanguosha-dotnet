#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineOwnPublicPileConversionsRuntime))]
internal sealed partial class CoreGameEngineOwnPublicPileConversionsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineOwnPublicPileConversionsRuntime> _resolve;
    private CoreGameEngineOwnPublicPileConversionsEntries(Func<EngineOwners.CoreGameEngineOwnPublicPileConversionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOwnPublicPileConversionsEntries Create(Func<EngineOwners.CoreGameEngineOwnPublicPileConversionsRuntime> resolve) => new(resolve);
}
