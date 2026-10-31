#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativeFactionSlashCompletionRuntime))]
internal sealed partial class CoreNativeFactionSlashCompletionEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeFactionSlashCompletionRuntime> _resolve;
    private CoreNativeFactionSlashCompletionEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeFactionSlashCompletionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeFactionSlashCompletionEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeFactionSlashCompletionRuntime> resolve) => new(resolve);
}
