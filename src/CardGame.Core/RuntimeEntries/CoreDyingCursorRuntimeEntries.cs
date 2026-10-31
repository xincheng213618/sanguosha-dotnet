#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreDyingCursorRuntimeEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> _resolve;
    private CoreDyingCursorRuntimeEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreDyingCursorRuntimeEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> resolve) => new(resolve);
}
