#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineGuanduChariotResponsesRuntime))]
internal sealed partial class CoreGameEngineGuanduChariotResponsesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineGuanduChariotResponsesRuntime> _resolve;
    private CoreGameEngineGuanduChariotResponsesEntries(Func<EngineOwners.CoreGameEngineGuanduChariotResponsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGuanduChariotResponsesEntries Create(Func<EngineOwners.CoreGameEngineGuanduChariotResponsesRuntime> resolve) => new(resolve);
}
