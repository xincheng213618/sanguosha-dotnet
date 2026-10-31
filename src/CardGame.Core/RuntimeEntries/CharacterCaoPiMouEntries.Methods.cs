#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoPiMouEntries
{
    internal IEnumerable<MouCaoPiControl> ActiveMouCaoPiControls(int seat) => _resolve().ActiveMouCaoPiControls(seat);
    internal bool AdvanceMouCaoPiChildren() => _resolve().AdvanceMouCaoPiChildren();
    internal void AnswerMouCaoPi(PromptChoice choice) => _resolve().AnswerMouCaoPi(choice);
    internal SkillProgramStepOutcome BeginMouCaoPi(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginMouCaoPi(supplied, op);
    internal bool CanActivateMouCaoPi(ProgramActivationLegalityContext c) => _resolve().CanActivateMouCaoPi(c);
    internal bool CanGainMouSongwei(ProgramTriggerLegalityContext c) => _resolve().CanGainMouSongwei(c);
    internal PromptChoice ChooseAiMouCaoPi(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseAiMouCaoPi(p, f);
    internal void ContinueMouCaoPi(long id) => _resolve().ContinueMouCaoPi(id);
    internal bool ContinuePaidMouCaoPi(ProgramSkillFrame f) => _resolve().ContinuePaidMouCaoPi(f);
    internal bool DrainMouCaoPi(ProgramSkillFrame f) => _resolve().DrainMouCaoPi(f);
    internal void ExpireMouCaoPiControls(TurnEndedEvent e) => _resolve().ExpireMouCaoPiControls(e);
    internal void FinishMouCaoPi(ProgramSkillFrame f) => _resolve().FinishMouCaoPi(f);
    internal bool HasMouXingshang(CharacterState owner) => _resolve().HasMouXingshang(owner);
    internal bool IsMouCaoPiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsMouCaoPiRecoveryParent(f, recovery);
    internal void LoseMouCaoPiSkills(CharacterState p, int source, IReadOnlyList<string>? ids) => _resolve().LoseMouCaoPiSkills(p, source, ids);
    internal bool MouCaoPiCannotRespondTo(int responder, int source) => _resolve().MouCaoPiCannotRespondTo(responder, source);
    internal bool MouCaoPiCardUseForbidden(ProgramCardUseQuery query) => _resolve().MouCaoPiCardUseForbidden(query);
    internal IReadOnlyList<PromptChoice> MouCaoPiChoices(ProgramSkillFrame f) => _resolve().MouCaoPiChoices(f);
    internal IReadOnlyList<MouCaoPiControl>? MouCaoPiControlsSnapshot(int seat) => _resolve().MouCaoPiControlsSnapshot(seat);
    internal bool MouCaoPiFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouCaoPiFirstChild(f, child);
    internal IEnumerable<SkillGrant> MouCaoPiGeneralGrants(CharacterState p) => _resolve().MouCaoPiGeneralGrants(p);
    internal bool MouCaoPiHandUseForbidden(int actor, CardKind effectiveKind, IEnumerable<int> ids, bool response = false) => _resolve().MouCaoPiHandUseForbidden(actor, effectiveKind, ids, response);
    internal bool MouCaoPiLegalActionForbidden(ProgramLegalActionQuery query) => _resolve().MouCaoPiLegalActionForbidden(query);
    internal bool MouCaoPiMaterialUseForbidden(ProgramCardMaterialQuery query) => _resolve().MouCaoPiMaterialUseForbidden(query);
    internal ProgramSkillFrame? MouCaoPiObserverRoot() => _resolve().MouCaoPiObserverRoot();
    internal string[] MouCaoPiPrintedSkills(CharacterState dead) => _resolve().MouCaoPiPrintedSkills(dead);
    internal bool MouCaoPiResponseForbidden(int actor, CardKind kind, CardActionType type) => _resolve().MouCaoPiResponseForbidden(actor, kind, type);
    internal int? MouCaoPiResponseSource() => _resolve().MouCaoPiResponseSource();
    internal bool MouCaoPiSkillsQualified(CharacterState owner, SkillGrant grant) => _resolve().MouCaoPiSkillsQualified(owner, grant);
    internal void PublishMouCaoPi(ProgramSkillFrame f) => _resolve().PublishMouCaoPi(f);
    internal bool ResumeMouCaoPi(long id) => _resolve().ResumeMouCaoPi(id);
    internal bool ReturnMouCaoPi(ProgramSkillFrame f) => _resolve().ReturnMouCaoPi(f);
    internal void ReturnMouCaoPiDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnMouCaoPiDraw(f, outcome);
    internal int Song(int seat) => _resolve().Song(seat);
}
