#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterJuShouMouRuntime))]
internal sealed partial class CharacterJuShouMouEntries
{
    private readonly Func<EngineOwners.CharacterJuShouMouRuntime> _resolve;
    private CharacterJuShouMouEntries(Func<EngineOwners.CharacterJuShouMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterJuShouMouEntries Create(Func<EngineOwners.CharacterJuShouMouRuntime> resolve) => new(resolve);
}
