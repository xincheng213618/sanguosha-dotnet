#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRecipientCategoryMarkContinuationsRuntime))]
internal sealed partial class CoreGameEngineRecipientCategoryMarkContinuationsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRecipientCategoryMarkContinuationsRuntime> _resolve;
    private CoreGameEngineRecipientCategoryMarkContinuationsEntries(Func<EngineOwners.CoreGameEngineRecipientCategoryMarkContinuationsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRecipientCategoryMarkContinuationsEntries Create(Func<EngineOwners.CoreGameEngineRecipientCategoryMarkContinuationsRuntime> resolve) => new(resolve);
}
