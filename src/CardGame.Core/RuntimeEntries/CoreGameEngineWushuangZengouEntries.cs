#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineWushuangZengouRuntime))]
internal sealed partial class CoreGameEngineWushuangZengouEntries
{
    private readonly Func<EngineOwners.CoreGameEngineWushuangZengouRuntime> _resolve;
    private CoreGameEngineWushuangZengouEntries(Func<EngineOwners.CoreGameEngineWushuangZengouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangZengouEntries Create(Func<EngineOwners.CoreGameEngineWushuangZengouRuntime> resolve) => new(resolve);
}
