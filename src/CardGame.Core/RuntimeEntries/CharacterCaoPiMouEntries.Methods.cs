#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoPiMouEntries
{
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.MouCaoPiControl> ActiveMouCaoPiControls(int seat) => _resolve().ActiveMouCaoPiControls(seat);
    internal bool AdvanceMouCaoPiChildren() => _resolve().AdvanceMouCaoPiChildren();
    internal void AnswerMouCaoPi(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerMouCaoPi(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMouCaoPi(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginMouCaoPi(supplied, op);
    internal bool CanActivateMouCaoPi(global::CardGame.Core.ProgramActivationLegalityContext c) => _resolve().CanActivateMouCaoPi(c);
    internal bool CanGainMouSongwei(global::CardGame.Core.ProgramTriggerLegalityContext c) => _resolve().CanGainMouSongwei(c);
    internal global::CardGame.Core.PromptChoice ChooseAiMouCaoPi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseAiMouCaoPi(p, f);
    internal void ContinueMouCaoPi(long id) => _resolve().ContinueMouCaoPi(id);
    internal bool ContinuePaidMouCaoPi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidMouCaoPi(f);
    internal bool DrainMouCaoPi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainMouCaoPi(f);
    internal void ExpireMouCaoPiControls(global::CardGame.Core.TurnEndedEvent e) => _resolve().ExpireMouCaoPiControls(e);
    internal void FinishMouCaoPi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishMouCaoPi(f);
    internal bool HasMouXingshang(global::CardGame.Core.CharacterState owner) => _resolve().HasMouXingshang(owner);
    internal bool IsMouCaoPiRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsMouCaoPiRecoveryParent(f, recovery);
    internal void LoseMouCaoPiSkills(global::CardGame.Core.CharacterState p, int source, global::System.Collections.Generic.IReadOnlyList<string>? ids) => _resolve().LoseMouCaoPiSkills(p, source, ids);
    internal bool MouCaoPiCannotRespondTo(int responder, int source) => _resolve().MouCaoPiCannotRespondTo(responder, source);
    internal bool MouCaoPiCardUseForbidden(global::CardGame.Core.ProgramCardUseQuery query) => _resolve().MouCaoPiCardUseForbidden(query);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MouCaoPiChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouCaoPiChoices(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.MouCaoPiControl>? MouCaoPiControlsSnapshot(int seat) => _resolve().MouCaoPiControlsSnapshot(seat);
    internal bool MouCaoPiFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouCaoPiFirstChild(f, child);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.SkillGrant> MouCaoPiGeneralGrants(global::CardGame.Core.CharacterState p) => _resolve().MouCaoPiGeneralGrants(p);
    internal bool MouCaoPiHandUseForbidden(int actor, global::CardGame.Core.CardKind effectiveKind, global::System.Collections.Generic.IEnumerable<int> ids, bool response = false) => _resolve().MouCaoPiHandUseForbidden(actor, effectiveKind, ids, response);
    internal bool MouCaoPiLegalActionForbidden(global::CardGame.Core.ProgramLegalActionQuery query) => _resolve().MouCaoPiLegalActionForbidden(query);
    internal bool MouCaoPiMaterialUseForbidden(global::CardGame.Core.ProgramCardMaterialQuery query) => _resolve().MouCaoPiMaterialUseForbidden(query);
    internal global::CardGame.Core.ProgramSkillFrame? MouCaoPiObserverRoot() => _resolve().MouCaoPiObserverRoot();
    internal string[] MouCaoPiPrintedSkills(global::CardGame.Core.CharacterState dead) => _resolve().MouCaoPiPrintedSkills(dead);
    internal bool MouCaoPiResponseForbidden(int actor, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardActionType type) => _resolve().MouCaoPiResponseForbidden(actor, kind, type);
    internal int? MouCaoPiResponseSource() => _resolve().MouCaoPiResponseSource();
    internal bool MouCaoPiSkillsQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => _resolve().MouCaoPiSkillsQualified(owner, grant);
    internal void PublishMouCaoPi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishMouCaoPi(f);
    internal bool ResumeMouCaoPi(long id) => _resolve().ResumeMouCaoPi(id);
    internal bool ReturnMouCaoPi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMouCaoPi(f);
    internal void ReturnMouCaoPiDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnMouCaoPiDraw(f, outcome);
    internal int Song(int seat) => _resolve().Song(seat);
}
