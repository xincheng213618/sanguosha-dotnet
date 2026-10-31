#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterDongBaiMobileRuntime))]
internal sealed partial class CharacterDongBaiMobileEntries
{
    private readonly Func<EngineOwners.CharacterDongBaiMobileRuntime> _resolve;
    private CharacterDongBaiMobileEntries(Func<EngineOwners.CharacterDongBaiMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDongBaiMobileEntries Create(Func<EngineOwners.CharacterDongBaiMobileRuntime> resolve) => new(resolve);
}
