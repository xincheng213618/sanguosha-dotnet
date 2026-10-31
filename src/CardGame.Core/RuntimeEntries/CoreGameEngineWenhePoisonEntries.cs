#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWenhePoisonRuntime))]
internal sealed partial class CoreGameEngineWenhePoisonEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenhePoisonRuntime> _resolve;
    private CoreGameEngineWenhePoisonEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenhePoisonRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWenhePoisonEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenhePoisonRuntime> resolve) => new(resolve);
}
