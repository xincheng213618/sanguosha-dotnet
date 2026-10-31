#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRecipientContestReceiptsRuntime))]
internal sealed partial class CoreGameEngineRecipientContestReceiptsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientContestReceiptsRuntime> _resolve;
    private CoreGameEngineRecipientContestReceiptsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientContestReceiptsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRecipientContestReceiptsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientContestReceiptsRuntime> resolve) => new(resolve);
}
