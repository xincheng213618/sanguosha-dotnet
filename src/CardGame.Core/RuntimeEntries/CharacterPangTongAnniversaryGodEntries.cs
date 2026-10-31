#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterPangTongAnniversaryGodRuntime))]
internal sealed partial class CharacterPangTongAnniversaryGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangTongAnniversaryGodRuntime> _resolve;
    private CharacterPangTongAnniversaryGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangTongAnniversaryGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterPangTongAnniversaryGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangTongAnniversaryGodRuntime> resolve) => new(resolve);
}
