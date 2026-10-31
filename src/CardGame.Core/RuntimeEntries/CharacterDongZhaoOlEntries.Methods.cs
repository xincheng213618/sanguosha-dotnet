#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDongZhaoOlEntries
{
    internal void ApplyZaowangSuccession(long deathId, global::CardGame.Core.CharacterState victim) => _resolve().ApplyZaowangSuccession(deathId, victim);
    internal global::CardGame.Core.SkillProgramStepOutcome ArmZaowang(global::CardGame.Core.ProgramSkillFrame input) => _resolve().ArmZaowang(input);
    internal void AssertRegisteredDongZhao(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertRegisteredDongZhao(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginXianlueRecord(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginXianlueRecord(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginXianlueRedistribute(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginXianlueRedistribute(f);
    internal bool CanContinueIssuedDongZhao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueIssuedDongZhao(f);
    internal bool CanOfferDongZhao(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().CanOfferDongZhao(c, t, w);
    internal bool DongZhaoFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().DongZhaoFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? DongZhaoObserverRoot() => _resolve().DongZhaoObserverRoot();
    internal bool HasDongZhaoDamageObserver(long id) => _resolve().HasDongZhaoDamageObserver(id);
    internal bool IsDongZhaoDying() => _resolve().IsDongZhaoDying();
    internal bool IsDongZhaoMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsDongZhaoMovement(f, e, p);
    internal void PublishXianluePrompt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishXianluePrompt(f);
    internal void RequireDongZhao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireDongZhao(f);
    internal void ResolveDongZhaoChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveDongZhaoChoice(choice);
    internal bool ResumeDongZhao(long id) => _resolve().ResumeDongZhao(id);
    internal bool TryAdvanceDongZhaoSubtree() => _resolve().TryAdvanceDongZhaoSubtree();
    internal bool ValidIssuedXianlue(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidIssuedXianlue(f);
    internal bool ValidIssuedZaowang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidIssuedZaowang(f);
    internal bool ZaowangFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().ZaowangFirstChild(root, child);
    internal bool ZaowangRebelDeathWins(global::CardGame.Core.CharacterState victim, global::CardGame.Core.CharacterState? killer) => _resolve().ZaowangRebelDeathWins(victim, killer);
}
