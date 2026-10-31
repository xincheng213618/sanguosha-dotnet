#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineBoundPublicPileProgramsRuntime))]
internal sealed partial class CoreGameEngineBoundPublicPileProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBoundPublicPileProgramsRuntime> _resolve;
    private CoreGameEngineBoundPublicPileProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBoundPublicPileProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineBoundPublicPileProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBoundPublicPileProgramsRuntime> resolve) => new(resolve);
}
