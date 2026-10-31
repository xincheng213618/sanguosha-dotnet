#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineUniqueHpPeerProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUniqueHpPeerProgramsRuntime> _resolve;
    private CoreGameEngineUniqueHpPeerProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUniqueHpPeerProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineUniqueHpPeerProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUniqueHpPeerProgramsRuntime> resolve) => new(resolve);
}
