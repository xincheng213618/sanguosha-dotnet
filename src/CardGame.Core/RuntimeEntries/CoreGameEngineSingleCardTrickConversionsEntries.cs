#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSingleCardTrickConversionsRuntime))]
internal sealed partial class CoreGameEngineSingleCardTrickConversionsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSingleCardTrickConversionsRuntime> _resolve;
    private CoreGameEngineSingleCardTrickConversionsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSingleCardTrickConversionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSingleCardTrickConversionsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSingleCardTrickConversionsRuntime> resolve) => new(resolve);
}
