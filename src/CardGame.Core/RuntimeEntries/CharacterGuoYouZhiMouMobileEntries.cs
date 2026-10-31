#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterGuoYouZhiMouMobileRuntime))]
internal sealed partial class CharacterGuoYouZhiMouMobileEntries
{
    private readonly Func<EngineOwners.CharacterGuoYouZhiMouMobileRuntime> _resolve;
    private CharacterGuoYouZhiMouMobileEntries(Func<EngineOwners.CharacterGuoYouZhiMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuoYouZhiMouMobileEntries Create(Func<EngineOwners.CharacterGuoYouZhiMouMobileRuntime> resolve) => new(resolve);
}
