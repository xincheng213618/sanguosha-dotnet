#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreDyingCursorRuntimeRuntime))]
internal sealed partial class CoreDyingCursorRuntimeEntries
{
    private readonly Func<EngineOwners.CoreDyingCursorRuntimeRuntime> _resolve;
    private CoreDyingCursorRuntimeEntries(Func<EngineOwners.CoreDyingCursorRuntimeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreDyingCursorRuntimeEntries Create(Func<EngineOwners.CoreDyingCursorRuntimeRuntime> resolve) => new(resolve);
}
