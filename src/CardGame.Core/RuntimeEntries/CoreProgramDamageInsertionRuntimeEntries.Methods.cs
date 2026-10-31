#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramDamageInsertionRuntimeEntries
{
    internal bool IsOriginalDyingSuspendedByProgramInsertion(DyingFrame dying) => _resolve().IsOriginalDyingSuspendedByProgramInsertion(dying);
    internal ProgramDamageInsertionRuntime DamageInsertions { get => _resolve().DamageInsertions; }
}
