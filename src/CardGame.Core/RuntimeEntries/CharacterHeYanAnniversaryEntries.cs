#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHeYanAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeYanAnniversaryRuntime> _resolve;
    private CharacterHeYanAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeYanAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHeYanAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeYanAnniversaryRuntime> resolve) => new(resolve);
}
