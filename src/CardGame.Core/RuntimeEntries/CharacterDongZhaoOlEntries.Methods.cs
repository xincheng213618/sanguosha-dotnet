#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDongZhaoOlEntries
{
    internal void ApplyZaowangSuccession(long deathId, CharacterState victim) => _resolve().ApplyZaowangSuccession(deathId, victim);
    internal SkillProgramStepOutcome ArmZaowang(ProgramSkillFrame input) => _resolve().ArmZaowang(input);
    internal SkillProgramStepOutcome BeginXianlueRecord(ProgramSkillFrame f) => _resolve().BeginXianlueRecord(f);
    internal SkillProgramStepOutcome BeginXianlueRedistribute(ProgramSkillFrame f) => _resolve().BeginXianlueRedistribute(f);
    internal bool CanContinueIssuedDongZhao(ProgramSkillFrame f) => _resolve().CanContinueIssuedDongZhao(f);
    internal bool CanOfferDongZhao(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => _resolve().CanOfferDongZhao(c, t, w);
    internal bool DongZhaoFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().DongZhaoFirstChild(root, child);
    internal ProgramSkillFrame? DongZhaoObserverRoot() => _resolve().DongZhaoObserverRoot();
    internal void PublishXianluePrompt(ProgramSkillFrame f) => _resolve().PublishXianluePrompt(f);
    internal void ResolveDongZhaoChoice(PromptChoice choice) => _resolve().ResolveDongZhaoChoice(choice);
    internal bool ResumeDongZhao(long id) => _resolve().ResumeDongZhao(id);
    internal bool TryAdvanceDongZhaoSubtree() => _resolve().TryAdvanceDongZhaoSubtree();
    internal bool ValidIssuedXianlue(ProgramSkillFrame f) => _resolve().ValidIssuedXianlue(f);
    internal bool ValidIssuedZaowang(ProgramSkillFrame f) => _resolve().ValidIssuedZaowang(f);
    internal bool ZaowangFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().ZaowangFirstChild(root, child);
    internal bool ZaowangRebelDeathWins(CharacterState victim, CharacterState? killer) => _resolve().ZaowangRebelDeathWins(victim, killer);
}
