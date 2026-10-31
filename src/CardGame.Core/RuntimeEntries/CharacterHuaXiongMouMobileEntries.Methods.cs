#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuaXiongMouMobileEntries
{
    internal bool AdvanceMouHuaXiongChildren() => _resolve().AdvanceMouHuaXiongChildren();
    internal void AnswerMouYaowu(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerMouYaowu(selected);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PlayerMarkerSnapshot>? AppendMouHuaXiongWei(int owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PlayerMarkerSnapshot>? ordinary) => _resolve().AppendMouHuaXiongWei(owner, ordinary);
    internal void AssertMouHuaXiong(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertMouHuaXiong(f);
    internal bool CanActivateMouYangwei(global::CardGame.Core.ProgramActivationLegalityContext c) => _resolve().CanActivateMouYangwei(c);
    internal bool CanOfferMouYaowu(global::CardGame.Core.ProgramTriggerLegalityContext c) => _resolve().CanOfferMouYaowu(c);
    internal void CommitMouYaowuChoice(global::CardGame.Core.ProgramSkillFrame f, string benefit, int? recipient) => _resolve().CommitMouYaowuChoice(f, benefit, recipient);
    internal bool DistanceCardUseExemption20HasMouHuaXiongWei(global::CardGame.Core.ProgramCardDistanceQuery q) => _resolve().DistanceCardUseExemption20HasMouHuaXiongWei(q);
    internal bool HasMouHuaXiongBeforeDamageObserver(long id) => _resolve().HasMouHuaXiongBeforeDamageObserver(id);
    internal bool HasMouHuaXiongDamageObserver(long id) => _resolve().HasMouHuaXiongDamageObserver(id);
    internal bool HasMouHuaXiongWei(int owner) => _resolve().HasMouHuaXiongWei(owner);
    internal bool IsMouHuaXiongChoiceResult(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramChoiceResultBinding binding) => _resolve().IsMouHuaXiongChoiceResult(f, binding);
    internal bool IsMouHuaXiongDying() => _resolve().IsMouHuaXiongDying();
    internal bool IsMouHuaXiongRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsMouHuaXiongRecoveryParent(f, recovery);
    internal bool KeepPaidMouHuaXiong(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepPaidMouHuaXiong(f);
    internal bool MouHuaXiongFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouHuaXiongFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? MouHuaXiongObserverRoot() => _resolve().MouHuaXiongObserverRoot();
    internal bool MouHuaXiongObserverSuffix(int index) => _resolve().MouHuaXiongObserverSuffix(index);
    internal bool MouHuaXiongPaidRoot(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouHuaXiongPaidRoot(f);
    internal bool MouYangweiCooling(int owner) => _resolve().MouYangweiCooling(owner);
    internal bool MouYangweiRoot(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouYangweiRoot(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MouYaowuChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouYaowuChoices(f);
    internal bool MouYaowuRoot(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouYaowuRoot(f);
    internal void RecoverMouYangweiAtEnding(int owner) => _resolve().RecoverMouYangweiAtEnding(owner);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveMouYangwei(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ResolveMouYangwei(supplied);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveMouYaowu(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ResolveMouYaowu(supplied);
    internal global::CardGame.Core.PromptChoice SelectAiMouYaowu(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiMouYaowu(p, f);
    internal bool ValidMouYangweiReceipt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMouYangweiReceipt(f);
    internal bool ValidMouYaowuReceipt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMouYaowuReceipt(f);
}
