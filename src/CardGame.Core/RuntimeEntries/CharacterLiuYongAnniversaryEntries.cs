#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterLiuYongAnniversaryRuntime))]
internal sealed partial class CharacterLiuYongAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuYongAnniversaryRuntime> _resolve;
    private CharacterLiuYongAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuYongAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiuYongAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuYongAnniversaryRuntime> resolve) => new(resolve);
}
