#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOwnPublicPileConversionsRuntime))]
internal sealed partial class CoreGameEngineOwnPublicPileConversionsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnPublicPileConversionsRuntime> _resolve;
    private CoreGameEngineOwnPublicPileConversionsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnPublicPileConversionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOwnPublicPileConversionsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnPublicPileConversionsRuntime> resolve) => new(resolve);
}
