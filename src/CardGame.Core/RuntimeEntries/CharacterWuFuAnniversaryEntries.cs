#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterWuFuAnniversaryRuntime))]
internal sealed partial class CharacterWuFuAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuFuAnniversaryRuntime> _resolve;
    private CharacterWuFuAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuFuAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWuFuAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuFuAnniversaryRuntime> resolve) => new(resolve);
}
