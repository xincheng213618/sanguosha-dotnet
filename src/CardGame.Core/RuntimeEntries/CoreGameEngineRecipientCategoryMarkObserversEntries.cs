#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategoryMarkObserversRuntime))]
internal sealed partial class CoreGameEngineRecipientCategoryMarkObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategoryMarkObserversRuntime> _resolve;
    private CoreGameEngineRecipientCategoryMarkObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategoryMarkObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRecipientCategoryMarkObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategoryMarkObserversRuntime> resolve) => new(resolve);
}
