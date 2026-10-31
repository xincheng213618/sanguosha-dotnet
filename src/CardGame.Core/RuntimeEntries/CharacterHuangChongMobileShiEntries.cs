#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterHuangChongMobileShiRuntime))]
internal sealed partial class CharacterHuangChongMobileShiEntries
{
    private readonly Func<EngineOwners.CharacterHuangChongMobileShiRuntime> _resolve;
    private CharacterHuangChongMobileShiEntries(Func<EngineOwners.CharacterHuangChongMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuangChongMobileShiEntries Create(Func<EngineOwners.CharacterHuangChongMobileShiRuntime> resolve) => new(resolve);
}
