#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterLeJiuAnniversaryRuntime))]
internal sealed partial class CharacterLeJiuAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLeJiuAnniversaryRuntime> _resolve;
    private CharacterLeJiuAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLeJiuAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLeJiuAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLeJiuAnniversaryRuntime> resolve) => new(resolve);
}
