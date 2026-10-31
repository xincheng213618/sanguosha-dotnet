#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDamageCardColorHealthRuntime))]
internal sealed partial class CoreGameEngineDamageCardColorHealthEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDamageCardColorHealthRuntime> _resolve;
    private CoreGameEngineDamageCardColorHealthEntries(Func<EngineOwners.CoreGameEngineDamageCardColorHealthRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDamageCardColorHealthEntries Create(Func<EngineOwners.CoreGameEngineDamageCardColorHealthRuntime> resolve) => new(resolve);
}
