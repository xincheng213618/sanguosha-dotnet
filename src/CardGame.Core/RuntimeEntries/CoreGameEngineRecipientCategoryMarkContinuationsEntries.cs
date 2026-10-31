#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRecipientCategoryMarkContinuationsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategoryMarkContinuationsRuntime> _resolve;
    private CoreGameEngineRecipientCategoryMarkContinuationsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategoryMarkContinuationsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRecipientCategoryMarkContinuationsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategoryMarkContinuationsRuntime> resolve) => new(resolve);
}
