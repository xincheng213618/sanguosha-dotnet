#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRecipientContestsRuntime))]
internal sealed partial class CoreGameEngineRecipientContestsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRecipientContestsRuntime> _resolve;
    private CoreGameEngineRecipientContestsEntries(Func<EngineOwners.CoreGameEngineRecipientContestsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRecipientContestsEntries Create(Func<EngineOwners.CoreGameEngineRecipientContestsRuntime> resolve) => new(resolve);
}
