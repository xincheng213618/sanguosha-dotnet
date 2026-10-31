#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterChengYuMouRuntime))]
internal sealed partial class CharacterChengYuMouEntries
{
    private readonly Func<EngineOwners.CharacterChengYuMouRuntime> _resolve;
    private CharacterChengYuMouEntries(Func<EngineOwners.CharacterChengYuMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterChengYuMouEntries Create(Func<EngineOwners.CharacterChengYuMouRuntime> resolve) => new(resolve);
}
