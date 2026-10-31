#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardImmediateDrawRuntime))]
internal sealed partial class CoreGameEnginePrepDiscardImmediateDrawEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardImmediateDrawRuntime> _resolve;
    private CoreGameEnginePrepDiscardImmediateDrawEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardImmediateDrawRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePrepDiscardImmediateDrawEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardImmediateDrawRuntime> resolve) => new(resolve);
}
