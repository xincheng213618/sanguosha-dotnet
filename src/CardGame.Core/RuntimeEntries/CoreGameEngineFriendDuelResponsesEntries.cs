#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineFriendDuelResponsesRuntime))]
internal sealed partial class CoreGameEngineFriendDuelResponsesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineFriendDuelResponsesRuntime> _resolve;
    private CoreGameEngineFriendDuelResponsesEntries(Func<EngineOwners.CoreGameEngineFriendDuelResponsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFriendDuelResponsesEntries Create(Func<EngineOwners.CoreGameEngineFriendDuelResponsesRuntime> resolve) => new(resolve);
}
