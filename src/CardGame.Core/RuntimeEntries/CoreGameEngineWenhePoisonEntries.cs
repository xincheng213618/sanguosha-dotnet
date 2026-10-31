#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineWenhePoisonRuntime))]
internal sealed partial class CoreGameEngineWenhePoisonEntries
{
    private readonly Func<EngineOwners.CoreGameEngineWenhePoisonRuntime> _resolve;
    private CoreGameEngineWenhePoisonEntries(Func<EngineOwners.CoreGameEngineWenhePoisonRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWenhePoisonEntries Create(Func<EngineOwners.CoreGameEngineWenhePoisonRuntime> resolve) => new(resolve);
}
