#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreDamageCursorRuntimeRuntime))]
internal sealed partial class CoreDamageCursorRuntimeEntries
{
    private readonly Func<EngineOwners.CoreDamageCursorRuntimeRuntime> _resolve;
    private CoreDamageCursorRuntimeEntries(Func<EngineOwners.CoreDamageCursorRuntimeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreDamageCursorRuntimeEntries Create(Func<EngineOwners.CoreDamageCursorRuntimeRuntime> resolve) => new(resolve);
}
