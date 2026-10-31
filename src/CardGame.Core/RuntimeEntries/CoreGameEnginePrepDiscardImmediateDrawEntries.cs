#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePrepDiscardImmediateDrawEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardImmediateDrawRuntime> _resolve;
    private CoreGameEnginePrepDiscardImmediateDrawEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardImmediateDrawRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePrepDiscardImmediateDrawEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardImmediateDrawRuntime> resolve) => new(resolve);
}
