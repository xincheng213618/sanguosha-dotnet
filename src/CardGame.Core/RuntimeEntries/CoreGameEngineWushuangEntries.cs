#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineWushuangRuntime))]
internal sealed partial class CoreGameEngineWushuangEntries
{
    private readonly Func<EngineOwners.CoreGameEngineWushuangRuntime> _resolve;
    private CoreGameEngineWushuangEntries(Func<EngineOwners.CoreGameEngineWushuangRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangEntries Create(Func<EngineOwners.CoreGameEngineWushuangRuntime> resolve) => new(resolve);
}
