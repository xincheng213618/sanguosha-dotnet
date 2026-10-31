#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangRuntime))]
internal sealed partial class CoreGameEngineWushuangEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangRuntime> _resolve;
    private CoreGameEngineWushuangEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangRuntime> resolve) => new(resolve);
}
