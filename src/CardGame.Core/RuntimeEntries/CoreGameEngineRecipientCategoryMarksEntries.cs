#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRecipientCategoryMarksRuntime))]
internal sealed partial class CoreGameEngineRecipientCategoryMarksEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRecipientCategoryMarksRuntime> _resolve;
    private CoreGameEngineRecipientCategoryMarksEntries(Func<EngineOwners.CoreGameEngineRecipientCategoryMarksRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRecipientCategoryMarksEntries Create(Func<EngineOwners.CoreGameEngineRecipientCategoryMarksRuntime> resolve) => new(resolve);
}
