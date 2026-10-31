#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterZhuZhiAnniversaryRuntime))]
internal sealed partial class CharacterZhuZhiAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuZhiAnniversaryRuntime> _resolve;
    private CharacterZhuZhiAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuZhiAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhuZhiAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuZhiAnniversaryRuntime> resolve) => new(resolve);
}
