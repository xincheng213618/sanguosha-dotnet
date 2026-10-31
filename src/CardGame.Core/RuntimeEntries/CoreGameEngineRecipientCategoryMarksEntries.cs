#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRecipientCategoryMarksEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategoryMarksRuntime> _resolve;
    private CoreGameEngineRecipientCategoryMarksEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategoryMarksRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRecipientCategoryMarksEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategoryMarksRuntime> resolve) => new(resolve);
}
