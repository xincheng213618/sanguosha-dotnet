#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCompletedUseChildReturnsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUseChildReturnsRuntime> _resolve;
    private CoreGameEngineCompletedUseChildReturnsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUseChildReturnsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCompletedUseChildReturnsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUseChildReturnsRuntime> resolve) => new(resolve);
}
