#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterXunXuAnniversaryRuntime))]
internal sealed partial class CharacterXunXuAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXunXuAnniversaryRuntime> _resolve;
    private CharacterXunXuAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXunXuAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXunXuAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXunXuAnniversaryRuntime> resolve) => new(resolve);
}
