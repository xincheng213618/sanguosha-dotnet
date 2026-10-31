#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRecipientContestReceiptsRuntime))]
internal sealed partial class CoreGameEngineRecipientContestReceiptsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRecipientContestReceiptsRuntime> _resolve;
    private CoreGameEngineRecipientContestReceiptsEntries(Func<EngineOwners.CoreGameEngineRecipientContestReceiptsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRecipientContestReceiptsEntries Create(Func<EngineOwners.CoreGameEngineRecipientContestReceiptsRuntime> resolve) => new(resolve);
}
