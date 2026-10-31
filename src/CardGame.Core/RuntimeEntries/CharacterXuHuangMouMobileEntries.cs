#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterXuHuangMouMobileRuntime))]
internal sealed partial class CharacterXuHuangMouMobileEntries
{
    private readonly Func<EngineOwners.CharacterXuHuangMouMobileRuntime> _resolve;
    private CharacterXuHuangMouMobileEntries(Func<EngineOwners.CharacterXuHuangMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXuHuangMouMobileEntries Create(Func<EngineOwners.CharacterXuHuangMouMobileRuntime> resolve) => new(resolve);
}
