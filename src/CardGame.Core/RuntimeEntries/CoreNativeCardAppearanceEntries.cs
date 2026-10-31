#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime))]
internal sealed partial class CoreNativeCardAppearanceEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> _resolve;
    private CoreNativeCardAppearanceEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeCardAppearanceEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> resolve) => new(resolve);
}
