#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeSlashCompletionRuntime))]
internal sealed partial class CoreNativeSlashCompletionEntries
{
    private readonly Func<EngineOwners.CoreNativeSlashCompletionRuntime> _resolve;
    private CoreNativeSlashCompletionEntries(Func<EngineOwners.CoreNativeSlashCompletionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeSlashCompletionEntries Create(Func<EngineOwners.CoreNativeSlashCompletionRuntime> resolve) => new(resolve);
}
