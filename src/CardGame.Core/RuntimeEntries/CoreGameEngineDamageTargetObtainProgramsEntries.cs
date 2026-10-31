#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetObtainProgramsRuntime))]
internal sealed partial class CoreGameEngineDamageTargetObtainProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetObtainProgramsRuntime> _resolve;
    private CoreGameEngineDamageTargetObtainProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetObtainProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDamageTargetObtainProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetObtainProgramsRuntime> resolve) => new(resolve);
}
