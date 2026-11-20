# Skills and applied states

Required when changing these mechanisms or their consumers. See the
[contract index](README.md) for related topics.

## Skill changes and availability

- Skill upgrades, persistent upgrade flags, and skill grants/replacements reuse `Runtime/Rules/ProgramSkillChangeRuntime.cs` and the frame-aware helpers in `EngineOwners/CoreGameEngineProgramSkillChangesRuntime.ProgramSkillChanges.cs` through the existing program nodes. Keep rule-specific trigger, cost, and phase qualification outside the shared mutation layer; do not add a turn/round prerequisite to skill changes themselves. Keep paid child state and returns on the original frame. New generals that reuse these operations inherit the shared `Skill changes:` checks instead of copying an upgrade implementation or fixture.

- Ordinary skill suppression, loss and borrowing use `skillState` / `ProgramSkillChangeRuntime`. `SkillAvailability` answers possession separately from effectiveness, and `MatchSkillBindingIndex` caches that answer. Invocation permission belongs to its actual window. Preserve distinct lifetimes, continuous predicates and invocation-scoped rules during migration. Record remaining exceptions in this topic. Keep a single shared validity answer and avoid extending legacy character-specific state stores.

- Skill validity must be derived from declared data in one owner. Do not replace character qualification callbacks with a predicate registry, interface list or new callbacks. Store restrictions separately from real skill grants, use one matching function, and keep window-scoped invocation authorization separate from possession and effectiveness.

- Raw skill-source admission uses `SkillAvailability` for selection, reveal, identity concealment and immutable mode content limits; preserve possession separately from effective admission. Applied equipment-skill restrictions use the same recipient restriction table and metadata matcher, including future equipment grants; real equipment-effect and draw-penalty rules keep their own facts.

- Source-dependent skill grants use declared `skillGrants` conditions, exact `ConditionalSource` relations and `ConditionalSkillGrantRuntime`; `SkillAvailability` interprets their eligibility for both reads and synchronization. Retain independent grants and the original dependent instance across condition exits. Source-loss invocation permission stays on the original frame, separately from ongoing source eligibility; recipient restriction conditions must not recurse through source-dependent skill queries.

- Remaining source migrations include projected lord skills, private skill libraries and Guandu's printed-grant qualification. Guandu retains the printed grant and evaluates the recipient's current effective faction as a separate condition. Awakening and limited skills still include source-specific eligibility and once-only tracking. Reuse shared skill mutations for their effects and preserve their distinct consumption boundaries.

## Applied state lifetimes

- Applied states outlive their caster and original skill. After application, evaluate the recipient's declared conditions, lifetime and removal rules; do not revoke a state merely because its caster died or lost the skill. Source qualification controls new applications and future windows; it does not requalify an already applied state. Read HP through the existing match health query, including its mode conversions and selected health receipts.

- Mou Hu Che Er's Kongwu uses its original paid origin to issue one equipment restriction through the target's next actual `TurnEnded` Before boundary. This lifetime skips the turn of issuance. The typed restriction suppresses both matching equipment skill grants and printed equipment effects, including cards with no skill grant. Field equipment and Wooden Ox read the same recipient restriction. The hand-size and HP qualification belongs to the original Play-phase settlement, and the draw penalty uses the original next-turn binding.

## Physical marks and hand limits

- Permanent physical card marks use `CardEntityMarkRuntime`, `markInitialHand` / `markCards`, and the shared `cardMarkCount` read; keep mark identity independent of holder, zone, source skill and presentation label. Holder-dependent benefits remain separate rules. Printed Lord qualification belongs to the actual grant as typed setup/program source data and uses the single `PrintedLordSkillQualifications` matcher.

### Hand-limit exemptions

- `MatchHandLimitQueries` combines declared numeric modifiers with registered contributions. Mobile Shi Guo Yuan's Xiugeng contributes the original permanent bonus for its recipient through `AppendNumericRuleContributions`. Counted hand entities, discard eligibility, and payment use their own shared queries.

- `IHandLimitCounting` supplies the counted physical Hand set for manual discard, automatic discard and refresh after partial payment. `HandLimitCountingRuntime` combines active policies, mode benefits and issued relations through the single `IHandLimitExemptions` matcher. Counting exemptions leave numeric `handLimit`, discard permission and payment obligations unchanged. Physical exemptions require a registered card and actual Hand residency for counting.
- Continuing `cardPolicies` use `handLimitCardKindExemption` with optional `handLimitMatch`. Evaluate current source effectiveness, role, declared conditions and the applicable mark, kind, current physical rank, effective suit or color, and survival requirements. Qualification can resume against the same permanent mark without another grant. A mark alone grants no benefit to another holder. Unknown color stays distinct from known suitlessness and black. Legacy kind matching accepts the printed kind and also accepts Alcohol as Slash when the existing identity policy allows it.
- `IgnoreTurnObtainedHandCardsForDiscard` exempts cards that entered that player's Hand after actual `TurnStarted`. Same-zone no-ops do not count as entries. A real reentry qualifies. These entry facts serve that policy and do not establish general acquisition or payment.
- `IHandLimitExemptions.Grant` uses the original typed outcome, actual gift or physical movement. Freeze the original frame, instruction, slot, source, recipient, entity or selector and lifetime. Reissuing a key keeps the original record, including a terminal one. Later skill loss or death of the issuer does not itself revoke the relation. Real generated cards use their original initialization authorization, source, hash and per-entity slot, including creation without a program frame.
- Membership and counting share entity, selector and lifetime facts. Counting additionally checks current Hand and any declared survival condition. Membership alone confers no card-use permission, face access or exemption for another holder. Use, Played, provider, rescue and counterspell qualification retain their own rules.

| Lifetime or producer | Rule-specific boundary |
| --- | --- |
| 心幽、周年披挂 | Freeze actual `(turnNumber, turnOwner)` coordinates. The same entity can regain the benefit in the original recipient's Hand within that lifetime. Coordinate matching is independent of whether `TurnEnded` has occurred. |
| 陶谦招祸 | Issue at actual self-gift commit or completion of another player's gift from Hand. Require setup completed, positive actual turn, matching progression turn, and absence of `TurnEnded` and `ActualTurnReturned`. Physical-use prohibition reads the same relation with its own zone scope. |
| 移动谋黄月英集智 | Use `NativeDrawOutcome.ActualDrawIds`. Keep the original pre-first-turn exception and require the matching progression turn, actual owner on the cursor and no actual return where applicable. Counting also requires the recipient alive. |
| 佐军 | Freeze the real Hand entry at keep selection for the bound entity drawn by the ordinary draw. Leaving Hand ends that entry's eligibility. The recipient's later `TurnEnded` Before ends the lifetime. The issuing turn's end leaves it active. Mark and use, response and conversion restrictions read the same relation. |
| 邀作、魔天恩、魔诡伏、happy 幻术 | Permanently bind the real entity to the original recipient. It can regain the benefit on return to that Hand. 诡伏 also requires a living recipient for counting. Own-mark conversion and global mark reads keep their original scopes. 幻术 generation, face changes, destruction on transfer to another holder and rewards stay with their entity owner. |
| 武庙雄幕 | Permanently bind actual acquisitions to the original recipient's Hand. Match the current physical rank of 8, including after a face change. |
| 周年德化 | After the real zero-entity use completes all names, permanently grant the printed damage-card kind benefit for current and future cards. Skill removal leaves it in force. Earlier name prohibitions and numeric limit contributions keep their own rules. |
| Exact repeated judgment, 移动谋甄姬 and direct kind grants | Keep public grant metadata and write entitlement to the canonical relation store. 甄姬 deduplicates the same entity in the same turn through the shared judgment relation. Other permanent exemptions do not qualify for that protocol. |

Frozen turn coordinates, explicit actual-turn status, permanent identity, a
specific Hand entry and `SkillStateLifetime.Until` have distinct expiry rules.
`IHandLimitExemptionNativeLifetimes` marks terminal relations at the original
native turn cleanup, next source-owner turn initialization or applicable owner
death cleanup. Coordinate-only relations retain their own lifetime. Expiry uses
the global turn-effect grant sequence and contributes its sequence to
`TurnCardUseEffectsExpiredEvent` at the original cleanup point. Public effect
projection exposes only the existing judgment-entity and kind-grant recipes.
Judgment labels read actual matching Hand entities, and the kind DTO reads the
original issue and spec. Private grants do not expose their entity counts.

The loader validates `grantHandLimitExemption` bindings, selectors, explicit
turn conditions and `until`. The issued relation freezes the actual bound set.
An empty bound set issues nothing. Capture demand for `until` uses shared
descriptor metadata and the lifetime mapping.
The remaining composition gaps are rule-specific eligibility, actual payment,
card generation and some numeric hand-limit contributions.

## Judgment attachments and generated equipment

- Judgment-area attachments use the shared attachment fact and physical movement lifetime. Slot occupancy is separate from delayed-card identity; direct judgment-area transfers preserve it, leaving ends it, and source loss does not revoke an issued attachment. Character code does not rebuild slot or payment proof ledgers.

- Generated equipment uses `EquipmentCardOrigin` and `EquipmentDestinationPolicy` through `GeneratedEquipmentRuntime`; random pile selection, private generation offers and native placement reuse the equipment composition nodes. Equipment's own entry/departure triggers declare `ownEquipmentOnly`; departing sources freeze before movement and reuse `ProgramInvocationAuthorization` on the owning movement window, never a replacement skill grant or character ledger.

- Use `offerGeneratedEquipment`, `selectRandomPileEquipment` and `placeBoundEquipment` for the Pu Yuan composition. Keep destruction on actual discard entry distinct from shop equipment destruction on leaving Equipment. `equipment.destroyedThisTurn` counts actual destruction batches through the movement journal. Random placement into an empty slot and actual equipment use keep their native qualification. Generic JSON placement for Judgment attachments remains a separate gap.
