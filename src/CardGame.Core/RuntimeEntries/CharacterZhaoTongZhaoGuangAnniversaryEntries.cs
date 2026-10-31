#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterZhaoTongZhaoGuangAnniversaryRuntime))]
internal sealed partial class CharacterZhaoTongZhaoGuangAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoTongZhaoGuangAnniversaryRuntime> _resolve;
    private CharacterZhaoTongZhaoGuangAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoTongZhaoGuangAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhaoTongZhaoGuangAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoTongZhaoGuangAnniversaryRuntime> resolve) => new(resolve);
}
