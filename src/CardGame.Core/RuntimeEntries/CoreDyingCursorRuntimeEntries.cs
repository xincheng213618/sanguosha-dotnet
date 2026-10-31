#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime))]
internal sealed partial class CoreDyingCursorRuntimeEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> _resolve;
    private CoreDyingCursorRuntimeEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreDyingCursorRuntimeEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> resolve) => new(resolve);
}
