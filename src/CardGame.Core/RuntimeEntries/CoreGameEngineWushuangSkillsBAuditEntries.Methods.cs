#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangSkillsBAuditEntries
{
    internal bool CanTriggerWushuangBAudit(ProgramTriggerCandidate c, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanTriggerWushuangBAudit(c, trigger, context);
    internal void ObserveWushuangBAudit(IGameEvent payload) => _resolve().ObserveWushuangBAudit(payload);
    internal bool WushuangFuqiCannotRespond(int source, int responder) => _resolve().WushuangFuqiCannotRespond(source, responder);
    internal bool WushuangFuqiNativeResponseForbidden(ProgramNativeResponseQuery query) => _resolve().WushuangFuqiNativeResponseForbidden(query);
    internal bool WushuangFuqiNullificationBanned(int responder) => _resolve().WushuangFuqiNullificationBanned(responder);
}
