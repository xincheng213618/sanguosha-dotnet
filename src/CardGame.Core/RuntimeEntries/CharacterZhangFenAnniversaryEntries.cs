#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterZhangFenAnniversaryRuntime))]
internal sealed partial class CharacterZhangFenAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFenAnniversaryRuntime> _resolve;
    private CharacterZhangFenAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFenAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangFenAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFenAnniversaryRuntime> resolve) => new(resolve);
}
