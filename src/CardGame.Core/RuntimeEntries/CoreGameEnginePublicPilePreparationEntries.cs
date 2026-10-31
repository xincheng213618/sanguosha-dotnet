#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePublicPilePreparationRuntime))]
internal sealed partial class CoreGameEnginePublicPilePreparationEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePublicPilePreparationRuntime> _resolve;
    private CoreGameEnginePublicPilePreparationEntries(Func<EngineOwners.CoreGameEnginePublicPilePreparationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePublicPilePreparationEntries Create(Func<EngineOwners.CoreGameEnginePublicPilePreparationRuntime> resolve) => new(resolve);
}
