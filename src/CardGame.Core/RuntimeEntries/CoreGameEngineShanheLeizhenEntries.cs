#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheLeizhenRuntime))]
internal sealed partial class CoreGameEngineShanheLeizhenEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheLeizhenRuntime> _resolve;
    private CoreGameEngineShanheLeizhenEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheLeizhenRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheLeizhenEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheLeizhenRuntime> resolve) => new(resolve);
}
