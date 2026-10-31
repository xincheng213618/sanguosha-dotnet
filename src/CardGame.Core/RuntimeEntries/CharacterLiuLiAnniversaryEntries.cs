#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterLiuLiAnniversaryRuntime))]
internal sealed partial class CharacterLiuLiAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuLiAnniversaryRuntime> _resolve;
    private CharacterLiuLiAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuLiAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiuLiAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuLiAnniversaryRuntime> resolve) => new(resolve);
}
