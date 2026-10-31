#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGouXiangArmorEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGouXiangArmorRuntime> _resolve;
    private CoreGameEngineGouXiangArmorEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGouXiangArmorRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGouXiangArmorEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGouXiangArmorRuntime> resolve) => new(resolve);
}
