#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineMantianGuohaiRuntime))]
internal sealed partial class CoreGameEngineMantianGuohaiEntries
{
    private readonly Func<EngineOwners.CoreGameEngineMantianGuohaiRuntime> _resolve;
    private CoreGameEngineMantianGuohaiEntries(Func<EngineOwners.CoreGameEngineMantianGuohaiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineMantianGuohaiEntries Create(Func<EngineOwners.CoreGameEngineMantianGuohaiRuntime> resolve) => new(resolve);
}
