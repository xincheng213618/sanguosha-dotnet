#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterFaZhengMouMobileRuntime))]
internal sealed partial class CharacterFaZhengMouMobileEntries
{
    private readonly Func<EngineOwners.CharacterFaZhengMouMobileRuntime> _resolve;
    private CharacterFaZhengMouMobileEntries(Func<EngineOwners.CharacterFaZhengMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterFaZhengMouMobileEntries Create(Func<EngineOwners.CharacterFaZhengMouMobileRuntime> resolve) => new(resolve);
}
