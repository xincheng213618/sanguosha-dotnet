#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSingleCardTrickConversionsRuntime))]
internal sealed partial class CoreGameEngineSingleCardTrickConversionsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSingleCardTrickConversionsRuntime> _resolve;
    private CoreGameEngineSingleCardTrickConversionsEntries(Func<EngineOwners.CoreGameEngineSingleCardTrickConversionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSingleCardTrickConversionsEntries Create(Func<EngineOwners.CoreGameEngineSingleCardTrickConversionsRuntime> resolve) => new(resolve);
}
