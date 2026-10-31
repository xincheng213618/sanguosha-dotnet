#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXuShengMouEntries
{
    internal bool MouXuShengAdvanceChildren() => _resolve().MouXuShengAdvanceChildren();
    internal int MouXuShengDamageBonus(GameEngine.IDamageAttempt attack) => _resolve().MouXuShengDamageBonus(attack);
    internal bool MouXuShengDying() => _resolve().MouXuShengDying();
    internal bool MouXuShengFirstNativeChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouXuShengFirstNativeChild(f, child);
    internal bool MouXuShengMovementReturn(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => _resolve().MouXuShengMovementReturn(f, effect, p);
    internal bool MouXuShengNativeEdge(int i) => _resolve().MouXuShengNativeEdge(i);
    internal ProgramSkillFrame? MouXuShengNativeRoot() => _resolve().MouXuShengNativeRoot();
    internal bool MouXuShengNativeSuffix(int n) => _resolve().MouXuShengNativeSuffix(n);
    internal CardSnapshot[] MouXuShengPrivateHandViews(int viewer) => _resolve().MouXuShengPrivateHandViews(viewer);
    internal bool MouXuShengRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().MouXuShengRecoveryParent(f, recovery);
    internal MouXuShengRuntime MouXuSheng { get => _resolve().MouXuSheng; }
}
