#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterGuoHuaiMouMobileRuntime))]
internal sealed partial class CharacterGuoHuaiMouMobileEntries
{
    private readonly Func<EngineOwners.CharacterGuoHuaiMouMobileRuntime> _resolve;
    private CharacterGuoHuaiMouMobileEntries(Func<EngineOwners.CharacterGuoHuaiMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuoHuaiMouMobileEntries Create(Func<EngineOwners.CharacterGuoHuaiMouMobileRuntime> resolve) => new(resolve);
}
