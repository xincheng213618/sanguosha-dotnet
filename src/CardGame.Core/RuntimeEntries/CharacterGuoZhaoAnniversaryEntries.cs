#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterGuoZhaoAnniversaryRuntime))]
internal sealed partial class CharacterGuoZhaoAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoZhaoAnniversaryRuntime> _resolve;
    private CharacterGuoZhaoAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoZhaoAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuoZhaoAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoZhaoAnniversaryRuntime> resolve) => new(resolve);
}
