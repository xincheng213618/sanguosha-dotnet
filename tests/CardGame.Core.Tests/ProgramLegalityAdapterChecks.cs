using System.Reflection;
using System.Text.Json;
using CardGame.Core;

internal static class ProgramLegalityAdapterChecks
{
    public static void EagerValidationAndMatchLocalInvocation()
    {
        var methods = Methods(typeof(Receiver)).Concat(Methods(typeof(StaticRules))).Reverse().ToList();
        var catalog = ProgramLegalityCatalog.Discover(methods);
        methods.Clear();
        Require(catalog.Activations(ProgramActivationLegalityPhase.BeforeCondition).Select(gate => gate.Order).SequenceEqual([10, 20]) &&
                catalog.Triggers(ProgramTriggerLegalityPhase.BeforeCondition).Select(gate => gate.Order).SequenceEqual([10, 20]) &&
                catalog.Activations(ProgramActivationLegalityPhase.AfterCommonStart).Single().Order == 0,
            "Discovery preserves each phase's original precedence, including the legal zero order, independently of supplied method order.");
        Reject(typeof(Duplicate), "Duplicate program legality precedence");
        Reject(typeof(WrongReturn), "invalid registration or signature");
        Reject(typeof(Generic), "invalid registration or signature");
        Reject(typeof(DuplicateInputs), "invalid registration or signature");
        Reject(typeof(CollectionWindow), "invalid registration or signature");

        var program = Load();
        var plans = new ProgramInstructionResolver();
        var ownerA = Player(0);
        var ownerB = Player(1);
        var a = new Receiver(ownerA, program);
        var b = new Receiver(ownerB, program);
        // The resolver's actual owner takes precedence over this unrelated
        // session, and a shared adapter must never capture that first owner.
        var routerA = new ProgramLegalityRouter(catalog, new object(), _ => a);
        var routerB = new ProgramLegalityRouter(catalog, b);
        var activationA = Activation(ownerA, "draw");
        var activationB = Activation(ownerB, "draw");
        var triggerA = Trigger(ownerA);
        var triggerB = Trigger(ownerB);

        Check(() => routerA.CanActivate(ProgramActivationLegalityPhase.BeforeCondition, activationA), a, "activation");
        Check(() => routerB.CanActivate(ProgramActivationLegalityPhase.BeforeCondition, activationB), b, "activation");
        Check(() => routerA.CanTrigger(ProgramTriggerLegalityPhase.BeforeCondition, triggerA), a, "trigger");
        Check(() => routerB.CanTrigger(ProgramTriggerLegalityPhase.BeforeCondition, triggerB), b, "trigger");
        Require(routerA.CanActivate(ProgramActivationLegalityPhase.AfterCommonStart, activationA) &&
                !routerA.CanActivate(ProgramActivationLegalityPhase.AfterCommonStart,
                    activationA with { Context = activationA.Context with { Seat = ownerB.Seat } }),
            "The first static invocation maps the current owner, skill facts and optional scalar parameter instead of using defaults or another receiver.");

        a.FirstAllowed = false;
        a.Trace.Clear();
        Require(routerA.CanActivate(ProgramActivationLegalityPhase.BeforeCondition, Activation(ownerA, "recover")) &&
                a.Trace.SequenceEqual(["activation:last"]),
            "An operation-specific gate is skipped only when the original instruction lacks that operation; later unfiltered gates still run.");
        a.FirstAllowed = true;
        Require(a.Trace.Count == 1 && b.Trace.Count == 1,
            "Operation qualification and first-use adapter initialization do not execute another match's receiver.");

        void Check(Func<bool> invoke, Receiver receiver, string phase)
        {
            receiver.Trace.Clear();
            Require(invoke() && receiver.Trace.SequenceEqual([$"{phase}:first", $"{phase}:last"]),
                "The first invocation forwards its original typed context and property inputs in declared order.");
            receiver.FirstAllowed = false;
            receiver.Trace.Clear();
            Require(!invoke() && receiver.Trace.SequenceEqual([$"{phase}:first"]),
                "A failed early gate short-circuits before the later gate without changing adapter ownership.");
            receiver.FirstAllowed = true;
            receiver.LastAllowed = false;
            receiver.Trace.Clear();
            Require(!invoke() && receiver.Trace.SequenceEqual([$"{phase}:first", $"{phase}:last"]),
                "A later gate rechecks its receiver's live result after the shared adapter has already been used.");
            receiver.LastAllowed = true;
            receiver.Failure = new InvalidOperationException("original legality failure");
            receiver.Trace.Clear();
            try { _ = invoke(); throw new Exception("Expected the original legality failure."); }
            catch (InvalidOperationException error) when (ReferenceEquals(error, receiver.Failure))
            {
                Require(receiver.Trace.SequenceEqual([$"{phase}:first"]),
                    "A gate failure preserves its original exception and stops before later rules without reflection wrapping.");
            }
            finally { receiver.Failure = null; }
        }

        void Reject(Type invalid, string message)
        {
            try { _ = ProgramLegalityCatalog.Discover(Methods(typeof(Receiver)).Concat(Methods(invalid))); }
            catch (InvalidOperationException error) when (error.Message.Contains(message, StringComparison.Ordinal)) { return; }
            throw new InvalidOperationException($"Discovery did not eagerly reject {invalid.Name} before any invocation.");
        }
        ProgramActivationLegalityContext Activation(CharacterState owner, string binding) =>
            new(owner, program, plans.Resolve(program, ProgramInstructionSourceKind.Activation, binding),
                new(owner.Seat, owner.Hp, owner.MaxHp, 4, TurnPhase.Play, IsOwnTurn: true));
        ProgramTriggerLegalityContext Trigger(CharacterState owner) =>
            new(owner, program, plans.Resolve(program, ProgramInstructionSourceKind.Trigger, "ending"),
                new(owner.Seat, program.Id, "ending", "fixture:instance", program.GameplayHash, 0), null);
    }

    private sealed class Receiver(CharacterState owner, SkillProgram program)
    {
        internal List<string> Trace { get; } = [];
        internal bool FirstAllowed { get; set; } = true;
        internal bool LastAllowed { get; set; } = true;
        internal Exception? Failure { get; set; }

        [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCondition, 10, SkillProgramEffectOp.Draw)]
        internal bool ActivationFirst(ProgramActivationLegalityContext context)
        {
            Require(ReferenceEquals(context.Owner, owner) && ReferenceEquals(context.Program, program),
                "An activation adapter must use the current receiver and its exact original inputs.");
            return First("activation");
        }
        [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCondition, 20)]
        internal bool ActivationLast(SkillProgram actualProgram, SkillProgramActivation activation, int ownerSeat)
        {
            Require(ReferenceEquals(actualProgram, program) && program.Activations.Contains(activation) && ownerSeat == owner.Seat,
                "Activation property adapters retain the real program, original binding and current owner.");
            Trace.Add("activation:last");
            return LastAllowed;
        }
        [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeCondition, 10, SkillProgramEffectOp.Draw)]
        internal bool TriggerFirst(ProgramTriggerLegalityContext context)
        {
            Require(ReferenceEquals(context.Owner, owner) && ReferenceEquals(context.Program, program) && context.WindowContext is null,
                "A trigger adapter must not invent a resolution window while forwarding its original typed context.");
            return First("trigger");
        }
        [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeCondition, 20)]
        internal bool TriggerLast(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, int ownerSeat)
        {
            Require(program.Triggers.Contains(trigger) && candidate.BindingId == trigger.Id &&
                    candidate.SkillId == program.Id && ownerSeat == owner.Seat,
                "Trigger property adapters retain the actual candidate and original trigger binding.");
            Trace.Add("trigger:last");
            return LastAllowed;
        }
        private bool First(string phase)
        {
            Trace.Add(phase + ":first");
            if (Failure is { } failure) throw failure;
            return FirstAllowed;
        }
    }

    private static class StaticRules
    {
        // Zero precedence and an optional parameter are existing legal shapes;
        // the adapter supplies the real scalar rather than its default value.
        [ProgramActivationLegality(ProgramActivationLegalityPhase.AfterCommonStart, 0)]
        internal static bool PropertyInputs(CharacterState owner, PlayerSkillContext context, int ownerSeat = -1) =>
            owner.Seat == context.Seat && ownerSeat == owner.Seat;
    }
    private sealed class Duplicate
    {
        [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCondition, 10)]
        internal bool Gate(ProgramActivationLegalityContext context) => throw new Exception("Discovery invoked a duplicate gate.");
    }
    private sealed class WrongReturn
    {
        [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCondition, 30)]
        internal int Gate(ProgramActivationLegalityContext context) => throw new Exception("Discovery invoked an invalid gate.");
    }
    private sealed class Generic
    {
        [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCondition, 30)]
        internal bool Gate<T>(T context) => throw new Exception("Discovery invoked a generic gate.");
    }
    private sealed class DuplicateInputs
    {
        [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCondition, 30)]
        internal bool Gate(CharacterState first, CharacterState second) => throw new Exception("Discovery invoked an ambiguous gate.");
    }
    private sealed class CollectionWindow
    {
        [ProgramTriggerLegality(ProgramTriggerLegalityPhase.Collection, 30)]
        internal bool Gate(ProgramSkillWindowContext context) => throw new Exception("Discovery invoked a collection gate.");
    }

    private static SkillProgram Load() => SkillProgramCatalog.Load(SkillProgramTestJson.CurrentSchema("""
        {"schemaVersion":__RULES_SCHEMA__,"skills":[{"id":"fixture:legality-adapter","revision":1,
          "activations":[
            {"id":"draw","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"otherLiving","usesPerTurn":1,
             "effects":[{"op":"draw","target":"owner","amount":1}]},
            {"id":"recover","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"otherLiving","usesPerTurn":1,
             "effects":[{"op":"recover","target":"owner","amount":1}]}],
          "triggers":[{"id":"ending","window":"turnEnding","subject":"owner","optional":false,
            "effects":[{"op":"draw","target":"owner","amount":1}]}]}]}
        """), JsonSerializer.Serialize(new
        {
            schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
            skills = new Dictionary<string, object> { ["fixture:legality-adapter"] = new { name = "Legality adapter", description = "Shared invocation contract" } }
        })).Programs["fixture:legality-adapter"];
    private static CharacterState Player(int seat) => new()
    {
        Seat = seat, Name = "fixture", IsHuman = true, Role = Role.Rebel, RoleRevealed = true,
        General = new("fixture", "Fixture", "fixture", []), GeneralSelected = true,
        GeneralRevealed = true, MaxHp = 4, Hp = 3
    };
    private static MethodInfo[] Methods(Type type) => type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic |
        BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }
}
