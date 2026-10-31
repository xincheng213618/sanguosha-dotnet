#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSequentialDeckKindGrowthEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome AcquireDeckKindSequentiallyAndGrowOnShortfall(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect) => _resolve().AcquireDeckKindSequentiallyAndGrowOnShortfall(supplied, effect);
    internal void AddSequentialDeckKindGrowthBonusAttackRangeContribution(global::CardGame.Core.CharacterState player, global::System.Collections.Generic.List<global::CardGame.Core.RuleQueryContribution> contributions) => _resolve().AddSequentialDeckKindGrowthBonusAttackRangeContribution(player, contributions);
    internal bool CanContinueSequentialDeckKindGrowth(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CanContinueSequentialDeckKindGrowth(frame);
    internal bool DrainSequentialDeckKindGrowthChildren(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().DrainSequentialDeckKindGrowthChildren(frame);
    internal void FinishSequentialDeckKindGrowth(global::CardGame.Core.ProgramSkillFrame frame, bool cancelled) => _resolve().FinishSequentialDeckKindGrowth(frame, cancelled);
    internal bool HasSequentialDeckKindGrowthParent(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().HasSequentialDeckKindGrowthParent(frame);
    internal bool ResumeSequentialDeckKindGrowth(long frameId) => _resolve().ResumeSequentialDeckKindGrowth(frameId);
    internal bool ReturnSequentialDeckKindGrowthMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnSequentialDeckKindGrowthMovement(frame);
    internal int SequentialDeckKindGrowthBonus(int owner) => _resolve().SequentialDeckKindGrowthBonus(owner);
    internal bool ValidSequentialDeckKindGrowth(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidSequentialDeckKindGrowth(frame);
    internal bool ValidSequentialDeckKindGrowthGrant(global::CardGame.Core.SequentialDeckKindGrowthGrantedEvent growth) => _resolve().ValidSequentialDeckKindGrowthGrant(growth);
    internal bool ValidSequentialDeckKindGrowthStep(long frameId, int owner, global::CardGame.Core.CardKind kind, global::CardGame.Core.SequentialDeckKindGrowthStepReceipt step) => _resolve().ValidSequentialDeckKindGrowthStep(frameId, owner, kind, step);
}
