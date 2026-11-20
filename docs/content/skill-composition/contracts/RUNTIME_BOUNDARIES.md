# Runtime boundaries

Required when changing these mechanisms or their consumers. See the
[contract index](README.md) for related topics.

## Validation and dependency direction

- Validate immutable skill definition shape in the loader, and reuse the executor's content-hash guard. Avoid repeating full static definition checks at each runtime continuation. Preserve dynamic payment, owner/frame identity, card movement, command validation, privacy, and save/replay contracts. Reuse stable integration and verification tools instead of creating a new family of scripts per general, and run routine verification once per integrated batch.

- Invariants belong at shared boundaries: command validation, the loader, checkpoint restore and the executor. Character runtime code trusts the executor, frame cursor, typed child returns and payment receipts; it does not re-prove them with private events, history scans, `*Paid`/`*Proof` methods or `InvalidOperationException` sweeps. Do not record a private event for a fact the engine already records; read the existing record, adding one shared read if it is missing.

- Dependency direction: shared modules depend on shared capabilities; character implementations depend on shared capabilities and do not depend on other character implementations. Entries and per-target injection are transitional wiring, not capability boundaries. Do not hide reverse dependencies in large context objects, service locators or generic query interfaces, and do not optimize constructor length independently of removing reverse dependencies.

- The existing runtime-composition check locks current constructor-direction violations to an exact baseline: new violations fail, and removed violations require deleting their baseline entries. A passing legacy baseline does not complete a concept. From the use/response legality batches onward, completion also requires the concept's shared modules to have no concrete character dependencies.

## Command commits and public projections

- Keep command commit and recovery in the command session. Prepare player views before commit; freeze all exposed nested collections so observers cannot change the prepared result. Player views must use `CreateSnapshot(viewerSeat)`; `ResolutionStack` is trusted diagnostics and may contain private state.

- Freeze collection-bearing event payloads in commit preparation before they enter history or reach observers. A read-only record does not freeze its lists or nested policy/deposit objects; new built-in event collections must be covered by `CommittedEventProjection`.

- New rule events must use `AdvanceEventRulesAndQueueFact`; new state progression must use `AdvanceRulesAndPublishState`. `QueueGameEvent` and `PublishState` are output-only helpers and must not become rule entry points again.

## Owning frames, delivery and invocation

- Keep pending resolution state on its owning frame. Use runtime push/replace/complete methods and typed parent returns; do not add parallel pending objects or use-ID sidecars for migrated state. A resumed child must not pay a completed cost twice.

- Delivered native Draw and awaited bound-gift instructions use the original frame's instruction delivery to drain their own children and finish their terminal instruction after source loss or death. Keep delivery on the original frame, including zero-card or legally replaced results; delivery never authorizes an unpaid suffix or an undelivered request. Preserve the synchronous path's existing contract when no native producer is used.

- `NativeDrawProducerRuntime` owns the program Draw cursor for both the ordinary `draw` step and the `IProgramOperationDraws` entry. The step dispatches from the active program frame. A typed operation reads the outcome on that frame. Phase draw counts and native card-effect amounts have their own producers.

- `MatchTurnNumericQueries` evaluates the actual Draw phase amount. Keep fixed card and skill draw amounts at their producers. Declared modifiers and runtime owners contribute to the same numeric evaluation. An `AppendNumericRuleContributions` callback reads the requested seat and numeric query and appends its terms. The original owners issue, consume and expire their grants. Gangshu retains its phase eligibility and expires a used unit on completion of that actual Draw phase, or earlier on a qualifying offset.

- Accepted card-action and movement trigger continuations use `ProgramInvocations` on the original native window and candidate cursor. Acceptance-scoped rights begin only after the actual binding starts; preserve earlier source-frozen permissions as distinct declarations. Keep source-loss invocation rights separate from owner-death rights; never refresh accepted qualification from later use history or recreate character payment-proof ledgers.

- Accepted materials are commitments on their original frame, separately from chosen inputs and physical payment results. Acceptance freezes eligibility; execution drops materials that left their frozen source without requalifying the promise. Declare source-loss continuation separately from owner-death draining, and never use draining to issue future effects.

- Trigger usage keys and consumption use `ProgramTriggerUsageRuntime` / `ProgramTriggerUsageRules` over the existing skill store and native Event window. Issued opportunities reserve declared limits once and retain that reservation on the original invocation; beginning the binding does not pay it again. Freeze qualification facts privately on the owning frame, keep source loss and owner death independent, and identify native activation sources from actual activations rather than unrelated reward triggers.

- Draw phase start and end windows resume a selected choice group at the original index after all group members. A single candidate advances one position. A completed native draw replacement keeps its phase-specific completion behavior.

- Automatic and manual hand-limit discard finish through `CoreActualDiscardPhaseRuntime.FinishDiscardPhase`. It confirms the actual discard phase and opens the original end window when eligible. Synchronous skip decisions use `EndTurnAfterDiscardSkip` to record the skip and continue directly to turn ending. Qiao Zhou qualifies before Du Xi. Delayed and already-issued skip windows retain their typed return. Both ordinary completion and completed skips enter the same native turn ending.

- Native multi-stage instructions reuse `ProgramStageRuntime`: declare the next stage before issuing a paid child, reread the original receipt after draining children, and keep dynamic termination rules in the recipe. Ordered random acquisition from other seats uses `ProgramRandomSeatAcquisitionRuntime`; version policies retain selection priority, turnover rules and event projection without repeating the movement loop or scanning history to prove completed payments.

## Selections and expression bindings

- Expression card sets carry only known frozen facts and original physical residency. Derived bindings inherit source locations and entry tokens, including unknown tokens, rather than sampling a new origin; use the shared zone-entry query. Accepted after-turn-ended calls retain only their declared original-frame invocation permission, independently of owner-death draining.

- Coupled target and card-payment constraints share one material-target decision for qualification, prompts and submission. Compile their instruction relationship from rule data, retain selection and paid children on the original frame, and use shared native legality; do not rediscover the relationship through character callbacks, ancestor searches or repeated effect scans.

## Native fact demand

- Declare native fact demand at the capability definition; shared capture must not enumerate character capability flags. Keep trigger and activation demand distinct. Compile operation/passive declarations, generic traits, expressions and windows; every lifetime-bearing operation exposes its real Until through descriptor `LifetimeBoundaries` metadata and the single `ProgramRuleLifetimeFacts` mapper, including passive declaration `State.Until`. Direct status reads declare their own demand instead of manufacturing an Until. Compose immutable catalog facts with the selected mode RequiredFacts in the match provider, never by mutating the shared catalog. Shared capture gates read the same match INativeFactDemand mask; ActualPhaseEnd implies actual-phase identity, and both mode contracts and compiled masks reject unknown requirements.

- Fact demand gates capture, not rule qualification or observer scheduling. Preserve specialized capture, OwnedDraw producer admission and actual-skip timing; a mode phase requirement must not create extra DrawSkipped observer frames. Keep issued phase-end collectors and typed single-candidate returns explicit until their rules are actually declared; removing demand branches does not complete that execution concept.

## Catalog discovery

- Runtime catalog discovery may filter attribute presence before sorting, while retaining each owner's null, inheritance, signature, duplicate and malformed-registration boundaries. Class markers are not inherited; method discovery preserves inherited attributes. Composite catalogs keep complete input for their independent child catalogs, and native named-return resolution still inspects the full declaring type. Presence filtering must not become a signature, namespace or host whitelist that silently skips invalid marked declarations.

- Built-in catalog-validator discovery may read a matching module's PE metadata through `ProgramCatalogValidationMethodDiscovery`. It selects direct marker declarations plus all virtual methods so inherited declarations still reach the original `Discover` validation. Preserve the original type/method order and generic definitions; unsupported images, unavailable locations or mismatched MVIDs fall back to the original reflection path as a whole. PE indexing never validates skill rules or swallows signature, duplicate or validator failures.
