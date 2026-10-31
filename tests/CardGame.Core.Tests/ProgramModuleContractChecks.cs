using System.Reflection;
using CardGame.Core;

internal static class ProgramModuleContractChecks
{

    public static void DeadTargetPolicyKeepsReceiptAndHostBoundaries()
    {
        var frame = new ProgramSkillFrame(1, 0, "fixture", "run", "hash", 0, [], []);
        var host = new PermissionHost();
        bool Allows(SkillProgramEffectOp op, ProgramSkillFrame? current = null, bool paid = false)
        {
            var effect = new SkillProgramEffect(op, SkillProgramEffectTarget.Owner, 0, new(SkillProgramConditionKind.Always, 0, []));
            return ProgramOperationCatalog.Default.Resolve(op).AllowsDeadTarget(new(effect, current ?? frame, host, paid));
        }
        Require(!Allows(SkillProgramEffectOp.Draw) && !Allows(SkillProgramEffectOp.ResolveMouZhouYuYingzi) && host.Queries == 0,
            "Ordinary and unrelated sibling operations must retain living-target requirements.");
        var legacy = new SkillProgramEffect(SkillProgramEffectOp.UseVirtualDuel, SkillProgramEffectTarget.Owner, 0, new(SkillProgramConditionKind.Always, 0, []));
        Require(!ProgramOperationCatalog.Default.AllowsDeadTarget(new(legacy, frame, host, false)),
            "An unregistered legacy operation must retain its denied dead-target permission.");
        Require(Allows(SkillProgramEffectOp.SelectTarget) && host.Queries == 0,
            "The existing death-flow target selector does not require a second host permission query.");
        Require(!Allows(SkillProgramEffectOp.ResolveMouZhouYuFanjian) && host.Queries == 1,
            "A paid continuation operation must still ask its current host.");
        host.Allow = true;
        Require(Allows(SkillProgramEffectOp.ResolveMouZhouYuFanjian) && host.Queries == 2 && ReferenceEquals(host.LastFrame, frame),
            "The current frame and per-match permission must reach the owning operation.");
        Require(!Allows(SkillProgramEffectOp.ClearLiushiEffect) && !Allows(SkillProgramEffectOp.ResolvePrepDiscardOrImmediateDraw, paid: true),
            "Host approval and a previously accepted return cannot replace an absent payment receipt.");
        Require(!Allows(SkillProgramEffectOp.DiscardDeathSourceCards) &&
                Allows(SkillProgramEffectOp.DiscardDeathSourceCards, frame with { WindowContext = new(SkillProgramTriggerWindow.OwnerDied, 9, 0) }),
            "A death-source operation must retain its original owner-death window restriction.");
        host.Failure = new InvalidOperationException("permission failure");
        try { _ = Allows(SkillProgramEffectOp.ResolveMouZhouYuFanjian); throw new InvalidOperationException("Expected host failure."); }
        catch (InvalidOperationException exception) when (ReferenceEquals(exception, host.Failure)) { }
    }
    private static void Require(bool value, string message) { if (!value) throw new InvalidOperationException(message); }
    private sealed class PermissionHost : ISkillProgramExecutionHost
    {
        internal bool Allow;
        internal int Queries;
        internal ProgramSkillFrame? LastFrame;
        internal Exception? Failure;
        public bool CanContinueAfterOwnerDeath(ProgramSkillFrame frame)
        { Queries++; LastFrame = frame; if (Failure is { } failure) throw failure; return Allow; }
        public bool IsGameOver => false;
        public ProgramSkillFrame? GetActiveFrame(long id) => throw new NotSupportedException();
        public SkillProgram GetProgram(string id) => throw new NotSupportedException();
        public SkillProgramActorState GetActor(int seat) => throw new NotSupportedException();
        public bool OwnsSkillInstance(int seat, string skill, string instance) => throw new NotSupportedException();
        public bool OwnsCards(int seat, IReadOnlyList<int> cards, IReadOnlyList<CardZoneKind> zones) => throw new NotSupportedException();
        public bool EvaluateCondition(ProgramSkillFrame frame, SkillProgramCondition condition, PlayerSkillContext context) => throw new NotSupportedException();
        public void UpdateFrame(ProgramSkillFrame frame) => throw new NotSupportedException();
        public void Complete(ProgramSkillFrame frame, bool completed, string? reason = null) => throw new NotSupportedException();
    }
}
