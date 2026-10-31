#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineUniqueHpPeerProgramsRuntime))]
internal sealed partial class CoreGameEngineUniqueHpPeerProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineUniqueHpPeerProgramsRuntime> _resolve;
    private CoreGameEngineUniqueHpPeerProgramsEntries(Func<EngineOwners.CoreGameEngineUniqueHpPeerProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineUniqueHpPeerProgramsEntries Create(Func<EngineOwners.CoreGameEngineUniqueHpPeerProgramsRuntime> resolve) => new(resolve);
}
