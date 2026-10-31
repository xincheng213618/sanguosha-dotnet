#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5QuyiRuntime))]
internal sealed partial class CoreGameEngineShanheS5QuyiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5QuyiRuntime> _resolve;
    private CoreGameEngineShanheS5QuyiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5QuyiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheS5QuyiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5QuyiRuntime> resolve) => new(resolve);
}
