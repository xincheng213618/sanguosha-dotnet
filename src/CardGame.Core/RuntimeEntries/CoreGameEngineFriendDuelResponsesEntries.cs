#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFriendDuelResponsesRuntime))]
internal sealed partial class CoreGameEngineFriendDuelResponsesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendDuelResponsesRuntime> _resolve;
    private CoreGameEngineFriendDuelResponsesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendDuelResponsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFriendDuelResponsesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendDuelResponsesRuntime> resolve) => new(resolve);
}
