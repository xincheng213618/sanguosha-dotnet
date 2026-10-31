#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterLiuYouAnniversaryRuntime))]
internal sealed partial class CharacterLiuYouAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuYouAnniversaryRuntime> _resolve;
    private CharacterLiuYouAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuYouAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiuYouAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuYouAnniversaryRuntime> resolve) => new(resolve);
}
