#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterCuiLieAnniversaryRuntime))]
internal sealed partial class CharacterCuiLieAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterCuiLieAnniversaryRuntime> _resolve;
    private CharacterCuiLieAnniversaryEntries(Func<EngineOwners.CharacterCuiLieAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCuiLieAnniversaryEntries Create(Func<EngineOwners.CharacterCuiLieAnniversaryRuntime> resolve) => new(resolve);
}
