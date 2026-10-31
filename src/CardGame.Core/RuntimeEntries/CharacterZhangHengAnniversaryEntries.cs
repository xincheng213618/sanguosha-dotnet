#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterZhangHengAnniversaryRuntime))]
internal sealed partial class CharacterZhangHengAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangHengAnniversaryRuntime> _resolve;
    private CharacterZhangHengAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangHengAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangHengAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangHengAnniversaryRuntime> resolve) => new(resolve);
}
