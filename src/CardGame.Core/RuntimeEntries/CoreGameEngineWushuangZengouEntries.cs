#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangZengouEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangZengouRuntime> _resolve;
    private CoreGameEngineWushuangZengouEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangZengouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangZengouEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangZengouRuntime> resolve) => new(resolve);
}
