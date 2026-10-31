#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePrepDiscardImmediateDrawRuntime))]
internal sealed partial class CoreGameEnginePrepDiscardImmediateDrawEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePrepDiscardImmediateDrawRuntime> _resolve;
    private CoreGameEnginePrepDiscardImmediateDrawEntries(Func<EngineOwners.CoreGameEnginePrepDiscardImmediateDrawRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePrepDiscardImmediateDrawEntries Create(Func<EngineOwners.CoreGameEnginePrepDiscardImmediateDrawRuntime> resolve) => new(resolve);
}
