#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeWeaponResponsesRuntime))]
internal sealed partial class CoreNativeWeaponResponsesEntries
{
    private readonly Func<EngineOwners.CoreNativeWeaponResponsesRuntime> _resolve;
    private CoreNativeWeaponResponsesEntries(Func<EngineOwners.CoreNativeWeaponResponsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeWeaponResponsesEntries Create(Func<EngineOwners.CoreNativeWeaponResponsesRuntime> resolve) => new(resolve);
}
