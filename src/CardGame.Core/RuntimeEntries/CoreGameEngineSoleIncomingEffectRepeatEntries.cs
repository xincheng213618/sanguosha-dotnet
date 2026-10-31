#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSoleIncomingEffectRepeatRuntime))]
internal sealed partial class CoreGameEngineSoleIncomingEffectRepeatEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSoleIncomingEffectRepeatRuntime> _resolve;
    private CoreGameEngineSoleIncomingEffectRepeatEntries(Func<EngineOwners.CoreGameEngineSoleIncomingEffectRepeatRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSoleIncomingEffectRepeatEntries Create(Func<EngineOwners.CoreGameEngineSoleIncomingEffectRepeatRuntime> resolve) => new(resolve);
}
