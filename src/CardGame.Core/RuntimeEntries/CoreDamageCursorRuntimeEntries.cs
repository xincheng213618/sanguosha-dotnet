#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime))]
internal sealed partial class CoreDamageCursorRuntimeEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime> _resolve;
    private CoreDamageCursorRuntimeEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreDamageCursorRuntimeEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime> resolve) => new(resolve);
}
