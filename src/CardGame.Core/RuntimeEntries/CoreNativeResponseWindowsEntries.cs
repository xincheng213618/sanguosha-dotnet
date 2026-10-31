#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeResponseWindowsRuntime))]
internal sealed partial class CoreNativeResponseWindowsEntries
{
    private readonly Func<EngineOwners.CoreNativeResponseWindowsRuntime> _resolve;
    private CoreNativeResponseWindowsEntries(Func<EngineOwners.CoreNativeResponseWindowsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeResponseWindowsEntries Create(Func<EngineOwners.CoreNativeResponseWindowsRuntime> resolve) => new(resolve);
}
