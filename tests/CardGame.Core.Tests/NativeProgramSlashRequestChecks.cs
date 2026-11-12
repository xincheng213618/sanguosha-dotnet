using System.Reflection;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class NativeProgramSlashRequestChecks
{
    public static void OriginalInstructionOwnsOfferSelectionAndPaidAcceptance()
    {
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 7, PlayerCount = 5 }, StandardContentRegistry.Create());
        var capabilities = (ProgramCapabilityRegistry)typeof(GameEngine)
            .GetProperty("ProgramCapabilities", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
        var players = capabilities.Get<IMatchParticipantStateAccess>();
        foreach (var player in players.Players) player.IsAlive = true;
        var frames = capabilities.Get<IProgramRuntimeFrames>();
        var facts = new Facts();
        var ranges = new Ranges();
        var runtime = new NativeProgramSlashRequestRuntime(frames, players, facts, ranges);
        var frameId = frames.NextId();
        var invocation = new ProgramInvocationAuthorization(70, 69, SkillProgramTriggerWindow.CardUseCompleted,
            new(0, "fixture:request", "request", "fixture:instance", "fixture:hash", 0), ProgramFrameId: frameId);
        var original = new ProgramSkillFrame(frameId, 0, "fixture:request", "request", "fixture:hash", 1, [], [])
        { SkillInstanceId = "fixture:instance", InvocationAuthorization = invocation };
        frames.Push(original);
        ProgramSkillFrame Current() => (ProgramSkillFrame)frames.Find(original.Id)!;

        var targets = new List<int> { 2, 3 };
        var spec = new ProgramSlashRequestSpec(ProgramSlashRequestTargetSource.FrozenCandidates, targets,
            "request-result", IgnoreDistance: true, UsesChoiceResult: true);
        var issued = runtime.Issue(Current(), 1, spec);
        targets.Add(4);
        Need(issued.Spec.CandidateSeats.SequenceEqual([2, 3]) && issued.Spec.CandidateSeats is System.Collections.IList { IsReadOnly: true } &&
            issued.Source == new CardConversionSource(original.SkillId, original.ActivationId, original.OwnerSeat, original.SkillInstanceId) &&
            Current().InvocationAuthorization == invocation && !issued.Spec.CountsTowardSlashLimit &&
            runtime.GetTargets(Current(), 1).SequenceEqual([2, 3]) && runtime.GetOffer(Current(), 2) is null,
            "The request freezes its actor, target candidates, source and no-debit permission while invocation authorization remains on the original instruction.");
        Need(runtime.Issue(Current(), 1, spec) == issued, "Reentering the same original request is idempotent.");
        Reject(() => runtime.Issue(Current(), 2, spec));
        Reject(() => runtime.Issue(Current(), 1, new(ProgramSlashRequestTargetSource.FrozenCandidates, [4])));
        facts.CanInvoke = false;
        Need(runtime.GetOffer(Current()) is null && runtime.GetIdentity(Current()) == issued && runtime.OwnsIssuedIdentity(Current(), issued, 1),
            "An unretained offer loses source eligibility without erasing its issued identity.");
        facts.CanInvoke = true;
        players.At(0).IsAlive = false;
        Need(runtime.GetOffer(Current()) is null, "Source invocation eligibility never implicitly permits the source owner's death.");
        players.At(0).IsAlive = true;
        runtime.SelectTarget(Current(), 1, 3);
        Need(runtime.GetTargets(Current(), 1).SequenceEqual([3]) && runtime.GetOffer(Current()) is not null,
            "Target selection narrows the same unpaid offer while cancellable material selection retains its authority.");
        Reject(() => runtime.SelectTarget(Current(), 1, 2));
        var acceptance = runtime.Accept(Current(), 1, 3);
        var recovered = JsonSerializer.Deserialize<ProgramSlashRequestState>(JsonSerializer.Serialize(Current().SlashRequest))!;
        Need(issued == recovered.Identity && acceptance == recovered.Acceptance && issued.GetHashCode() == recovered.Identity.GetHashCode() &&
            NativeProgramSlashRequestRuntime.SameIdentity(issued, recovered.Identity) && recovered.SelectedTargetSeat == 3 &&
            recovered.Acceptance is { TargetSeat: 3 } && recovered.Identity.Spec.CandidateSeats is System.Collections.IList { IsReadOnly: true },
            "Raw JSON retains the selected and accepted target on one immutable request identity.");
        frames.Replace(original.Id, Current() with { SlashRequest = recovered });
        facts.CanInvoke = false; players.At(0).IsAlive = false; players.At(3).IsAlive = false;
        Need(runtime.GetOffer(Current()) is null && !runtime.OwnsOffer(Current(), 1, 3) &&
            runtime.GetTargets(Current(), 1).SequenceEqual([3]) && runtime.OwnsAcceptance(Current(), acceptance, 1, 3) &&
            runtime.Accept(Current(), 1, 3) == recovered.Acceptance &&
            !runtime.OwnsAcceptance(Current(), acceptance with { TargetSeat = 2 }, 1, 2),
            "A paid child retains its original accepted target after source loss, owner death and target death without reopening an offer.");
        runtime.Close(Current());
        Need(runtime.Issue(Current(), 1, spec).Equals(recovered.Identity) && runtime.GetOffer(Current()) is null &&
            runtime.OwnsAcceptance(Current(), acceptance, 1, 3), "Closing or reentering a paid request never revives its offer or discards its paid authority.");

        players.At(0).IsAlive = true; players.At(3).IsAlive = true;
        var retained = new ProgramSlashRequestSpec(ProgramSlashRequestTargetSource.AllOtherLiving,
            RetainSourceLoss: true);
        runtime.Issue(Current(), 1, retained, 1);
        Need(runtime.GetTargets(Current(), 1).SequenceEqual(players.Players.Where(player => player.Seat != 1).Select(player => player.Seat)),
            "An explicitly retained source-loss offer still uses the complete current other-living audience.");
        players.At(0).IsAlive = false;
        Need(runtime.GetOffer(Current()) is null, "Source-loss retention and owner-death retention remain independent permissions.");
        runtime.Close(Current());
        runtime.Issue(Current(), 1, new(ProgramSlashRequestTargetSource.AllOtherLiving,
            RetainSourceLoss: true, RetainOwnerDeath: true), 2);
        Need(runtime.GetOffer(Current()) is not null, "An explicit other-actor request may retain both permissions.");
        players.At(1).IsAlive = false;
        Need(runtime.GetOffer(Current()) is null, "Retaining the source owner's death never authorizes a dead requested actor to issue a new use.");
        players.At(0).IsAlive = true; players.At(1).IsAlive = true; facts.CanInvoke = true;
        runtime.Close(Current());
        Reject(() => runtime.Issue(Current(), 1, retained, 1));

        facts.Distances[0] = 2; facts.Distances[2] = 1; facts.Distances[3] = 1; facts.Distances[4] = 3;
        runtime.Issue(Current(), 1, new(ProgramSlashRequestTargetSource.NearestLiving, [2, 3], NearestStrictSnapshot: true), 3);
        Need(runtime.GetTargets(Current(), 1).SequenceEqual([2, 3]), "Nearest candidates are all equally near living seats before independent native legality.");
        facts.Distances[3] = 2;
        Need(runtime.GetTargets(Current(), 1).Count == 0, "A strict nearest snapshot does not quietly choose a smaller later candidate set.");
        runtime.Close(Current());
        runtime.Issue(Current(), 1, new(ProgramSlashRequestTargetSource.NearestLiving), 4);
        Need(runtime.GetTargets(Current(), 1).SequenceEqual([2]), "A dynamic nearest request follows the current nearest-living scope instead.");
        var nearestPaid = runtime.Accept(Current(), 1, 2);
        facts.Distances[4] = 0; players.At(2).IsAlive = false;
        Need(runtime.GetNearestLivingSeats(1).SequenceEqual([4]) && runtime.GetTargets(Current(), 1).SequenceEqual([2]) &&
            runtime.OwnsAcceptance(Current(), nearestPaid, 1, 2), "A paid nearest request never resamples a replacement target for its accepted child.");
        frames.Replace(original.Id, Current() with { InstructionIndex = 2 });
        Need(runtime.GetIdentity(Current()) is null && !runtime.OwnsAcceptance(Current(), nearestPaid, 1, 2),
            "A request cannot authorize a different active instruction.");
        runtime.Issue(Current(), 1, new(ProgramSlashRequestTargetSource.FrozenCandidates, []));
        Need(runtime.GetTargets(Current(), 1).Count == 0, "An explicit empty target set never becomes every living character.");
        runtime.Close(Current());
        ranges.Targets.Add((1, 3));
        runtime.Issue(Current(), 1, new(ProgramSlashRequestTargetSource.FrozenCandidates, [3, 4], RangeOwnerSeat: 1), 1);
        Need(runtime.GetTargets(Current(), 1).SequenceEqual([3]), "An explicit request range belongs to its declared owner, including the requested actor itself.");
        var rangePaid = runtime.Accept(Current(), 1, 3);
        ranges.Targets.Clear();
        Need(runtime.GetTargets(Current(), 1).SequenceEqual([3]) && runtime.OwnsAcceptance(Current(), rangePaid, 1, 3),
            "An issued paid target does not lose its identity when its original request range changes.");
        runtime.Close(Current());
        var childIdentity = runtime.Issue(Current(), 1, new(ProgramSlashRequestTargetSource.FrozenCandidates, [3]), 2);
        var nativeAuthorization = new ProgramAssistedSlashAuthorization(Current().InstructionIndex)
        { RequestIdentity = childIdentity, TargetSeat = 3 };
        facts.CanInvoke = false; players.At(0).IsAlive = false;
        Reject(() => runtime.Accept(Current(), 1, 3));
        Reject(() => runtime.AcceptIssued(Current(), nativeAuthorization with { TargetSeat = 4 }, 1, 3));
        var nativePaid = runtime.AcceptIssued(Current(), nativeAuthorization, 1, 3);
        Need(runtime.OwnsAcceptance(Current(), nativePaid, 1, 3) && runtime.GetOffer(Current()) is null &&
            !runtime.CanExecuteAcceptedUse(Current(), nativePaid),
            "An accepted payment remains a fact without acquiring undeclared source-loss or owner-death execution permission.");
        var execution = new NativeProgramSlashRequestExecutionRuntime(frames, runtime,
            CardGame.Core.RuntimeEntries.CoreGameEngineProgramSlashRequestEntries.Create(() =>
                game.GetRuntimeModule<CardGame.Core.EngineOwners.CoreGameEngineProgramSlashRequestRuntime>()),
            capabilities.Get<IProgramRuntimePrompts>());
        Reject(() => execution.PrepareAcceptedUse(Current(), nativePaid));
        facts.CanInvoke = true; players.At(0).IsAlive = true;
        frames.Replace(original.Id, Current() with { AssistedSlashRequest = new(1, 3) });
        var provider = new ProgramSkillFrame(frames.NextId(), 1, "fixture:provider", "provide", "fixture:provider-hash", 1, [], []);
        frames.Push(provider);
        Need(runtime.AcceptIssued(Current(), nativeAuthorization, 1, 3) == nativePaid && runtime.Accept(Current(), 1, 3) == nativePaid,
            "A real paid provider can commit its explicit original non-top request without replacing the provider frame.");
        execution.PrepareAcceptedUse(Current(), nativePaid);
        Need(frames.Top?.Id == provider.Id && Current().AssistedSlashRequest is null && Current().ChoiceBindings.Count == 0,
            "Preparing a native-instruction return changes only its actual original frame, without manufacturing a choice result or replacing the paid provider.");
        facts.CanInvoke = false; players.At(0).IsAlive = false;
        Need(runtime.CanExecuteAcceptedUse(Current(), nativePaid), "An already prepared native use retains its issued permission after later source loss or death.");
        execution.PrepareAcceptedUse(Current(), nativePaid);
        var use = new CardUseFrame(frames.NextId(), 1, 0, CardKind.Slash, [3]);
        frames.Push(use);
        var bound = execution.BindExecution(use, nativePaid);
        Need(bound.ProgramSlashRequestExecution is { } receipt && receipt.ProgramFrameId == original.Id &&
            receipt.UseFrameId == use.Id && receipt.CardActionId is null && receipt.Acceptance == nativePaid &&
            frames.ParentOf(use.Id)?.Id == provider.Id && Current().SlashRequest!.ExecutionFrameId == use.Id &&
            execution.BindExecution(bound, nativePaid).ProgramSlashRequestExecution == receipt,
            "Execution references the real native use and nullable action, while the paid provider keeps its real physical parent and typed return.");
        Reject(() => runtime.ReleaseAcceptance(Current(), nativePaid));
        Reject(() => execution.BindExecution(bound, nativePaid, provider.Id));
        var duplicate = new CardUseFrame(frames.NextId(), 1, 0, CardKind.Slash, [3]);
        frames.Push(duplicate);
        Reject(() => execution.BindExecution(duplicate, nativePaid));
        frames.Pop(duplicate.Id, ResolutionFrameKind.CardUse);
        frames.Pop(use.Id, ResolutionFrameKind.CardUse);
        frames.Pop(provider.Id, ResolutionFrameKind.ProgramSkill);

        players.At(0).IsAlive = true; facts.CanInvoke = true;
        frames.Replace(original.Id, Current() with { InstructionIndex = 3,
            ChoiceBindings = [new ProgramChoiceResultBinding("completed-choice", "used-slash", 1)] });
        runtime.Issue(Current(), 1, new(ProgramSlashRequestTargetSource.FrozenCandidates, [3],
            ResultBind: "completed-choice", ReturnProtocol: ProgramSlashRequestReturnProtocol.ChoiceResult));
        var choicePaid = runtime.Accept(Current(), 1, 3);
        execution.PrepareAcceptedUse(Current(), choicePaid);
        execution.PrepareAcceptedUse(Current(), choicePaid);
        Need(Current().ChoiceBindings is [{ OptionId: "used-slash" }],
            "A physical issuer's already committed named choice is reused by the shared execution protocol without another result row.");
        frames.Replace(original.Id, Current() with { InstructionIndex = 4, FixedTargetSlash = new(3, false) });
        runtime.Issue(Current(), 1, new(ProgramSlashRequestTargetSource.FrozenCandidates, [3],
            ReturnProtocol: ProgramSlashRequestReturnProtocol.FixedTarget));
        var fixedPaid = runtime.Accept(Current(), 1, 3);
        execution.PrepareAcceptedUse(Current(), fixedPaid);
        execution.PrepareAcceptedUse(Current(), fixedPaid);
        Need(Current().FixedTargetSlash is { TargetSeat: 3, Issued: true },
            "The fixed native return retains its real draft and idempotently marks its issued state.");
        Reject(() => runtime.ReleaseAcceptance(Current(), fixedPaid));
        frames.Replace(original.Id, Current() with { InstructionIndex = 5 });
        runtime.Issue(Current(), 1, new(ProgramSlashRequestTargetSource.FrozenCandidates, [3, 4]));
        var failedAttempt = runtime.Accept(Current(), 1, 4);
        Need(runtime.ReleaseAcceptance(Current(), failedAttempt) && runtime.GetOffer(Current(), 1) is not null &&
            !runtime.OwnsAcceptance(Current(), failedAttempt, 1, 4),
            "A real unpaid-use failure releases its payment attempt while retaining the same live request and selected target.");
        var retriedAttempt = runtime.Accept(Current(), 1, 4);
        Need(retriedAttempt.AttemptOrdinal > failedAttempt.AttemptOrdinal &&
            runtime.OwnsAcceptance(Current(), retriedAttempt, 1, 4) && !runtime.OwnsAcceptance(Current(), failedAttempt, 1, 4) &&
            !runtime.ReleaseAcceptance(Current(), failedAttempt),
            "A previous payment receipt cannot release or borrow a later attempt with the same actor and target.");
        runtime.ReleaseAcceptance(Current(), retriedAttempt);
        Need(runtime.GetTargets(Current(), 1).SequenceEqual([3, 4]) && runtime.Accept(Current(), 1, 3).TargetSeat == 3,
            "A payment failure does not turn its selected payment target into a separate preselection that locks later attempts.");
        frames.Replace(original.Id, Current() with { InstructionIndex = 6 });
        runtime.Issue(Current(), 1, new(ProgramSlashRequestTargetSource.FrozenCandidates, [3], RetainAcceptedSourceLoss: true));
        var retainedSource = runtime.Accept(Current(), 1, 3);
        facts.CanInvoke = false;
        Need(runtime.CanExecuteAcceptedUse(Current(), retainedSource), "An accepted continuation can explicitly retain source loss without retaining a new offer.");
        players.At(0).IsAlive = false;
        Need(runtime.OwnsAcceptance(Current(), retainedSource, 1, 3) && !runtime.CanExecuteAcceptedUse(Current(), retainedSource),
            "Accepted source-loss permission does not implicitly retain the source owner's death.");
        runtime.Close(Current());
        facts.CanInvoke = true; players.At(0).IsAlive = true;
        runtime.Issue(Current(), 1, new(ProgramSlashRequestTargetSource.FrozenCandidates, [3],
            RetainAcceptedSourceLoss: true, RetainAcceptedOwnerDeath: true), 1);
        var retainedPaid = runtime.Accept(Current(), 1, 3);
        facts.CanInvoke = false; players.At(0).IsAlive = false;
        Need(runtime.CanExecuteAcceptedUse(Current(), retainedPaid) &&
            JsonSerializer.Deserialize<ProgramSlashRequestAcceptance>(JsonSerializer.Serialize(retainedPaid)) == retainedPaid,
            "An explicitly paid continuation retains both declared execution permissions and their immutable value through cold data.");
        runtime.ReleaseAcceptance(Current(), retainedPaid);
        Need(runtime.GetOffer(Current()) is null, "Accepted-use retention does not confer source-loss or owner-death permission on the next unpaid offer.");
        facts.CanInvoke = true; players.At(0).IsAlive = true;
        frames.Replace(original.Id, Current() with { InstructionIndex = 7 });
        runtime.Issue(Current(), 1, new(ProgramSlashRequestTargetSource.FrozenCandidates, [3, 4],
            FailureReturnPolicy: ProgramSlashRequestFailureReturnPolicy.ResumeOriginalInstruction));
        var noUse = runtime.Accept(Current(), 1, 3);
        var paidProducer = new ProgramSkillFrame(frames.NextId(), 1, "fixture:no-use-payment", "pay", "fixture:hash", 1, [], []);
        frames.Push(paidProducer); frames.Pop(paidProducer.Id, ResolutionFrameKind.ProgramSkill);
        var failure = runtime.FailAcceptedUse(Current(), noUse, paidProducer.Id);
        Need(runtime.FailAcceptedUse(Current(), noUse, paidProducer.Id) == failure && runtime.OwnsAcceptance(Current(), noUse, 1, 3) &&
            !runtime.CanExecuteAcceptedUse(Current(), noUse) && !execution.ResumeFailedRequest(original.Id) &&
            JsonSerializer.Deserialize<ProgramSlashRequestFailure>(JsonSerializer.Serialize(failure)) == failure,
            "A paid producer's no-use return is immutable and idempotent; it preserves payment ownership without authorizing a use or stealing the compound owner's return.");
        Reject(() => runtime.ReleaseAcceptance(Current(), noUse));
        Need(runtime.TakeFailure(Current()) == failure && runtime.TakeFailure(Current()) is null &&
            !runtime.CanExecuteAcceptedUse(Current(), noUse) && runtime.ReleaseAcceptance(Current(), noUse),
            "Only the original owner consumes failure once and may then release that failed attempt for a real retry.");
        var afterFailure = runtime.Accept(Current(), 1, 4);
        Need(afterFailure.AttemptOrdinal > noUse.AttemptOrdinal && !runtime.OwnsAcceptance(Current(), noUse, 1, 3),
            "A resumed opportunity never lets the failed payment receipt borrow its next accepted attempt.");
        execution.PrepareAcceptedUse(Current(), afterFailure);
        Reject(() => runtime.FailAcceptedUse(Current(), afterFailure, paidProducer.Id));

        frames.Replace(original.Id, Current() with { InstructionIndex = 8 });
        var slotIdentity = runtime.Issue(Current(), 1, new(ProgramSlashRequestTargetSource.FrozenCandidates, [3],
            FailureReturnPolicy: ProgramSlashRequestFailureReturnPolicy.ResumeOriginalInstruction));
        players.At(3).IsAlive = false;
        var slotProducer = new ProgramSkillFrame(frames.NextId(), 1, "fixture:paid-slot", "pay", "fixture:hash", 1, [], []);
        frames.Push(slotProducer); frames.Pop(slotProducer.Id, ResolutionFrameKind.ProgramSkill);
        var slotFailure = runtime.FailIssuedUse(Current(), slotIdentity, 1, 3, slotProducer.Id);
        Need(runtime.OwnsAcceptance(Current(), slotFailure.Acceptance, 1, 3) && !runtime.CanExecuteAcceptedUse(Current(), slotFailure.Acceptance) &&
            Current().SlashRequest is { PreparingNativeUse: false, ExecutionFrameId: null },
            "A target lost after a genuine producer payment creates only a failed return, without a usable acceptance or a fabricated native use.");
        Need(runtime.TakeFailure(Current()) == slotFailure, "A paid slot no-use return follows the same single owner consumption protocol.");
        players.At(3).IsAlive = true;

        frames.Replace(original.Id, Current() with { InstructionIndex = 9, FixedTargetSlash = new(3, false) });
        runtime.Issue(Current(), 1, new(ProgramSlashRequestTargetSource.FrozenCandidates, [3],
            ReturnProtocol: ProgramSlashRequestReturnProtocol.FixedTarget,
            FailureReturnPolicy: ProgramSlashRequestFailureReturnPolicy.ResumeOriginalInstruction));
        var fixedFailure = runtime.Accept(Current(), 1, 3);
        runtime.FailAcceptedUse(Current(), fixedFailure, slotProducer.Id);
        var handoff = new RecordingAdvanceFrames(frames);
        var failureExecution = new NativeProgramSlashRequestExecutionRuntime(handoff, runtime,
            CardGame.Core.RuntimeEntries.CoreGameEngineProgramSlashRequestEntries.Create(() =>
                game.GetRuntimeModule<CardGame.Core.EngineOwners.CoreGameEngineProgramSlashRequestRuntime>()),
            capabilities.Get<IProgramRuntimePrompts>());
        Need(failureExecution.ResumeFailedRequest(original.Id) && Current().FixedTargetSlash is null &&
            Current().SlashRequest is { Closed: true, Failure.Consumed: true } && handoff.ProgramAdvances == 1 &&
            !failureExecution.ResumeFailedRequest(original.Id) && handoff.ProgramAdvances == 1,
            "The common fixed return consumes once, closes its draft and hands the unchanged cursor to the ordinary executor without inventing a successful use.");
        frames.Replace(original.Id, Current() with { InstructionIndex = 10 });
        var closedIdentity = runtime.Issue(Current(), 1, new(ProgramSlashRequestTargetSource.FrozenCandidates, [3]));
        var closedAuthorization = new ProgramAssistedSlashAuthorization(Current().InstructionIndex)
        { RequestIdentity = closedIdentity, TargetSeat = 3 };
        runtime.Close(Current());
        Reject(() => runtime.AcceptIssued(Current(), closedIdentity, 1, 3));
        Reject(() => runtime.AcceptIssued(Current(), closedAuthorization, 1, 3));
        Need(runtime.GetIdentity(Current()) == closedIdentity && Current().SlashRequest!.Acceptance is null &&
            !runtime.CanExecuteAcceptedUse(Current(), new(closedIdentity, 3)),
            "A closed unpaid identity remains an issued fact but cannot be reused to create a new accepted or executable use.");
        CheckPermissionScopes(frames, players, runtime, execution, facts, original.Id);
    }

    private static void CheckPermissionScopes(IProgramRuntimeFrames frames, IMatchParticipantStateAccess players,
        NativeProgramSlashRequestRuntime requests, NativeProgramSlashRequestExecutionRuntime execution, Facts facts, long originalId)
    {
        facts.CanInvoke = true; players.At(0).IsAlive = true;
        ProgramSkillFrame Current() => (ProgramSkillFrame)frames.Find(originalId)!;
        var permissions = new NativeProgramSlashRequestPermissionsRuntime(frames, requests);
        ProgramSlashQuotaQuery Quota(int actor, int target) => new(players.At(actor), players.At(target), CardKind.Slash,
            null, null, null, null, null, false);
        ProgramSlashDistanceQuery Distance(int actor, int target, long? useId = null) =>
            new(players.At(actor), players.At(target), CardKind.Slash, ExistingUseFrameId: useId);
        frames.Replace(originalId, Current() with { InstructionIndex = 11 });
        requests.Issue(Current(), 1, new(ProgramSlashRequestTargetSource.FrozenCandidates, [3], IgnoreDistance: true));
        Need(permissions.Declaration(1) is not null && permissions.Declaration(0) is null &&
            permissions.RequestedPhysicalSlashDistance(Distance(1, 3)) && !permissions.RequestedSlashDistance(Distance(1, 4)) &&
            permissions.RequestedSlashQuota(Quota(1, 3)) && !permissions.RequestedSlashQuota(Quota(1, 4)),
            "The actual top request grants only its requested distance and quota targets to its own actor.");
        var accepted = requests.Accept(Current(), 1, 3);
        Need(permissions.Declaration(1) == accepted.Identity, "The exact accepted-before-use declaration remains readable at its original program window.");
        var provider = new ProgramSkillFrame(frames.NextId(), 1, "fixture:declaration-provider", "provide", "fixture:hash", 1, [], []);
        frames.Push(provider);
        Need(permissions.Declaration(1) is null && !permissions.RequestedSlashDistance(Distance(1, 3)),
            "An unrelated top frame cannot borrow a request by finding a character-marked ancestor.");
        var declarationId = frames.NextId();
        var parent = new CardDeclarationParentReturn(CardDeclarationPurpose.Use, originalId, ResolutionFrameKind.ProgramSkill,
            ResolutionFrameStep.AwaitingResponse, 1, TargetSeat: 3);
        var declaration = new CardDeclarationFrame(declarationId, 1, 1,
            new(declarationId, originalId, 1, new(901, CardKind.Slash, CardLocation.Hand(1)),
                new("fixture:declaration", "use", 1, "fixture:declared-instance"), CardKind.Slash), parent, [3]);
        frames.Push(declaration);
        Need(permissions.Declaration(1) is null, "A native declaration's parent id alone does not authorize another request.");
        frames.Replace(declaration.Id, declaration with { Return = parent with { ProgramSlashRequest = accepted } });
        Need(permissions.Declaration(1) == accepted.Identity && permissions.Declaration(0) is null &&
            permissions.RequestedCardDistance(new(players.At(1), players.At(3), CardKind.Slash, null, null, null)),
            "Only the declaration's explicit accepted return carries the same original scope into its current native window.");
        frames.Pop(declaration.Id, ResolutionFrameKind.CardDeclaration);
        frames.Pop(provider.Id, ResolutionFrameKind.ProgramSkill);
        execution.PrepareAcceptedUse(Current(), accepted);
        var first = new CardUseFrame(frames.NextId(), 1, 0, CardKind.Slash, [3]);
        frames.Push(first); execution.BindExecution(first, accepted);
        Need(!permissions.RequestedPhysicalSlashDistance(Distance(1, 3, first.Id)) && !permissions.IssuedSlashDebitExemption(first.Id, 1),
            "Prospective distance does not silently become an issued-use privilege or a debit exemption.");
        frames.Pop(first.Id, ResolutionFrameKind.CardUse);

        requests.Issue(Current(), 1, new(ProgramSlashRequestTargetSource.FrozenCandidates, [3], IgnoreDistance: true,
            QuotaTargets: ProgramSlashRequestPermissionTargets.AnyOtherTarget,
            IssuedPhysicalSlashDistanceTargets: ProgramSlashRequestPermissionTargets.AnyOtherTarget), 1);
        Need(permissions.RequestedSlashQuota(Quota(1, 4)) && !permissions.RequestedSlashQuota(Quota(1, 1)) &&
            !permissions.RequestedSlashDistance(Distance(1, 4)),
            "All-other-target quota permission stays separate from the current request's narrower prospective distance.");
        var allTargets = requests.Accept(Current(), 1, 3);
        execution.PrepareAcceptedUse(Current(), allTargets);
        var second = new CardUseFrame(frames.NextId(), 1, 0, CardKind.Slash, [3]);
        frames.Push(second); execution.BindExecution(second, allTargets);
        Need(permissions.RequestedPhysicalSlashDistance(Distance(1, 4, second.Id)) &&
            !permissions.RequestedPhysicalSlashDistance(Distance(1, 1, second.Id)) && !permissions.IssuedSlashDebitExemption(second.Id, 1),
            "An explicit issued distance audience applies to its original use without implying an actual debit exemption.");
        Need(!permissions.RequestedSlashDistance(Distance(1, 4, second.Id)) &&
            !permissions.RequestedCardDistance(new(players.At(1), players.At(4), CardKind.Slash, second.Id, null, null)),
            "An issued physical-Slash distance right does not acquire virtual or generic card-distance query authority.");
        frames.Pop(second.Id, ResolutionFrameKind.CardUse);

        requests.Issue(Current(), 1, new(ProgramSlashRequestTargetSource.FrozenCandidates, [3], IgnoreDistance: true,
            CountsTowardSlashLimit: true, QuotaTargets: ProgramSlashRequestPermissionTargets.AnyOtherTarget,
            IssuedPhysicalSlashDistanceTargets: ProgramSlashRequestPermissionTargets.AnyOtherTarget, ExemptIssuedSlashDebit: true), 2);
        Need(!permissions.RequestedSlashQuota(Quota(1, 3)), "A counted candidate does not borrow its later issued debit exemption as quota admission.");
        var debit = requests.Accept(Current(), 1, 3);
        execution.PrepareAcceptedUse(Current(), debit);
        var third = new CardUseFrame(frames.NextId(), 1, 0, CardKind.Slash, [3]);
        frames.Push(third);
        var captured = execution.BindExecution(third, debit);
        facts.CanInvoke = false; players.At(0).IsAlive = false;
        Need(permissions.RequestedPhysicalSlashDistance(Distance(1, 4, third.Id)) && permissions.IssuedSlashDebitExemption(third.Id, 1),
            "Issued distance and debit rights read the native execution even after its source is lost or dies.");
        frames.Pop(third.Id, ResolutionFrameKind.CardUse);
        frames.Pop(originalId, ResolutionFrameKind.ProgramSkill);
        frames.Push(captured);
        Need(permissions.Declaration(1) is null && permissions.RequestedPhysicalSlashDistance(Distance(1, 4, third.Id)) &&
            permissions.IssuedSlashDebitExemption(third.Id, 1) && !permissions.RequestedPhysicalSlashDistance(Distance(0, 4, third.Id)) &&
            !permissions.IssuedSlashDebitExemption(third.Id, 0),
            "A native use keeps its captured rights after the original program disappears; another actor cannot borrow them.");
        frames.Pop(third.Id, ResolutionFrameKind.CardUse);
    }

    private sealed class Facts : IProgramSlashRequestFacts
    {
        internal bool CanInvoke { get; set; } = true;
        internal Dictionary<int, int> Distances { get; } = [];
        public bool CanInvokeProgram(ProgramSkillFrame frame) => CanInvoke;
        public int GetCombatDistance(int actorSeat, int targetSeat) => Distances.GetValueOrDefault(targetSeat, 1);
    }
    private sealed class RecordingAdvanceFrames(IProgramRuntimeFrames inner) : IProgramRuntimeFrames
    {
        internal int ProgramAdvances { get; private set; }
        public ResolutionFrame? Top => inner.Top;
        public ResolutionFrame? Find(long id) => inner.Find(id);
        public ResolutionFrame? ParentOf(long id) => inner.ParentOf(id);
        public ResolutionFrame? ChildOf(long id) => inner.ChildOf(id);
        public IReadOnlyList<T> OfType<T>() where T : ResolutionFrame => inner.OfType<T>();
        public ProgramSkillFrame ActiveProgram(long id) => inner.ActiveProgram(id);
        public long NextId() => inner.NextId();
        public void ReplaceTop(ResolutionFrame frame) => inner.ReplaceTop(frame);
        public void Replace(long id, ResolutionFrame frame) => inner.Replace(id, frame);
        public void Push(ResolutionFrame frame) => inner.Push(frame);
        public void Pop(long id, ResolutionFrameKind kind) => inner.Pop(id, kind);
        public void Advance(long id) => inner.Advance(id);
        public void AdvanceTop<T>() where T : ResolutionFrame => inner.AdvanceTop<T>();
        public void AdvanceProgram(long id) => ProgramAdvances++;
    }
    private sealed class Ranges : IAttackRangeQueries
    {
        internal HashSet<(int Actor, int Target)> Targets { get; } = [];
        public bool HasRelation(int actor, int target) => false;
        public bool IsWithinAttackRange(int actor, int target, int? excludedEquipmentId = null, EquipmentSlot? excludedEquipmentSlot = null) =>
            Targets.Contains((actor, target));
    }
    private static void Need(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
    private static void Reject(Action action)
    { try { action(); } catch (InvalidOperationException) { return; } throw new InvalidOperationException("An invalid original Slash request was accepted."); }
}
