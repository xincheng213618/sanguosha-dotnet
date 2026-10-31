#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterCaiYangAnniversaryRuntime))]
internal sealed partial class CharacterCaiYangAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiYangAnniversaryRuntime> _resolve;
    private CharacterCaiYangAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiYangAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaiYangAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiYangAnniversaryRuntime> resolve) => new(resolve);
}
