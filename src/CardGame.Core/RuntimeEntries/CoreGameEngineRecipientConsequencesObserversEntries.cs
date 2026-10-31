#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRecipientConsequencesObserversRuntime))]
internal sealed partial class CoreGameEngineRecipientConsequencesObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientConsequencesObserversRuntime> _resolve;
    private CoreGameEngineRecipientConsequencesObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientConsequencesObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRecipientConsequencesObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientConsequencesObserversRuntime> resolve) => new(resolve);
}
