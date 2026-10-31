#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterMengHuoMouMobileRuntime))]
internal sealed partial class CharacterMengHuoMouMobileEntries
{
    private readonly Func<EngineOwners.CharacterMengHuoMouMobileRuntime> _resolve;
    private CharacterMengHuoMouMobileEntries(Func<EngineOwners.CharacterMengHuoMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterMengHuoMouMobileEntries Create(Func<EngineOwners.CharacterMengHuoMouMobileRuntime> resolve) => new(resolve);
}
