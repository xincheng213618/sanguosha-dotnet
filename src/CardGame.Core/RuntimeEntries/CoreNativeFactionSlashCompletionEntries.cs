#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeFactionSlashCompletionEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeFactionSlashCompletionRuntime> _resolve;
    private CoreNativeFactionSlashCompletionEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeFactionSlashCompletionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeFactionSlashCompletionEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeFactionSlashCompletionRuntime> resolve) => new(resolve);
}
