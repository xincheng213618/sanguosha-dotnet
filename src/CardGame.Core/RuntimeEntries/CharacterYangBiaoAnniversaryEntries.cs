#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYangBiaoAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangBiaoAnniversaryRuntime> _resolve;
    private CharacterYangBiaoAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangBiaoAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYangBiaoAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangBiaoAnniversaryRuntime> resolve) => new(resolve);
}
