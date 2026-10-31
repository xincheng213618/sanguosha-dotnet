#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSequentialDeckKindGrowthEntries
{
    internal SkillProgramStepOutcome AcquireDeckKindSequentiallyAndGrowOnShortfall(ProgramSkillFrame supplied, SkillProgramEffect effect) => _resolve().AcquireDeckKindSequentiallyAndGrowOnShortfall(supplied, effect);
    internal void AddSequentialDeckKindGrowthBonusAttackRangeContribution(CharacterState player, List<RuleQueryContribution> contributions) => _resolve().AddSequentialDeckKindGrowthBonusAttackRangeContribution(player, contributions);
    internal bool CanContinueSequentialDeckKindGrowth(ProgramSkillFrame frame) => _resolve().CanContinueSequentialDeckKindGrowth(frame);
    internal bool DrainSequentialDeckKindGrowthChildren(ProgramSkillFrame frame) => _resolve().DrainSequentialDeckKindGrowthChildren(frame);
    internal void FinishSequentialDeckKindGrowth(ProgramSkillFrame frame, bool cancelled) => _resolve().FinishSequentialDeckKindGrowth(frame, cancelled);
    internal bool HasSequentialDeckKindGrowthParent(ProgramSkillFrame frame) => _resolve().HasSequentialDeckKindGrowthParent(frame);
    internal bool ResumeSequentialDeckKindGrowth(long frameId) => _resolve().ResumeSequentialDeckKindGrowth(frameId);
    internal bool ReturnSequentialDeckKindGrowthMovement(ProgramSkillFrame frame) => _resolve().ReturnSequentialDeckKindGrowthMovement(frame);
    internal int SequentialDeckKindGrowthBonus(int owner) => _resolve().SequentialDeckKindGrowthBonus(owner);
    internal bool ValidSequentialDeckKindGrowth(ProgramSkillFrame frame) => _resolve().ValidSequentialDeckKindGrowth(frame);
    internal bool ValidSequentialDeckKindGrowthGrant(SequentialDeckKindGrowthGrantedEvent growth) => _resolve().ValidSequentialDeckKindGrowthGrant(growth);
    internal bool ValidSequentialDeckKindGrowthStep(long frameId, int owner, CardKind kind, SequentialDeckKindGrowthStepReceipt step) => _resolve().ValidSequentialDeckKindGrowthStep(frameId, owner, kind, step);
}
