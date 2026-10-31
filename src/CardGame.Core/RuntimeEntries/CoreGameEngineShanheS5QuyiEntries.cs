#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheS5QuyiRuntime))]
internal sealed partial class CoreGameEngineShanheS5QuyiEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheS5QuyiRuntime> _resolve;
    private CoreGameEngineShanheS5QuyiEntries(Func<EngineOwners.CoreGameEngineShanheS5QuyiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheS5QuyiEntries Create(Func<EngineOwners.CoreGameEngineShanheS5QuyiRuntime> resolve) => new(resolve);
}
