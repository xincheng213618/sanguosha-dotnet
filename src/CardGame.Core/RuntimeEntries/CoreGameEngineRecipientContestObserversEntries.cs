#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRecipientContestObserversRuntime))]
internal sealed partial class CoreGameEngineRecipientContestObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientContestObserversRuntime> _resolve;
    private CoreGameEngineRecipientContestObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientContestObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRecipientContestObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientContestObserversRuntime> resolve) => new(resolve);
}
