#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRecipientConsequencesReceiptsRuntime))]
internal sealed partial class CoreGameEngineRecipientConsequencesReceiptsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientConsequencesReceiptsRuntime> _resolve;
    private CoreGameEngineRecipientConsequencesReceiptsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientConsequencesReceiptsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRecipientConsequencesReceiptsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientConsequencesReceiptsRuntime> resolve) => new(resolve);
}
