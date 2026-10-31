#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineLiejieSourceDiscardsRuntime))]
internal sealed partial class CoreGameEngineLiejieSourceDiscardsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineLiejieSourceDiscardsRuntime> _resolve;
    private CoreGameEngineLiejieSourceDiscardsEntries(Func<EngineOwners.CoreGameEngineLiejieSourceDiscardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineLiejieSourceDiscardsEntries Create(Func<EngineOwners.CoreGameEngineLiejieSourceDiscardsRuntime> resolve) => new(resolve);
}
