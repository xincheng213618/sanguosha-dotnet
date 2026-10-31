#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterMangYaChangAnniversaryRuntime))]
internal sealed partial class CharacterMangYaChangAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMangYaChangAnniversaryRuntime> _resolve;
    private CharacterMangYaChangAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMangYaChangAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterMangYaChangAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMangYaChangAnniversaryRuntime> resolve) => new(resolve);
}
