#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramOwnerDeathDamageEntries
{
    internal bool IsOriginalDyingSuspendedByOwnerDeathDamage(DyingFrame dying) => _resolve().IsOriginalDyingSuspendedByOwnerDeathDamage(dying);
    internal bool IsOwnerDeathProgramDamage(ProgramSkillFrame frame) => _resolve().IsOwnerDeathProgramDamage(frame);
    internal bool IsOwnerDeathSuspendedDying(long programFrameId) => _resolve().IsOwnerDeathSuspendedDying(programFrameId);
}
