#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterChengGongYingOlRuntime))]
internal sealed partial class CharacterChengGongYingOlEntries
{
    private readonly Func<EngineOwners.CharacterChengGongYingOlRuntime> _resolve;
    private CharacterChengGongYingOlEntries(Func<EngineOwners.CharacterChengGongYingOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterChengGongYingOlEntries Create(Func<EngineOwners.CharacterChengGongYingOlRuntime> resolve) => new(resolve);
}
