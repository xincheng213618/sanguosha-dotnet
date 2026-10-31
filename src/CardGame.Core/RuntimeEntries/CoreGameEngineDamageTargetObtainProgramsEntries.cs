#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageTargetObtainProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetObtainProgramsRuntime> _resolve;
    private CoreGameEngineDamageTargetObtainProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetObtainProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDamageTargetObtainProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetObtainProgramsRuntime> resolve) => new(resolve);
}
