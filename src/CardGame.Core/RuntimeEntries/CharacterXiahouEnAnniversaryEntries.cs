#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterXiahouEnAnniversaryRuntime))]
internal sealed partial class CharacterXiahouEnAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouEnAnniversaryRuntime> _resolve;
    private CharacterXiahouEnAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouEnAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXiahouEnAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouEnAnniversaryRuntime> resolve) => new(resolve);
}
