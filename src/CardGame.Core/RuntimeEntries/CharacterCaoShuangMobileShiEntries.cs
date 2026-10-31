#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoShuangMobileShiEntries
{
    private readonly Func<EngineOwners.CharacterCaoShuangMobileShiRuntime> _resolve;
    private CharacterCaoShuangMobileShiEntries(Func<EngineOwners.CharacterCaoShuangMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoShuangMobileShiEntries Create(Func<EngineOwners.CharacterCaoShuangMobileShiRuntime> resolve) => new(resolve);
}
