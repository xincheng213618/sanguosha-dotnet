#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeFactionSlashCompletionRuntime))]
internal sealed partial class CoreNativeFactionSlashCompletionEntries
{
    private readonly Func<EngineOwners.CoreNativeFactionSlashCompletionRuntime> _resolve;
    private CoreNativeFactionSlashCompletionEntries(Func<EngineOwners.CoreNativeFactionSlashCompletionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeFactionSlashCompletionEntries Create(Func<EngineOwners.CoreNativeFactionSlashCompletionRuntime> resolve) => new(resolve);
}
