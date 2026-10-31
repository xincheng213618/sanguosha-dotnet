#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunHuanAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunHuanAnniversaryRuntime> _resolve;
    private CharacterSunHuanAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunHuanAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunHuanAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunHuanAnniversaryRuntime> resolve) => new(resolve);
}
