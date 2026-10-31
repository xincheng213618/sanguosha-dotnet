#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChengPuBoundaryMobileEntries
{
    internal bool AdvanceBoundaryChengPuHealth() => _resolve().AdvanceBoundaryChengPuHealth();
    internal global::CardGame.Core.SkillProgramStepOutcome BeginBoundaryChengPuLihuo(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginBoundaryChengPuLihuo(supplied);
    internal global::CardGame.Core.CardUseFrame? BoundaryChengPuCompletedUse(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BoundaryChengPuCompletedUse(f);
    internal int BoundaryChengPuDamageTotal(long useId) => _resolve().BoundaryChengPuDamageTotal(useId);
    internal bool BoundaryChengPuDying() => _resolve().BoundaryChengPuDying();
    internal bool BoundaryChengPuHealthChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().BoundaryChengPuHealthChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? BoundaryChengPuHealthRoot() => _resolve().BoundaryChengPuHealthRoot();
    internal bool BoundaryChengPuHealthSuffix(int n) => _resolve().BoundaryChengPuHealthSuffix(n);
    internal int BoundaryChengPuLihuoDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().BoundaryChengPuLihuoDamage(attack);
    internal int BoundaryChengPuPublicActionBonus(int actorSeat, global::CardGame.Core.LegalAction action, int targetSeat) => _resolve().BoundaryChengPuPublicActionBonus(actorSeat, action, targetSeat);
    internal void ContinueBoundaryChengPuLihuo(long id) => _resolve().ContinueBoundaryChengPuLihuo(id);
    internal bool KeepBoundaryChengPuLihuo(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepBoundaryChengPuLihuo(f);
    internal void RequireBoundaryChengPuLihuo(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireBoundaryChengPuLihuo(f);
    internal bool ResumeBoundaryChengPuLihuo(long id) => _resolve().ResumeBoundaryChengPuLihuo(id);
}
