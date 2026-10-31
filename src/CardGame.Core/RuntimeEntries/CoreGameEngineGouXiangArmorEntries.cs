#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGouXiangArmorRuntime))]
internal sealed partial class CoreGameEngineGouXiangArmorEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGouXiangArmorRuntime> _resolve;
    private CoreGameEngineGouXiangArmorEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGouXiangArmorRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGouXiangArmorEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGouXiangArmorRuntime> resolve) => new(resolve);
}
