#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamagePreventionProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamagePreventionProgramsRuntime> _resolve;
    private CoreGameEngineDamagePreventionProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamagePreventionProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDamagePreventionProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamagePreventionProgramsRuntime> resolve) => new(resolve);
}
