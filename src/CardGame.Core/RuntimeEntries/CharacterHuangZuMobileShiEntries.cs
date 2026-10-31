#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterHuangZuMobileShiRuntime))]
internal sealed partial class CharacterHuangZuMobileShiEntries
{
    private readonly Func<EngineOwners.CharacterHuangZuMobileShiRuntime> _resolve;
    private CharacterHuangZuMobileShiEntries(Func<EngineOwners.CharacterHuangZuMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuangZuMobileShiEntries Create(Func<EngineOwners.CharacterHuangZuMobileShiRuntime> resolve) => new(resolve);
}
