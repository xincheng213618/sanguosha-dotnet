#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineLastDamageSourceReciprocityRuntime))]
internal sealed partial class CoreGameEngineLastDamageSourceReciprocityEntries
{
    private readonly Func<EngineOwners.CoreGameEngineLastDamageSourceReciprocityRuntime> _resolve;
    private CoreGameEngineLastDamageSourceReciprocityEntries(Func<EngineOwners.CoreGameEngineLastDamageSourceReciprocityRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineLastDamageSourceReciprocityEntries Create(Func<EngineOwners.CoreGameEngineLastDamageSourceReciprocityRuntime> resolve) => new(resolve);
}
