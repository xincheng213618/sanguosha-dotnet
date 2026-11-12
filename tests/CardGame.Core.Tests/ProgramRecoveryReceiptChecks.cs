using System.Reflection;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.EngineStates;

internal static class ProgramRecoveryReceiptChecks
{
    private const string SkillId = "fixture:recovery-receipt-owner";

    public static void OriginalRequestsAndApplicationsOwnTypedChildren()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 7, HumanSeat = 0, HumanRole = Role.Lord,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, registry);
        Need(game.Submit(new StartGameCommand()).Accepted, "The receipt fixture starts its real event journal.");
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        var capabilities = (ProgramCapabilityRegistry)typeof(GameEngine).GetProperty("ProgramCapabilities", flags)!.GetValue(game)!;
        var frames = capabilities.Get<IProgramRuntimeFrames>();
        var query = capabilities.Get<IProgramHealthChildQueries>();
        var health = game.GetRuntimeModule<HealthContinuationRuntime>();
        var journal = ((CoreCommandJournalStorageState)typeof(GameEngine).GetField("_commandJournalStorage", flags)!.GetValue(game)!)
            .ReferenceToPendingEvents;
        var priorCount = frames.OfType<ResolutionFrame>().Count;
        var parent = new ProgramSkillFrame(frames.NextId(), 0, SkillId, "recover",
            registry.GetSkill(SkillId).Program!.GameplayHash, 1, [], []) { SkillInstanceId = "fixture:recovery-owner" };
        var candidate = new RecoveryReplacementCandidate(1, SkillId, "fixture:replacement", "replacement", 1, 1);
        var request = new RecoveryAttempt(frames.NextId(), 0, 0, 1, 2, 1, 0, [candidate],
            new(RecoveryAttemptProducer.Program, parent.InstructionIndex));
        parent = parent with { PendingRecoveryAttempts = [request] };
        frames.Push(parent);
        try
        {
            health.RecordProgramRecoveryRequest(parent, request);
            parent = (ProgramSkillFrame)frames.Find(parent.Id)!;
            var issued = parent.PendingRecoveryAttempts!.Single();
            Need(issued.ProgramOrigin == new ProgramRecoveryOrigin(parent.Id, parent.InstructionIndex, parent.GameplayHash) &&
                parent.PendingRecoveryAttempts is System.Collections.IList { IsReadOnly: true },
                "The actual pending request freezes the original program and instruction before its native recovery child is issued.");
            var issuedOwner = parent;
            var eventCount = journal.Count;
            health.RecordProgramRecoveryRequest(parent, issued);
            Need(ReferenceEquals(issuedOwner, frames.Find(parent.Id)) && journal.Count == eventCount,
                "Revisiting the same original request does not replace its origin or issue another requested fact.");
            Reject(() => health.RecordProgramRecoveryRequest(parent, issued with
                { ProgramOrigin = issued.ProgramOrigin! with { ProgramFrameId = long.MaxValue } }));
            Reject(() => health.RecordProgramRecoveryRequest(parent with { InstructionIndex = 2 }, issued));
            parent = parent with { PendingRecoveryAttempts = null };
            frames.Replace(parent.Id, parent);
            var recovery = new RecoveryReplacementFrame(issued.Id, parent.Id, issued,
                new(PostEventContinuation.Program, parent.Id), RecoveryReplacementStage.RecoveryApplied, OriginalRecovered: 1);
            frames.Push(recovery);
            try
            {
                CheckRequestReturn(parent, recovery, candidate);
            }
            finally { frames.Pop(recovery.Id, recovery.Kind); }

            var applied = new RecoveryFrame(frames.NextId(), parent.Id, 0, 0, 1, HpBefore: 2);
            var change = new HpChangeContext(frames.NextId(), parent.Id, 0, 0, HpChangeKind.Recovery, 1, 2, 3);
            health.RecordProgramRecoveryApplication(applied, change);
            parent = (ProgramSkillFrame)frames.Find(parent.Id)!;
            var application = parent.RecoveryApplications!.Single();
            var applicationOwner = parent;
            eventCount = journal.Count;
            health.RecordProgramRecoveryApplication(applied, change);
            Need(ReferenceEquals(applicationOwner, frames.Find(parent.Id)) && journal.Count == eventCount,
                "One real recovery and HP change retain one idempotent application invoice on their original owner.");
            Reject(() => health.RecordProgramRecoveryApplication(applied, change with { HpAfter = 4 }));
            var hp = new HpChangedTriggerWindowFrame(change.Id, change, [], [], PostEventContinuation.Program, parent.Id);
            frames.Push(hp);
            try
            {
                journal.RemoveAll(fact => fact is ProgramRecoveryAppliedEvent);
                Need(query.ProgramRecoveryChild(parent, hp),
                    "The original application's actual receipt authorizes its typed HP child without rediscovering an output event.");
                journal.Add(new ProgramRecoveryAppliedEvent(parent.Id, application));
                journal.Add(new ProgramRecoveryAppliedEvent(parent.Id, application));
                Need(query.ProgramRecoveryChild(parent, hp), "Repeated historical notifications do not replace or invalidate the original application receipt.");
                Need(!query.ProgramRecoveryChild(parent with { RecoveryApplications = null }, hp) &&
                    !query.ProgramRecoveryChild(parent with { RecoveryApplications = [application, application] }, hp),
                    "An old applied fact cannot supply a missing invoice or choose between duplicate invoices on the actual owner.");
                foreach (var malformed in new[]
                {
                    application with { InstructionIndex = parent.InstructionIndex + 1 },
                    application with { GameplayHash = "foreign-hash" },
                    application with { RecoveryFrameId = parent.Id },
                    application with { RequestedAmount = 0 },
                    application with { SourceSeat = 1 },
                    application with { TargetSeat = 1 },
                    application with { HpAfter = 4 }
                })
                    Need(!query.ProgramRecoveryChild(parent with { RecoveryApplications = [malformed] }, hp),
                        "A typed HP child cannot borrow another instruction, recovery, participant, amount or HP delta through a historical event.");
                Need(!query.ProgramRecoveryChild(parent, hp with { ResumeFrameId = long.MaxValue }) &&
                    !query.ProgramRecoveryChild(parent, hp with { Change = change with { Kind = HpChangeKind.Loss } }),
                    "The actual HP child keeps its native return owner and recovery kind.");
                var restored = JsonSerializer.Deserialize<ProgramSkillFrame>(JsonSerializer.Serialize(parent))!;
                Need(query.ProgramRecoveryChild(restored, hp) && restored.RecoveryApplications is System.Collections.IList { IsReadOnly: true },
                    "A serialized continuation retains its immutable exact application receipt without depending on a live event search.");
                capabilities.Get<IMatchParticipantStateAccess>().At(0).Hp = 1;
                Need(query.ProgramRecoveryChild(parent, hp),
                    "Later current HP does not rewrite the already-paid native recovery's frozen before/after invoice.");
                health.ClearProgramRecoveryApplication(change);
                Need(!query.ProgramRecoveryChild((ProgramSkillFrame)frames.Find(parent.Id)!, hp),
                    "Completing the real HP return clears its original invoice; old applied notifications cannot recreate it.");
            }
            finally { frames.Pop(hp.Id, hp.Kind); }
        }
        finally
        {
            while (frames.OfType<ResolutionFrame>().Count > priorCount)
            {
                var top = frames.Top!;
                if (top.PendingRecoveryAttempts is { Count: > 0 })
                    frames.Replace(top.Id, top with { PendingRecoveryAttempts = null });
                frames.Pop(top.Id, top.Kind);
            }
        }

        void CheckRequestReturn(ProgramSkillFrame owner, RecoveryReplacementFrame recovery, RecoveryReplacementCandidate replacement)
        {
            journal.RemoveAll(fact => fact is ProgramRecoveryRequestedEvent or RecoveryReplacementChosenEvent);
            Need(query.ProgramRecoveryChild(owner, recovery),
                "A real issued recovery reads its original attempt origin rather than requiring a requested notification to remain in history.");
            var origin = recovery.Attempt.ProgramOrigin!;
            var oldFact = new ProgramRecoveryRequestedEvent(owner.Id, owner.InstructionIndex, owner.GameplayHash,
                recovery.Id, recovery.Attempt.SourceSeat, recovery.Attempt.TargetSeat, recovery.Attempt.Amount, recovery.Attempt.HpBefore);
            journal.Add(oldFact); journal.Add(oldFact);
            Need(query.ProgramRecoveryChild(owner, recovery), "Duplicate requested notifications do not change the actual attempt's issuing owner.");
            var awaited = recovery with { Return = new(PostEventContinuation.AwaitedProgramMovement, owner.Id) };
            Need(!query.ProgramRecoveryChild(owner, awaited) && query.ProgramRecoveryChild(owner with
                { PendingMovementContinuation = new(owner.OwnerSeat, 0, null) }, awaited),
                "An awaited movement return retains its real producer-owned continuation instead of being narrowed to a direct Program return.");
            foreach (var malformed in new[]
            {
                recovery.Attempt with { ProgramOrigin = null },
                recovery.Attempt with { ProgramOrigin = origin with { ProgramFrameId = long.MaxValue } },
                recovery.Attempt with { ProgramOrigin = origin with { InstructionIndex = 2 } },
                recovery.Attempt with { ProgramOrigin = origin with { GameplayHash = "foreign-hash" } },
                recovery.Attempt with { Completion = new(RecoveryAttemptProducer.SilverLion) },
                recovery.Attempt with { Amount = 0 },
                recovery.Attempt with { HpBefore = null },
                recovery.Attempt with { HappyQiaoBefore = new(2, 1, 4) }
            })
                Need(!query.ProgramRecoveryChild(owner, recovery with { Attempt = malformed }),
                    "Historical requested facts cannot authorize a missing or foreign origin, another producer, a zero request, or an inconsistent scalar/dual-health snapshot.");
            var dual = recovery with { Attempt = recovery.Attempt with { HpBefore = null, HappyQiaoBefore = new(2, 1, 4) } };
            var restored = JsonSerializer.Deserialize<RecoveryReplacementFrame>(JsonSerializer.Serialize(dual))!;
            Need(query.ProgramRecoveryChild(owner, restored),
                "A dual-health request preserves its original pair of pools and origin across JSON without inventing a scalar HP-before value.");
            var selected = recovery with { CandidateIndex = 0, OriginalRecovered = 0 };
            Need(query.ProgramRecoveryChild(owner, selected),
                "The actual accepted candidate index authorizes its paid return without recomputing current skill possession or requiring a chosen notification.");
            journal.Add(new RecoveryReplacementChosenEvent(recovery.Id, recovery.Attempt.TargetSeat, replacement.OwnerSeat,
                replacement.SkillId, replacement.SkillInstanceId, replacement.PolicyId, recovery.Attempt.Amount, replacement.RecoveryAmount));
            journal.Add(journal[^1]);
            Need(query.ProgramRecoveryChild(owner, selected), "Repeated chosen output facts do not become a second candidate-acceptance answer.");
            foreach (var malformed in new[]
            {
                selected with { Stage = RecoveryReplacementStage.Choosing },
                selected with { CandidateIndex = 1 },
                selected with { OriginalRecovered = 1 },
                selected with { PendingApplication = new(0, 1, 1, 0) },
                selected with { Attempt = selected.Attempt with { Candidates = [replacement with { RecoveryAmount = 2 }] } },
                recovery with { OriginalRecovered = 2 },
                recovery with { Stage = (RecoveryReplacementStage)999 }
            })
                Need(!query.ProgramRecoveryChild(owner, malformed),
                    "A typed recovery retains actual candidate, application stage and supported-amount bounds instead of accepting an old chosen event as payment.");
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("recovery-receipt-check", new(1, 0, 0), []);
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.LoadCurrent($$$$"""
                {"skills":[{"id":"{{{{SkillId}}}}","revision":1,"cardPolicies":[
                  {"id":"replacement","kind":"redirectOwnTurnFactionRecovery","factionId":"wu","ownerRole":"lord","value":1,"providerDrawCount":1}],
                  "activations":[{"id":"recover","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,
                    "targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"recover","target":"owner","amount":1}]}]}]}
                """, $$$$"""
                {"schemaVersion":{{{{SkillProgramCatalog.PresentationSchemaVersion}}}},"skills":{"{{{{SkillId}}}}":{"name":"Recovery receipts","description":"Original request and application owners"}}}
                """);
            builder.AddSkill(new ContentSkillDefinition(SkillId, "Recovery receipts", "Original request and application owners")
            { Program = catalog.Programs[SkillId] });
        }
    }

    private static void Need(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
    private static void Reject(Action action)
    {
        try { action(); }
        catch (InvalidOperationException) { return; }
        throw new InvalidOperationException("A recovery request or application changed its actual original invoice.");
    }
}
