#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheYongzhanRuntime))]
internal sealed partial class CoreGameEngineShanheYongzhanEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheYongzhanRuntime> _resolve;
    private CoreGameEngineShanheYongzhanEntries(Func<EngineOwners.CoreGameEngineShanheYongzhanRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheYongzhanEntries Create(Func<EngineOwners.CoreGameEngineShanheYongzhanRuntime> resolve) => new(resolve);
}
