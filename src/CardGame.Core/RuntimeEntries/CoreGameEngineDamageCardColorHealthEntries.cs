#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageCardColorHealthRuntime))]
internal sealed partial class CoreGameEngineDamageCardColorHealthEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageCardColorHealthRuntime> _resolve;
    private CoreGameEngineDamageCardColorHealthEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageCardColorHealthRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDamageCardColorHealthEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageCardColorHealthRuntime> resolve) => new(resolve);
}
