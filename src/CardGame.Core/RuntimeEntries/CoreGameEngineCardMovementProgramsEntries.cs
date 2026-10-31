#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime))]
internal sealed partial class CoreGameEngineCardMovementProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> _resolve;
    private CoreGameEngineCardMovementProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCardMovementProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> resolve) => new(resolve);
}
