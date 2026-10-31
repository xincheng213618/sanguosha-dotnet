#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLeiBoAnniversaryRuntime))]
internal sealed partial class CharacterLeiBoAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterLeiBoAnniversaryRuntime> _resolve;
    private CharacterLeiBoAnniversaryEntries(Func<EngineOwners.CharacterLeiBoAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLeiBoAnniversaryEntries Create(Func<EngineOwners.CharacterLeiBoAnniversaryRuntime> resolve) => new(resolve);
}
