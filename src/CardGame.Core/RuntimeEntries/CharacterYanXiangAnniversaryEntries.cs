#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterYanXiangAnniversaryRuntime))]
internal sealed partial class CharacterYanXiangAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYanXiangAnniversaryRuntime> _resolve;
    private CharacterYanXiangAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYanXiangAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYanXiangAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYanXiangAnniversaryRuntime> resolve) => new(resolve);
}
