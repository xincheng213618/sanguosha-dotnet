#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeCardAppearanceRuntime))]
internal sealed partial class CoreNativeCardAppearanceEntries
{
    private readonly Func<EngineOwners.CoreNativeCardAppearanceRuntime> _resolve;
    private CoreNativeCardAppearanceEntries(Func<EngineOwners.CoreNativeCardAppearanceRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeCardAppearanceEntries Create(Func<EngineOwners.CoreNativeCardAppearanceRuntime> resolve) => new(resolve);
}
