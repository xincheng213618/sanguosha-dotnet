#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRecipientCategoryMarkObserversRuntime))]
internal sealed partial class CoreGameEngineRecipientCategoryMarkObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRecipientCategoryMarkObserversRuntime> _resolve;
    private CoreGameEngineRecipientCategoryMarkObserversEntries(Func<EngineOwners.CoreGameEngineRecipientCategoryMarkObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRecipientCategoryMarkObserversEntries Create(Func<EngineOwners.CoreGameEngineRecipientCategoryMarkObserversRuntime> resolve) => new(resolve);
}
