#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineBoundPublicPileProgramsRuntime))]
internal sealed partial class CoreGameEngineBoundPublicPileProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineBoundPublicPileProgramsRuntime> _resolve;
    private CoreGameEngineBoundPublicPileProgramsEntries(Func<EngineOwners.CoreGameEngineBoundPublicPileProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineBoundPublicPileProgramsEntries Create(Func<EngineOwners.CoreGameEngineBoundPublicPileProgramsRuntime> resolve) => new(resolve);
}
