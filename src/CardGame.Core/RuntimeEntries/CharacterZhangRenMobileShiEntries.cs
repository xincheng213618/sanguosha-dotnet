#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhangRenMobileShiRuntime))]
internal sealed partial class CharacterZhangRenMobileShiEntries
{
    private readonly Func<EngineOwners.CharacterZhangRenMobileShiRuntime> _resolve;
    private CharacterZhangRenMobileShiEntries(Func<EngineOwners.CharacterZhangRenMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangRenMobileShiEntries Create(Func<EngineOwners.CharacterZhangRenMobileShiRuntime> resolve) => new(resolve);
}
