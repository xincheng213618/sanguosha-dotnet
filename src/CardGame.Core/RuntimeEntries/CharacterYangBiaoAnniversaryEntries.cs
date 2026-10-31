#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterYangBiaoAnniversaryRuntime))]
internal sealed partial class CharacterYangBiaoAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterYangBiaoAnniversaryRuntime> _resolve;
    private CharacterYangBiaoAnniversaryEntries(Func<EngineOwners.CharacterYangBiaoAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYangBiaoAnniversaryEntries Create(Func<EngineOwners.CharacterYangBiaoAnniversaryRuntime> resolve) => new(resolve);
}
