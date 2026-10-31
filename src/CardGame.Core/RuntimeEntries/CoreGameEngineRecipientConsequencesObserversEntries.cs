#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRecipientConsequencesObserversRuntime))]
internal sealed partial class CoreGameEngineRecipientConsequencesObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRecipientConsequencesObserversRuntime> _resolve;
    private CoreGameEngineRecipientConsequencesObserversEntries(Func<EngineOwners.CoreGameEngineRecipientConsequencesObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRecipientConsequencesObserversEntries Create(Func<EngineOwners.CoreGameEngineRecipientConsequencesObserversRuntime> resolve) => new(resolve);
}
