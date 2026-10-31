#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePindianHandReturnsRuntime))]
internal sealed partial class CoreGameEnginePindianHandReturnsEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePindianHandReturnsRuntime> _resolve;
    private CoreGameEnginePindianHandReturnsEntries(Func<EngineOwners.CoreGameEnginePindianHandReturnsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePindianHandReturnsEntries Create(Func<EngineOwners.CoreGameEnginePindianHandReturnsRuntime> resolve) => new(resolve);
}
