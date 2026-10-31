#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheS5XuncanRuntime))]
internal sealed partial class CoreGameEngineShanheS5XuncanEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheS5XuncanRuntime> _resolve;
    private CoreGameEngineShanheS5XuncanEntries(Func<EngineOwners.CoreGameEngineShanheS5XuncanRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheS5XuncanEntries Create(Func<EngineOwners.CoreGameEngineShanheS5XuncanRuntime> resolve) => new(resolve);
}
