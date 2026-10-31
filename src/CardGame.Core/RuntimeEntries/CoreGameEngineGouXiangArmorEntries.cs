#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineGouXiangArmorRuntime))]
internal sealed partial class CoreGameEngineGouXiangArmorEntries
{
    private readonly Func<EngineOwners.CoreGameEngineGouXiangArmorRuntime> _resolve;
    private CoreGameEngineGouXiangArmorEntries(Func<EngineOwners.CoreGameEngineGouXiangArmorRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGouXiangArmorEntries Create(Func<EngineOwners.CoreGameEngineGouXiangArmorRuntime> resolve) => new(resolve);
}
