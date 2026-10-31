#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterMaMiaoAnniversaryRuntime))]
internal sealed partial class CharacterMaMiaoAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaMiaoAnniversaryRuntime> _resolve;
    private CharacterMaMiaoAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaMiaoAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterMaMiaoAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaMiaoAnniversaryRuntime> resolve) => new(resolve);
}
