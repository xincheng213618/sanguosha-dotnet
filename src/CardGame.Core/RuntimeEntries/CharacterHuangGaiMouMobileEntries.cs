#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterHuangGaiMouMobileRuntime))]
internal sealed partial class CharacterHuangGaiMouMobileEntries
{
    private readonly Func<EngineOwners.CharacterHuangGaiMouMobileRuntime> _resolve;
    private CharacterHuangGaiMouMobileEntries(Func<EngineOwners.CharacterHuangGaiMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuangGaiMouMobileEntries Create(Func<EngineOwners.CharacterHuangGaiMouMobileRuntime> resolve) => new(resolve);
}
