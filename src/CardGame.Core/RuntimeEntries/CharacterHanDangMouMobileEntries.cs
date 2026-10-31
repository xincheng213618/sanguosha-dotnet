#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterHanDangMouMobileRuntime))]
internal sealed partial class CharacterHanDangMouMobileEntries
{
    private readonly Func<EngineOwners.CharacterHanDangMouMobileRuntime> _resolve;
    private CharacterHanDangMouMobileEntries(Func<EngineOwners.CharacterHanDangMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHanDangMouMobileEntries Create(Func<EngineOwners.CharacterHanDangMouMobileRuntime> resolve) => new(resolve);
}
