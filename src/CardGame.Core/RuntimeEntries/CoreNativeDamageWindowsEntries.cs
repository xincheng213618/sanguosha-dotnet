#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeDamageWindowsRuntime))]
internal sealed partial class CoreNativeDamageWindowsEntries
{
    private readonly Func<EngineOwners.CoreNativeDamageWindowsRuntime> _resolve;
    private CoreNativeDamageWindowsEntries(Func<EngineOwners.CoreNativeDamageWindowsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeDamageWindowsEntries Create(Func<EngineOwners.CoreNativeDamageWindowsRuntime> resolve) => new(resolve);
}
