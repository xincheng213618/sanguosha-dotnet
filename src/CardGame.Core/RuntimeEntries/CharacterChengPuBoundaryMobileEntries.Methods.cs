#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChengPuBoundaryMobileEntries
{
    internal bool AdvanceBoundaryChengPuHealth() => _resolve().AdvanceBoundaryChengPuHealth();
    internal SkillProgramStepOutcome BeginBoundaryChengPuLihuo(ProgramSkillFrame supplied) => _resolve().BeginBoundaryChengPuLihuo(supplied);
    internal CardUseFrame? BoundaryChengPuCompletedUse(ProgramSkillFrame f) => _resolve().BoundaryChengPuCompletedUse(f);
    internal int BoundaryChengPuDamageTotal(long useId) => _resolve().BoundaryChengPuDamageTotal(useId);
    internal bool BoundaryChengPuDying() => _resolve().BoundaryChengPuDying();
    internal bool BoundaryChengPuHealthChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().BoundaryChengPuHealthChild(f, child);
    internal ProgramSkillFrame? BoundaryChengPuHealthRoot() => _resolve().BoundaryChengPuHealthRoot();
    internal bool BoundaryChengPuHealthSuffix(int n) => _resolve().BoundaryChengPuHealthSuffix(n);
    internal int BoundaryChengPuLihuoDamage(GameEngine.IDamageAttempt attack) => _resolve().BoundaryChengPuLihuoDamage(attack);
    internal int BoundaryChengPuPublicActionBonus(int actorSeat, LegalAction action, int targetSeat) => _resolve().BoundaryChengPuPublicActionBonus(actorSeat, action, targetSeat);
    internal void ContinueBoundaryChengPuLihuo(long id) => _resolve().ContinueBoundaryChengPuLihuo(id);
    internal bool KeepBoundaryChengPuLihuo(ProgramSkillFrame f) => _resolve().KeepBoundaryChengPuLihuo(f);
    internal void RequireBoundaryChengPuLihuo(ProgramSkillFrame f) => _resolve().RequireBoundaryChengPuLihuo(f);
    internal bool ResumeBoundaryChengPuLihuo(long id) => _resolve().ResumeBoundaryChengPuLihuo(id);
}
