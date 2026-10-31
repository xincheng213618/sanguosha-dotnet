#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRecipientConsequencesReceiptsRuntime))]
internal sealed partial class CoreGameEngineRecipientConsequencesReceiptsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRecipientConsequencesReceiptsRuntime> _resolve;
    private CoreGameEngineRecipientConsequencesReceiptsEntries(Func<EngineOwners.CoreGameEngineRecipientConsequencesReceiptsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRecipientConsequencesReceiptsEntries Create(Func<EngineOwners.CoreGameEngineRecipientConsequencesReceiptsRuntime> resolve) => new(resolve);
}
