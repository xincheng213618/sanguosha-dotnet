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

- `MatchTurnNumericQueries` evaluates the actual Draw phase amount. It starts from the mode amount and each effective printed equipment term. Keep fixed card and skill draw amounts at their producers. Declared modifiers and runtime owners contribute to the same numeric evaluation. The ordinary declaration path reads the active skill bindings indexed for the participant and query. `AppendNumericRuleContributions` walks the ordered list of registered runtime callbacks. Each callback decides whether its own current state or issued grants contribute for the requested seat and query. The original owners issue, consume and expire their grants. Gangshu retains its phase eligibility and expires a used unit on completion of that actual Draw phase, or earlier on a qualifying offset. Kongwu contributes a penalty for each original debt bound to the participant's current actual turn.

- `CoreProgramDistanceRulesRuntime` supplies distance callbacks with round and seat counts, active skills, physical hand and equipment identities, and typed event facts. Zhuitao reads its directed grants and revocations from the original journal in order for each seat pair.

- `CoreGameEngineNativeUsedDiscardOriginsRuntime` captures the actor and exact discard entry of finished uses and responses during the original movement batch. Xunxian and Tongxie request this fact through their trigger operations. Tongxie observes each queued fact synchronously and validates the original movement and live Slash parent before opening its own later follow-up window.

- `CoreGameEngineActualTurnLegacyCompletionRuntime` normalizes an original zero-entity program use when the actual-use ordinal or the content's native completion fact is required. Trigger operations, the Limu activation and the existing Leiluan conversion declaration contribute that fact at catalog compilation. The original producer frame and typed completion return still control admission and continuation.

- `CoreGameEngineTreatedWoundedRuntime` supplies current wounded status and identity concealment to conditional skill bindings and rule expressions. This uses the same match health rules as native checks, including undefined HP, the active Happy Qiao health pool and qualifying Guiming. Xingzhao reads that current status through its declared condition.

- `CoreGameEngineGameActivationAwakeningRuntime` reads Huishi's original awakening-override fact for native qualification and continuation. A live query matches the current skill instance and eligible binding. An issued query matches the earlier fact to the original frame, recipient, instance, binding and gameplay hash. Huishi still issues the grant through its own paid choice.
- Huishi filters previously accepted awakenings through the typed match journal. Choujue, Beishui and mobile Mou Zhiji identify their original seat and skill using the existing acceptance event. This evidence is available before the later completion and skill grant.
- The trigger usage runtime owns the replacement awakening use. The `awakening` game-scope key belongs to the seat and skill across bindings and instances. The executor consumes it after qualification and before the maximum-HP payment and replacement choice. Candidate checks and Huishi read that same use through the compiled replacement operation.
- Tianyi and Jianghuo share the natural damage check. Each currently living seat must have a positive `DamageAppliedEvent` targeting it in the committed or pending match journal. This includes positive damage absorbed by Armor and damage without a source. Each skill retains its own trigger window and checks the Huishi override first.

- `CoreGameEngineProgramAttackRangeAidRuntime` owns `requestAttackRangeAid` from the original responder list through weapon payment or the beneficiary's typed native Draw. It advances the current responder only after the original paid children return. Accepted payment and its recovery, damage, movement and skill-change children keep the original parent frame.

- Dingpan's native activation limit and public progress both read the same count of living Rebels through `IMatchRoleQueries`. The public progress retains the zero-limit entry. Target equipment eligibility and the accepted equipment choice still belong to Dingpan.

- Use a skill's `modifiers` declaration for ordinary additive values and conditions that its existing queries can express. For example, OL Huo Jun's Qiongshou declares `handLimit` with `operation: add` and `value: 4`. The native Discard phase uses the resulting limit when it requests real card payments.

- OL Du Fu Ren's Shunshi contributes to the shared Draw count, Slash limit and hand limit through the same registration. Each paid gift is consumed separately at the participant's next actual phase of the corresponding kind. Skipped phases leave that grant pending. A Draw-start instruction may read the pending Draw term before confirmation; confirmed queries read the terms issued for the original phase key.

- OL Chen Deng's Fengji contributes to the actual Draw count and Slash limit through the same numeric entry. Each current-round term reads its original choice and applies the accepted Xuanhui swaps for that round. The Slash query uses the ordinary native debit for each physical use.

- Mobile Boundary Yan Yan's Chengshi adds one to the participant's ordinary Slash limit for each grant Juzhan issues while the original Slash resolves. The grants accumulate across turns. The card claim waits for completion of the original Slash, and the directed card prohibition ends with its turn.

- Mou Lu Meng's Hengye reads the effective skill holder's game-scoped growth for Draw count, Slash limit, hand limit and attack range. The damage and kill paths own the capped growth and reset. Turn-start recovery still reads the same growth independently of these queries.

- Mobile Li Yan's Duliang contributes the original grants assigned to the beneficiary's current actual Draw phase. Separate grants can stack on the first actual Draw, including an inserted phase, and survive loss of the issuing skill. The next Draw phase reads its own assignments.

- Anniversary Hu Che Er's Fuzhong reads the effective skill holder's current Zhong count. At least one Zhong adds one card to the actual Draw count, and at least three Zhong add three to the hand limit checked in Discard. A fixed amount from a separate native draw producer uses its own amount.

- OL Du Xi's Quxi contributes to the actual Draw count of each living marker holder. Each current Feng adds one and each current Qian subtracts one. Drawing leaves the issued marks in place for subsequent Draw phases and for their original transfer windows.

- OL Gao Gan's Juguan settles retaliation from the original damaged targets at the start of the issuer's next actual turn. The qualified reward adds two to the first bound actual Draw phase for that living participant, including after loss of the skill. A Draw skip can expire that reward before it binds.

- OL Cheng Gong Ying's Kuangxiang reduces the living recipient's next actual Draw count by one per issued red card. Before confirmation the query reads pending grants. A confirmed Draw reads only the grants consumed on its original phase key. Skipped phases leave the grants pending.

- OL Cao Teng's Yongzu adds one to the original recipient's hand limit for each issued permanent grant. That grant survives loss or death of its source. Yongzu's temporary skill choice has its own expiry at the recipient's next actual turn start.

- OL Lu Yu Sheng's Runwei contributes the issued hand-limit adjustments for the current actual Discard phase and its participant. The Draw choice subtracts one and the discard choice adds one. Each adjustment stays on its original phase key after the choice resolves.

- Accepted card-action and movement trigger continuations use `ProgramInvocations` on the original native window and candidate cursor. Acceptance-scoped rights begin only after the actual binding starts; preserve earlier source-frozen permissions as distinct declarations. Keep source-loss invocation rights separate from owner-death rights; never refresh accepted qualification from later use history or recreate character payment-proof ledgers.

- Accepted materials are commitments on their original frame, separately from chosen inputs and physical payment results. Acceptance freezes eligibility; execution drops materials that left their frozen source without requalifying the promise. Declare source-loss continuation separately from owner-death draining, and never use draining to issue future effects.

- Trigger usage keys and consumption use `ProgramTriggerUsageRuntime` / `ProgramTriggerUsageRules` over the existing skill store and native Event window. Issued opportunities reserve declared limits once and retain that reservation on the original invocation; beginning the binding does not pay it again. Freeze qualification facts privately on the owning frame, keep source loss and owner death independent, and identify native activation sources from actual activations rather than unrelated reward triggers.

- Draw phase start and end windows resume a selected choice group at the original index after all group members. A single candidate advances one position. A completed native draw replacement keeps its phase-specific completion behavior.

- Automatic and manual hand-limit discard finish through `CoreActualDiscardPhaseRuntime.FinishDiscardPhase`. It confirms the actual discard phase and opens the original end window when eligible. Synchronous skip decisions use `EndTurnAfterDiscardSkip` to record the skip and continue directly to turn ending. Qiao Zhou qualifies before Du Xi. Delayed and already-issued skip windows retain their typed return. Both ordinary completion and completed skips enter the same native turn ending.

- `CoreActualDiscardPhaseRuntime` opens `beforePhaseEntry` discard policy choices while the actual turn is still in its preceding phase. The original `NativeDiscardEntryFrame` validates the current binding before acceptance and retains the accepted choice through its invocation child, including after loss of the originating skill. Accepting records the skip and continues to turn ending. Declining resumes the original Discard entry.

- Native multi-stage instructions reuse `ProgramStageRuntime`: declare the next stage before issuing a paid child, reread the original receipt after draining children, and keep dynamic termination rules in the recipe. Ordered random acquisition from other seats uses `ProgramRandomSeatAcquisitionRuntime`; version policies retain selection priority, turnover rules and event projection without repeating the movement loop or scanning history to prove completed payments.

- Native program execution offers `TryBeginProgramInvocation` before the resume catalog and before direct execution. A registered observer can start an invocation on the original program frame. Its issuing qualification and typed return stay with the observer. The resume catalog and step executor retain their existing entry points.

- Native equipment invocations capture the first effective physical card of the matching printed kind from field equipment. An accepted invocation can record equipment ID `0` when no matching physical card is present, including virtual equipment and a weapon already spent on the original payment. The original native parent owns the frozen receipt, fingerprint and typed return. The equipment entity ID is independent of the invocation's skill instance ID.

## Selections and expression bindings

- The shared Pindian policy checks whether a living participant can discard to use a maximum-rank deck card. It reads active policy bindings and physical discardable cards, excluding equipment that grants the qualifying binding. The same read serves participant legality, private selection and validation. The input query accepts a current hand card or that discard alternative. Call sites separately check living participants and target eligibility. The original Pindian window owns payment, deck reservation, reveal and cleanup.
- The native Pindian target query enumerates living eligible opponents in seat order after confirming the source has an input. Kuangzhan uses the same list when it offers opponents and when it opens the selected native contest.
- Pindian selection uses the original activation shape to distinguish a selected hand source from other card requirements. The zero-card offer requires a live discard alternative. The original activation then records Shenduan's exact binding and actual Play phase. Execution reads that admission from the owning frame before opening the native contest and paying its physical input.

- Expression card sets carry only known frozen facts and original physical residency. Derived bindings inherit source locations and entry tokens, including unknown tokens, rather than sampling a new origin; use the shared zone-entry query. Accepted after-turn-ended calls retain only their declared original-frame invocation permission, independently of owner-death draining.

- Coupled target and card-payment constraints share one material-target decision for qualification, prompts and submission. Compile their instruction relationship from rule data, retain selection and paid children on the original frame, and use shared native legality; do not rediscover the relationship through character callbacks, ancestor searches or repeated effect scans.

## Native fact demand

- Declare native fact demand at the capability definition; shared capture must not enumerate character capability flags. Keep trigger and activation demand distinct. Compile operation/passive declarations, generic traits, expressions and windows; every lifetime-bearing operation exposes its real Until through descriptor `LifetimeBoundaries` metadata and the single `ProgramRuleLifetimeFacts` mapper, including passive declaration `State.Until`. Direct status reads declare their own demand instead of manufacturing an Until. Compose immutable catalog facts with the selected mode RequiredFacts in the match provider, never by mutating the shared catalog. Shared capture gates read the same match INativeFactDemand mask; ActualPhaseEnd implies actual-phase identity, and both mode contracts and compiled masks reject unknown requirements.

- Fact demand gates capture, not rule qualification or observer scheduling. Preserve specialized capture, OwnedDraw producer admission and actual-skip timing; a mode phase requirement must not create extra DrawSkipped observer frames. Keep issued phase-end collectors and typed single-candidate returns explicit until their rules are actually declared; removing demand branches does not complete that execution concept.

## Catalog discovery

- Runtime catalog discovery may filter attribute presence before sorting, while retaining each owner's null, inheritance, signature, duplicate and malformed-registration boundaries. Class markers are not inherited; method discovery preserves inherited attributes. Composite catalogs keep complete input for their independent child catalogs, and native named-return resolution still inspects the full declaring type. Presence filtering must not become a signature, namespace or host whitelist that silently skips invalid marked declarations.

- Built-in catalog-validator discovery may read a matching module's PE metadata through `ProgramCatalogValidationMethodDiscovery`. It selects direct marker declarations plus all virtual methods so inherited declarations still reach the original `Discover` validation. Preserve the original type/method order and generic definitions; unsupported images, unavailable locations or mismatched MVIDs fall back to the original reflection path as a whole. PE indexing never validates skill rules or swallows signature, duplicate or validator failures.
