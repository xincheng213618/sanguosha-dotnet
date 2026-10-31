#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativeSlashCompletionRuntime))]
internal sealed partial class CoreNativeSlashCompletionEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashCompletionRuntime> _resolve;
    private CoreNativeSlashCompletionEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashCompletionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeSlashCompletionEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashCompletionRuntime> resolve) => new(resolve);
}
