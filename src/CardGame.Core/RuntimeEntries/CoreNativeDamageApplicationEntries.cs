#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeDamageApplicationRuntime))]
internal sealed partial class CoreNativeDamageApplicationEntries
{
    private readonly Func<EngineOwners.CoreNativeDamageApplicationRuntime> _resolve;
    private CoreNativeDamageApplicationEntries(Func<EngineOwners.CoreNativeDamageApplicationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeDamageApplicationEntries Create(Func<EngineOwners.CoreNativeDamageApplicationRuntime> resolve) => new(resolve);
}
