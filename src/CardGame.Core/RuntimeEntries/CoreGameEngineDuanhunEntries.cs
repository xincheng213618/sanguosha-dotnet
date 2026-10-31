#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDuanhunRuntime))]
internal sealed partial class CoreGameEngineDuanhunEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDuanhunRuntime> _resolve;
    private CoreGameEngineDuanhunEntries(Func<EngineOwners.CoreGameEngineDuanhunRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDuanhunEntries Create(Func<EngineOwners.CoreGameEngineDuanhunRuntime> resolve) => new(resolve);
}
