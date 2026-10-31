#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangSkillsBAuditEntries
{
    internal void AddWushuangGuzhengChoices(global::CardGame.Core.ProgramSkillFrame f, global::System.Collections.Generic.ICollection<global::CardGame.Core.PromptChoice> choices) => _resolve().AddWushuangGuzhengChoices(f, choices);
    internal bool AnswerWushuangGuzheng(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PromptChoice choice) => _resolve().AnswerWushuangGuzheng(f, choice);
    internal bool CanTriggerWushuangBAudit(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanTriggerWushuangBAudit(c, trigger, context);
    internal void ObserveWushuangBAudit(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveWushuangBAudit(payload);
    internal bool WushuangFuqiCannotRespond(int source, int responder) => _resolve().WushuangFuqiCannotRespond(source, responder);
    internal bool WushuangFuqiNativeResponseForbidden(global::CardGame.Core.ProgramNativeResponseQuery query) => _resolve().WushuangFuqiNativeResponseForbidden(query);
    internal bool WushuangFuqiNullificationBanned(int responder) => _resolve().WushuangFuqiNullificationBanned(responder);
    internal global::System.Collections.Generic.IReadOnlyList<int> WushuangGuzhengCards(int phaseOwner) => _resolve().WushuangGuzhengCards(phaseOwner);
}
