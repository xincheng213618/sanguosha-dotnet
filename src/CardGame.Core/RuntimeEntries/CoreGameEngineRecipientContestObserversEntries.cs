#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRecipientContestObserversRuntime))]
internal sealed partial class CoreGameEngineRecipientContestObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRecipientContestObserversRuntime> _resolve;
    private CoreGameEngineRecipientContestObserversEntries(Func<EngineOwners.CoreGameEngineRecipientContestObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRecipientContestObserversEntries Create(Func<EngineOwners.CoreGameEngineRecipientContestObserversRuntime> resolve) => new(resolve);
}
