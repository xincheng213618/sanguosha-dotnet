#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterDongBaiMobileRuntime))]
internal sealed partial class CharacterDongBaiMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongBaiMobileRuntime> _resolve;
    private CharacterDongBaiMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongBaiMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDongBaiMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongBaiMobileRuntime> resolve) => new(resolve);
}
