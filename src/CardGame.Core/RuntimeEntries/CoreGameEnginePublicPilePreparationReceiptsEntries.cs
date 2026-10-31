#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePublicPilePreparationReceiptsRuntime))]
internal sealed partial class CoreGameEnginePublicPilePreparationReceiptsEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePublicPilePreparationReceiptsRuntime> _resolve;
    private CoreGameEnginePublicPilePreparationReceiptsEntries(Func<EngineOwners.CoreGameEnginePublicPilePreparationReceiptsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePublicPilePreparationReceiptsEntries Create(Func<EngineOwners.CoreGameEnginePublicPilePreparationReceiptsRuntime> resolve) => new(resolve);
}
