#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterDongChengMouRuntime))]
internal sealed partial class CharacterDongChengMouEntries
{
    private readonly Func<EngineOwners.CharacterDongChengMouRuntime> _resolve;
    private CharacterDongChengMouEntries(Func<EngineOwners.CharacterDongChengMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDongChengMouEntries Create(Func<EngineOwners.CharacterDongChengMouRuntime> resolve) => new(resolve);
}
