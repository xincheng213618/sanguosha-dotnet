#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXuShengMouEntries
{
    internal bool MouXuShengAdvanceChildren() => _resolve().MouXuShengAdvanceChildren();
    internal int MouXuShengDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().MouXuShengDamageBonus(attack);
    internal bool MouXuShengDying() => _resolve().MouXuShengDying();
    internal bool MouXuShengFirstNativeChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouXuShengFirstNativeChild(f, child);
    internal bool MouXuShengMovementReturn(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().MouXuShengMovementReturn(f, effect, p);
    internal bool MouXuShengNativeEdge(int i) => _resolve().MouXuShengNativeEdge(i);
    internal global::CardGame.Core.ProgramSkillFrame? MouXuShengNativeRoot() => _resolve().MouXuShengNativeRoot();
    internal bool MouXuShengNativeSuffix(int n) => _resolve().MouXuShengNativeSuffix(n);
    internal global::CardGame.Core.CardSnapshot[] MouXuShengPrivateHandViews(int viewer) => _resolve().MouXuShengPrivateHandViews(viewer);
    internal bool MouXuShengRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().MouXuShengRecoveryParent(f, recovery);
    internal global::CardGame.Core.MouXuShengRuntime MouXuSheng { get => _resolve().MouXuSheng; }
}
