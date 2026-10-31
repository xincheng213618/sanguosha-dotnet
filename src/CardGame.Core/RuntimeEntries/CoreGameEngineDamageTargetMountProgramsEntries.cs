#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountProgramsRuntime))]
internal sealed partial class CoreGameEngineDamageTargetMountProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountProgramsRuntime> _resolve;
    private CoreGameEngineDamageTargetMountProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDamageTargetMountProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountProgramsRuntime> resolve) => new(resolve);
}
