#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGaoXiangMobileShiEntries
{
    private readonly Func<EngineOwners.CharacterGaoXiangMobileShiRuntime> _resolve;
    private CharacterGaoXiangMobileShiEntries(Func<EngineOwners.CharacterGaoXiangMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGaoXiangMobileShiEntries Create(Func<EngineOwners.CharacterGaoXiangMobileShiRuntime> resolve) => new(resolve);
}
