#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterHeYanAnniversaryRuntime))]
internal sealed partial class CharacterHeYanAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeYanAnniversaryRuntime> _resolve;
    private CharacterHeYanAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeYanAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHeYanAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeYanAnniversaryRuntime> resolve) => new(resolve);
}
