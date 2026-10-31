#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuaXiongMouMobileEntries
{
    internal bool AdvanceMouHuaXiongChildren() => _resolve().AdvanceMouHuaXiongChildren();
    internal void AnswerMouYaowu(PromptChoice selected) => _resolve().AnswerMouYaowu(selected);
    internal IReadOnlyList<PlayerMarkerSnapshot>? AppendMouHuaXiongWei(int owner, IReadOnlyList<PlayerMarkerSnapshot>? ordinary) => _resolve().AppendMouHuaXiongWei(owner, ordinary);
    internal bool CanActivateMouYangwei(ProgramActivationLegalityContext c) => _resolve().CanActivateMouYangwei(c);
    internal bool CanOfferMouYaowu(ProgramTriggerLegalityContext c) => _resolve().CanOfferMouYaowu(c);
    internal void CommitMouYaowuChoice(ProgramSkillFrame f, string benefit, int? recipient) => _resolve().CommitMouYaowuChoice(f, benefit, recipient);
    internal bool DistanceCardUseExemption20HasMouHuaXiongWei(ProgramCardDistanceQuery q) => _resolve().DistanceCardUseExemption20HasMouHuaXiongWei(q);
    internal bool HasMouHuaXiongWei(int owner) => _resolve().HasMouHuaXiongWei(owner);
    internal bool IsMouHuaXiongDying() => _resolve().IsMouHuaXiongDying();
    internal bool IsMouHuaXiongRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsMouHuaXiongRecoveryParent(f, recovery);
    internal bool KeepPaidMouHuaXiong(ProgramSkillFrame f) => _resolve().KeepPaidMouHuaXiong(f);
    internal bool MouHuaXiongFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouHuaXiongFirstChild(f, child);
    internal ProgramSkillFrame? MouHuaXiongObserverRoot() => _resolve().MouHuaXiongObserverRoot();
    internal bool MouHuaXiongObserverSuffix(int index) => _resolve().MouHuaXiongObserverSuffix(index);
    internal bool MouHuaXiongPaidRoot(ProgramSkillFrame f) => _resolve().MouHuaXiongPaidRoot(f);
    internal bool MouYangweiCooling(int owner) => _resolve().MouYangweiCooling(owner);
    internal bool MouYangweiRoot(ProgramSkillFrame f) => _resolve().MouYangweiRoot(f);
    internal IReadOnlyList<PromptChoice> MouYaowuChoices(ProgramSkillFrame f) => _resolve().MouYaowuChoices(f);
    internal bool MouYaowuRoot(ProgramSkillFrame f) => _resolve().MouYaowuRoot(f);
    internal void RecoverMouYangweiAtEnding(int owner) => _resolve().RecoverMouYangweiAtEnding(owner);
    internal SkillProgramStepOutcome ResolveMouYangwei(ProgramSkillFrame supplied) => _resolve().ResolveMouYangwei(supplied);
    internal SkillProgramStepOutcome ResolveMouYaowu(ProgramSkillFrame supplied) => _resolve().ResolveMouYaowu(supplied);
    internal PromptChoice SelectAiMouYaowu(PendingDecision p, ProgramSkillFrame f) => _resolve().SelectAiMouYaowu(p, f);
    internal bool ValidMouYangweiReceipt(ProgramSkillFrame f) => _resolve().ValidMouYangweiReceipt(f);
    internal bool ValidMouYaowuReceipt(ProgramSkillFrame f) => _resolve().ValidMouYaowuReceipt(f);
}
