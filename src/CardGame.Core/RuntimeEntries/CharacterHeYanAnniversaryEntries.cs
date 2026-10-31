#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterHeYanAnniversaryRuntime))]
internal sealed partial class CharacterHeYanAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterHeYanAnniversaryRuntime> _resolve;
    private CharacterHeYanAnniversaryEntries(Func<EngineOwners.CharacterHeYanAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHeYanAnniversaryEntries Create(Func<EngineOwners.CharacterHeYanAnniversaryRuntime> resolve) => new(resolve);
}
