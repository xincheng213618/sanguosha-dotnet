# 表达式条件

本页说明效果步骤的 `{"kind":"expression"}` 条件、整数 `amountExpr` 与冻结牌集合表达式，以及用它们替代人物专用操作的迁移方向。入口与其他条件见[技能配置、条件与顺序步骤](RUNTIME_CONFIGURATION.md)。

## 为什么需要

人物专用操作中有相当一部分只是“通用操作 + 一个专门的判断或数量”：例如“目标手牌少于体力上限时本人摸一张”“体力唯一最高时翻面”。过去每个这样的判断都新增一个 `SkillProgramEffectOp` 或 `SkillProgramNumberExpression` 枚举值，再写解析、组合校验、执行和返回凭据，一个武将常常因此多出数 KB 的 C#。

表达式条件把这类判断写回 JSON：执行器已经在提交步骤游标后、执行操作前统一检查效果的 `condition`，条件为假则跳过该步骤。所以凡是接受 `condition` 的操作，都可以直接使用表达式，不需要为每个操作增加代码。

## 写法

```json
{ "op": "draw", "target": "owner", "amount": 1,
  "condition": { "kind": "expression", "expr":
    ["and", ["alive", "selectedTarget"], ["<", ["handCount", "selectedTarget"], ["maxHp", "selectedTarget"]]] } }
```

`expr` 是 `[运算符, 参数...]` 形式的 JSON 数组，必须是布尔值。条件对象只接受 `kind` 与 `expr`。

| 类型 | 写法 |
| --- | --- |
| 布尔字面量 | `true`、`false` |
| 整数字面量 | -99 到 99 |
| 参与者 | `"owner"`、`"selectedTarget"`（恰好一名已选目标）、`"eventSource"`、`"eventTarget"`；量词内的 `"it"` |
| 参与者集合 | `"living"`（存活角色，座位顺序）、`"livingOthers"`（除拥有者外的存活角色） |
| 逻辑 | `["and", b, b, ...]`、`["or", b, b, ...]`（2–8 项）、`["not", b]` |
| 比较 | `["<" / "<=" / ">" / ">=" / "==" / "!=", i, i]`、`["same", p, p]` |
| 原始队伍 | `["sameTeam", p, p]` 读取实际 TeamId，包含自己；已知角色没有队伍则为假，参与者未知则为未知，不使用 AI 敌友估计 |
| 角色状态 | 布尔：`alive`、`hasHp`、`faceDown`、`chained`；整数：`hp`、`maxHp`、`lostHp`、`handCount`、`handLimit`、`equipmentCount` |
| 窗口冻结事实 | `["windowHp", "owner" / "eventTarget"]`、`["windowHandCount", "owner" / "eventTarget"]` 读取原触发窗口的体力或手牌数；不读取后来子结算改变的现值 |
| 运算 | `["+", i, ...]`、`["min", i, ...]`、`["max", i, ...]`（2–8 项）、`["-", i, i]`、`["floorDiv", i, i]`；`["when", b, i]` 在 b 为真时取 i，否则未知 |
| 绑定读取 | 整数：`["number", "name"]`、`["boundCount", "cards"]`、`["boundSuitCount", "cards"]`、`["boundDistinctSuitCount", "cards"]`、`["boundRank", "singleCard"]`；布尔：`["boundIsEquipment", "singleCard"]` |
| 物理牌标记 | `["cardMarkCount", p, "mark-id", "hand"]`，牌区也可为 `"equipment"`；按当前牌区读取永久实体标记 |
| 手牌印刷花色 | `["handPrintedSuitCount", p, "spade"]`，花色也可为 heart/club/diamond/none；只数当前手牌的实体印刷花色，不应用花色改写 |
| 条件授予事实 | `["ruleWounded", p]` 读规则受伤；`["equipmentSuitCount", p]` 读独立来源改写后的装备花色数；`["publicPileCount", p, "skill:id"]` 读本人该技能全部来源的公共堆实体数 |
| 量词 | `["all", set, b]`、`["any", set, b]` 返回布尔；`["count", set, b]` 返回整数；`b` 内用 `"it"` 表示当前成员 |

加载时检查运算符、参数个数、类型、字面量范围、嵌套深度（最多 12 层）以及 `"it"` 只能出现在角色集合谓词中。自然组合仍有有限深度，不能用无限展开代替机制边界。表达式随规则 JSON 规范化后进入技能玩法指纹，空白不影响指纹；新增该可选节点不改变规则格式版本。

## 求值语义

- 普通角色状态在步骤执行时读取当前公开状态；先完成的摸牌、移动及其子结算会影响后续步骤的判断。显式 `windowHp` / `windowHandCount` 使用原 `WindowContext.Facts`；缺少窗口/目标或体力未定义、尚未选择体力池时返回未知。AI 也只使用提供给它的同一冻结事实，没有窗口时不猜测。
- 三值逻辑：缺失的参与者（例如未选目标、事件没有来源）、未定义体力（`hp`、`lostHp`）以及未知集合都是“未知”。比较与运算遇到未知返回未知；`not` 保持未知；`and` 遇到确定的假即为假，`or` 遇到确定的真即为真；整个条件最终为未知时按假处理并跳过步骤。
- 因此需要“没有体力值的角色不参与比较”时要明确写出，例如 `["or", ["not", ["hasHp", "it"]], ["<", ["hp", "it"], ["hp", "owner"]]]`。
- 表达式只读，不支付、不移动、不发事件。拥有者死亡、失去技能实例和终局仍由执行器统一处理。
- AI 预估只知道拥有者与已选目标的公开状态；其他参与者和集合视为未知，所以依赖它们的步骤在估值中按不发动处理。
- `handLimit` 读取实际规则计算后的手牌上限；AI 只在公开上下文提供该值时使用。`floorDiv` 按数学意义向下取整，除零或商超出整数范围返回未知。未选定独立体力池而两个体力值不同时，`hp`、`lostHp` 也返回未知，不能默认选择其中一人；这与隐匿角色的 `hasHp=false` 不同。

## 适用范围

布尔表达式可用于效果步骤的 `condition`（包括 `all`/`any`/`not` 组合内）。`skillStates` 的 `when` / `while` 与 `skillState.match.filter` 使用同一求值器，加载器分别限制为该上下文能够提供的只读事实；它们不借用运行中的程序帧。其他入口是否接受表达式仍由各自加载边界决定。整数表达式还可以用于下述数量字段。

`skillGrants[].while` 也使用同一表达式，参与者限 owner、量词 it 及 living/livingOthers，不允许帧绑定、选项、事件参与者或签发窗口事实。条件成立且精确父来源有效时，唯一物化器授予叶技能；条件退出只移除该派生来源，再次成立保留原实例。`ruleWounded` 复用现有规则受伤权威，不能用 Hp<MaxHp 替代；`equipmentSuitCount` 在当前求值已合格的独立改写来源上计算，不回最终技能绑定查询。两者可能依赖技能来源，受体持续限制的 raw while 不接受它们，避免自递归；效果及条件授予可用。公共堆数是 raw 事实，已知空堆返回 0，缺少参与者或读取能力则未知，不按同名技能猜同一实例。

完整来源、调用续接策略和迁移范围见[条件授予的统一实现](CAPABILITY_MAP.md#条件授予的统一实现)。

## 按回合和时点读取真实用牌

触发器的 `condition` 可使用同一表达式。触发资格仍在原资格阶段判断；该入口不接受 `number`、选项或牌绑定读取，也不为求值创建程序帧。依赖历史的窗口读取原 `HistoryPoint`，由实际回合、拥有者和包含待提交事实的完整 journal 游标确定截点；游标不是事件序号。缺少窗口 point 时 `window` 与 `currentUse` 为未知；明确的 `now` 则读取当前实际回合，不借用一个假窗口。

| 写法 | 结果 |
|---|---|
| `["cardUses","window","latest"]` | 原窗口截点每次用牌的最新已知事实 |
| `["cardUses","now","observations"]` | 当前实际回合逐次采样的事实；后续取消不抹除旧观察 |
| `["cardUses","window","confirmations"]` | 原窗口截点以前每次真实原生目标确认的快照，按确认发生顺序；目标未改变也保留真实确认 |
| `["cardUses","window","accepted"]` | 原接受成员与顺序不变，actor／targets 读取截点时已知值；主要目标及 finalized／completed 状态取截至截点已达到的阶段 |
| `["cardUses","window","finalizations"]` | 按首次 finalized 发生顺序读取不可变快照，主要目标取当时的规范化结果；后来更新或完成不回写 |
| `"currentUse"` / `"use"` | 当前窗口用牌／事实集合谓词中的成员 |
| `["useActor",fact]` / `["useDeclarer",fact]` | 采样时使用者／首次接受者，均不等于材料提供者 |
| `["useTargets",fact]` | Latest／Observations 为完整 designated 目标；Confirmations 为原 TargetsConfirmed 的 raw 目标 |
| `["useConfirmedTargets",fact]` | 最近一次真实确认的原始完整目标；没有确认元数据则未知 |
| `["usePrimaryTargets",fact]` | 按视图读取规范化主要目标：latest／observations 是最近确认，accepted 是已达 finalized／completed 阶段，finalizations 是首次 finalized；借刀只取持刀者，区别于包含 victim 的 raw 目标 |
| `["useKindIs",fact,["slash","fireSlash","thunderSlash"]]` | 匹配原生声明时牌种，不读后来改写的 EffectiveKind；数组使用共同 ProgramCardKindMatch，完整旧杀族包含冰杀，单项 slash 仍仅普通杀 |
| `["usePhaseIs",fact,"play"]` | 最近一次真实确认发生时的实际 TurnPhase，不是当前阶段；无采样元数据则未知 |
| `["usePhaseIs",fact,"play","accepted"]` | 原接受时阶段；不代表当前仍在出牌阶段，也不代替确认时阶段 |
| `["useEstablished",fact]` / `["useDeclared",fact]` | 目标已在实际 finalized 时成立／已有真实原生声明身份；目标确认本身不等于 Established |
| `["useFinalized",fact]` / `["useCompleted",fact]` | 截点时已达到相应真实结算阶段；不把接受或确认当成完成 |
| `["useIsResponse",fact]` / `["useIsSelfNativeResponse",fact]` | 实际响应类型／既有自身原生响应协议 |
| `["atOrBefore",fact,"currentUse","accepted"]` | 双方有接受记录时按真实接受游标比较包含式截点；没有该元数据时保留真实长整数身份／声明顺序，不伪造 Action |
| `["first",facts]` / `["last",facts]` | 所选事实集的首项／末项；空集或未知集返回未知，不把接受、确认或完成顺序互换 |
| `["takeLast",facts,2]` | 取所选事实集的尾部两项，保持原顺序；数量可用整数表达式，已知非正数得到空集，未知数量仍未知 |
| `["samePlayPhase",fact,fact]` | 两项均在 Play 接受，且接受时实际回合、回合拥有者与 Play 实例相同；不隐含使用者相同 |
| `["sameUse",fact,fact]` | 双方有 native ID 时只比较该真实 ID；只有双方都没有 native ID 才比较真实 ActionId，单边缺失或没有可比身份时未知 |
| `["filter",set,predicate]` / `["targets",facts]` | 筛选事实或角色集合／按所选视图的 Targets 去重合并；不会隐式改成主要目标 |
| `["intersect",participants,participants]` | 角色集合交集，保持左侧顺序；任一集合未知时结果未知 |

`any`、`all`、`count` 同时支持事实与角色集合。事实谓词用 `use`，角色谓词用 `it`，两种迭代可嵌套，`first(filter(...))` 保留外层角色绑定；`contains` 判断角色是否在集合中。筛选谓词未知时集合仍未知，不能把未知成员当作不匹配跳过。`turnOwner` 是窗口或明确 now 读取的实际回合拥有者。各视图均支持 `window` 和 `now`，表达式自动声明历史采集需求，与当前是否持有技能分开。

Confirmations 每项是一份真实确认快照，后来更新不回写它。Latest／Observations 的 Targets 仍反映 designated 目标；其 `ConfirmedTargets`、`PrimaryTargets`、`Phase` 是最近确认留下的元数据，普通更新不擦除，也不能因此把更新当成新的确认。声明牌种同样独立于有效牌种，角色改写不制造另一条接受记录。

Accepted 与 Finalizations 从同一 journal 派生，不增加 Pair 表，也不改变上述视图含义。前者用于按接受顺序选择关系、再检查截点时是否已完成；后者用于按首次 finalized 顺序比较牌，不用后来完成目标覆盖当时快照。`samePlayPhase` 只回答同一接受阶段，使用者、首次接受者、响应类型与当前实际阶段须由规则各自组合。

完成窗口的 point 在真实 `ActualCardUseTargetsCompletedEvent` 处冻结；随后候选收集或嵌套子结算不能移动它。伺盗先在该截点选择合格 accepted 尾部两项，再要求两项 finalized 且 completed，最后交主要目标；不能先过滤未完成项后跨过去，也不要求尾项就是当前外层牌。联翩则在 finalizations 中取同阶段、同使用者的最后两项。两者的历史条件和 `targetSetExpr` 已能写入 JSON，真实复合支付与执行仍由原效果协议承担。

跨角色首次成为杀目标可由共同表达式组合，不需要人物历史查询：

```json
{"kind":"expression","expr":["sameUse",
  ["first",["filter",["cardUses","window","confirmations"],
    ["and",["useKindIs","use",["slash","fireSlash","thunderSlash"]],
      ["contains",["useConfirmedTargets","use"],"owner"]]]],
  "currentUse"]}
```

魅魂用首次 finalized 的原窗口 point 读取这一跨所有使用者的确认历史，不能加 actor==owner。刘琦屯江在实际资格时读取 `now/confirmations`，并以同一条件组合 `phaseCount` 的 Play entered/skipped 资格，使用 `evaluateConditionAtResolution:true` 保留求值时点。界 current 窃听则使用 pre-TurnEnd 的原窗口 point 和 raw 确认目标，不能接收结束后新增动作，也不能把借刀 victim 从目标域漏掉。真实确认与急袭的 finalized-established ever 关系是不同概念。

`selectTarget` / `selectTargets` 可用 `targetSetExpr` 提供角色集合，与原目标类型和实际合法域取交集；提交时仍走共同输入校验。已支付步骤需继续使用原帧冻结的集合，不随之后子结算的新事实刷新菜单。已迁移复合效果按各自加载合同要求 `targetSetExpr`，不能缺字段后静默回退为人物历史扫描；该格式要求与旧语法退役统一处理，不逐字段另升版本。表达式只读视图可在同一次求值内缓存，不承载 pending、支付或游标。

## 按回合和时点读取实际阶段

```json
["phaseCount","now","owner","play","entered"]
["phaseCount","window","eventTarget","all","skipped"]
```

`phaseCount` 返回非负整数。时点为 `now` 或原窗口的 `window`；参与者是阶段行动者，仍受该时点的实际回合及回合拥有者范围限制。阶段可为 `preparation`、`judgment`、`draw`、`play`、`discard`、`ending` 或 `all`，结果为 `entered` 或 `skipped`。缺少可用时点或参与者时返回未知；已有范围内没有对应事实才返回零。`phaseCount` 自动声明 `ActualDiscardRecoveryPhase` 身份事实需求，不因普通进入／跳过数量读取而请求阶段末观察窗口。

实际阶段捕获从定义需求编译：操作描述符区分 Triggered／Activated，通用 trait、被动声明、表达式、窗口和技能状态 Until 共同参与。`PhaseStarted` 期限和 `phaseCount` 需要阶段身份；`PhaseEnded` 期限与 `ActualPhaseEnded` 窗口声明阶段结束需求，结束需求经同一闭包包含身份需求。期限收集同时包含效果的 Until 与持续声明的 `State.Until`。

内容目录的 RequiredFacts 保持不可变；当前模式的 RequiredFacts 只在对局 provider 合成至同一只读 `INativeFactDemand`，不污染其他使用该目录的对局。未知模式需求或未知 mask 位明确拒绝。该声明机制无需规则 JSON 或存档格式升级。

需求声明不扩张执行协议：`ActualPhaseEnded` 仍用于特定已签发事项的单候选原生窗口，并不自动收集任意普通 observer。现有阶段末 collectors、迟到事项及 typed return 保留原归属。OwnedDraw 也不因模式的阶段 mask 额外打开 DrawSkipped 帧，共享实际 skip fallback 仍在原生跳过边界采样。

entered 只计真实阶段开始，恢复旧 token 不增加次数；skipped 只计原生实际跳过边界。自然槽、插入阶段和额外摸牌各保留原 occurrence，不把请求、替代声明或付款次数当成实际结果。原窗口以完整 journal 游标为截点，后续子阶段不会倒灌。

entered 与 skipped 不互斥，也不能用阶段结束次数替代 entered。原生弃牌开始窗已经进入后，技能才决定跳过弃牌阶段时，两项都计入；只有发出跳过请求、尚未到达真实跳过边界时仍不计 skipped。

例如，要求至少实际进入过一次出牌阶段，且本回合没有任何一次出牌阶段被跳过：

```json
["and",
  [">",["phaseCount","now","owner","play","entered"],0],
  ["==",["phaseCount","now","owner","play","skipped"],0]]
```

同回合有多次 Play 时，一次 skip 就使该条件不成立。平寇可直接用 `phaseCount(...,"all","skipped")` 作数量表达式并在原步骤冻结；蒙态只判断相应种类是否大于零。跳阶段作为技能成本时仍读原支付事实，不能拿上述实际次数替换。

## 整次获得的冻结牌事实

`Card` 是共同获得收据中的一张冻结牌事实，`CardSet` 是保留原顺序的事实集合；两者区别于真实用牌历史的 `Fact`／`FactSet`，也不等于当前手牌。读取使用原窗口 `MovementBatch.HandGains`，只开放当前程序或触发 owner 的收据。参与者不是 owner、缺少收据或缺少相应时点时返回未知，不能通过事件目标或迭代变量读取他人暗手；AI 没有这份私密收据时同样返回未知。

| 写法 | 结果 |
|---|---|
| `["gainCards","owner","incoming"]` | 整次物理到手集合；保留回到原手等与真正获得不同的情况 |
| `["gainCards","owner","previousHand"]` | 完成点排除全部 incoming 后冻结的原手牌，同次新牌不能互为旧手牌见证 |
| `["gainInitiallyAlive","owner"]` | 首次到达时是否存活，不重新读取当前存活状态 |
| `["gainActualTurn","owner"]` / `["gainRecipientAlive","owner"]` | 原获得收据的实际回合与完成点存活；与当前回合、当前存活及首次存活分别读取 |
| `["gainHasSource","owner","completion"]` | 原收据在 completion／initial 时点是否有当前候选或原程序来源；不代替当前发动资格 |
| `["gainFirstOutsideDraw","owner"]` | 收据中的首次 outside-Draw 事实；Processing 回到原手不消耗真正获得的首次 |
| `["gainPhaseIs","owner","draw","owner"]` | 冻结阶段为 Draw，且阶段执行者为指定参与者；阶段使用 TurnPhase 的 camelCase 名称，自己的额外 Draw 仍匹配 |
| `["cardActualGain","card"]` | 此实体是否真正获得，而非回到原手；不隐式过滤 incoming |
| `["cardKindIs","card",["slash","fireSlash","thunderSlash"]]` | 匹配冻结有效牌种，使用共同种类匹配；完整旧三杀兼容寒冰杀，单项 slash 仅普通杀 |
| `["cardSuitIs","card",["heart","diamond"]]` | 匹配冻结有效花色；可显式包含 none |
| `["sameCardKind",card,card]` / `["sameCardSuit",card,card]` | 精确比较两份冻结有效种类／花色；不把杀族合并为同种，已知 None 与 None 相等 |
| `["sameCardName",card,card]` | 比较冻结有效牌名，复用共享杀族归一；不重读当前牌面 |
| `["cardAt",card,"owner","hand"]` | 当前物理位置是否为该角色的 hand／equipment；不是冻结位置 |
| `["cardCanDiscard","owner",card]` | 接受阶段的本人当前弃牌资格；不证明后来已付款 |
| `["cardInBinding",card,"accepted"]` | 该实体 ID 是否属于原帧命名绑定；绑定须已生产 |

`all`／`any`／`count`／`filter` 同时支持 CardSet，双参数形式在谓词内使用默认 `card`。三参数形式 `[运算符, 集合, "变量名", 谓词]` 只用于牌集合，允许嵌套比较 incoming 与 previousHand：

```json
["filter", ["gainCards", "owner", "incoming"], "newCard",
  ["any", ["gainCards", "owner", "previousHand"], "oldCard",
    ["sameCardKind", "newCard", "oldCard"]]]
```

变量只在自己的谓词内有效，内层保留外层牌变量及已有 `it`／`use`。变量名最多 128 字符，以字母或下划线开头，其余为字母、数字、下划线或连字符；不能覆盖保留名、参与者、集合名或已在作用域内的牌变量。嵌套两个默认 `card` 也会被拒绝，应改用不同的显式名称。筛选谓词未知时整个集合未知，不把该成员静默排除；已知空集合仍是空集合。例子只表达冻结同种关系，不替代实际支付和展示返回的规则。

获得读取自动声明 `HandGainOccurrence` 需求，并由共同闭包包含整次摸牌识别所需的 `NativeDrawInvocation`；`gainPhaseIs`／`gainFirstOutsideDraw` 另声明 `MovementTiming`。读取不授权发动，首次来源、来源失去和死亡后的执行仍由原窗口授权处理。灵箜的获得资格使用这些普通条件，效果由 `captureCards` 接 `markCards` 组成；手牌上限收益与贤淑仍有独立实现。

## 摸牌调用事实的声明边界

`NativeDrawActualTurnOrdinalIs` 属于触发条件，由 Triggered 定义需求编译推导 `NativeDrawInvocation`；`CurrentTurnDrawPhaseDrawCountHalfFloor` trait 也声明同一事实。操作描述符保持 Triggered／Activated 区分，既有被动声明与选中模式需求在对局 provider 合成，原生 invoice 采集只读 `INativeFactDemand`，不枚举人物开关。

`NativeDrawInvocation` 不隐含阶段身份需求。读取 phase token 的操作或 trait 仍需声明 `ActualDiscardRecoveryPhase`；事实 mask 不能替代动态来源资格，也不把普通阶段需求变成额外 DrawSkipped 观察者帧。

初始发牌、零请求和无真实摸牌实体仍不产生已记录的调用；实际回合开始前只接受原有类型化开局来源。零张或合法替代可以完成普通 `draw` 的当前步骤，但不能据此伪造 DrawPile invoice。ordinal、threshold、展骥资格、原帧付讫状态与 typed return 保留其冻结边界。整次获得由共同 `HandGainOccurrence` 收据冻结到原批次，散文、知意、灵箜不再从摸牌完成点接收回调；灵箜获得标记已组合为普通节点，散文付款／展示续接已接入接受承诺、正在验证；知意实际复合效果仍有缺口，不能据此宣称整个摸牌执行链已纯 JSON。

## 已签发的用牌加伤

```json
{"op":"addCurrentCardDamage","target":"owner","targetScope":"currentTarget",
 "amount":1,"cardKinds":["slash","fireSlash","thunderSlash"],"damageLayer":"cardDamage",
 "condition":{"kind":"expression","expr":[">=",["windowHp","eventTarget"],["windowHp","owner"]]}}
```

`targetScope` 默认为 `use`，也可指定 `currentTarget`；后者按原窗口目标签发，不限制为杀。`amountExpr` 与 `amount` 互斥，复用既有整数求值和未知值处理。`cardKinds` 可限制实际伤害牌种，完整旧杀族的寒冰杀匹配仍用共同匹配函数；未指定则不限制。`damageLayer` 默认为 `additionalDamage`，`cardDamage` 同时参与牌伤害层计算。

该动作接在真实用牌的 committed、finalized 或 Slash-before-response 窗口；当前目标范围要求后两者。数值在步骤执行时确定，归原用牌帧；具体伤害结算读取共同调整，连环传播继承已确定的伤害，不再加一次。修改当前伤害仍使用原伤害操作。技能的资格、成本和后续惩罚分别保留在它们的原窗口。

旧 `addCurrentTargetSlashDamage` 操作已退役，四份内容改用上例的作用域、条件与层，不保留解析别名。取消抵消和追猎的动态升伤仍属于各自规则。

## 本次目标响应规则的实时条件

原帧 `ResponseRule.While` 复用同一表达式求值器，`eventSource` 是原用牌者，`eventTarget` 是被要求响应的角色。代供材料者与这两个身份分开，不用其手牌替代响应者手牌。该条件只读明确支持的公共角色事实；不能引用程序绑定、技能有效性或冻结窗口数据。

迫袭使用 `["<=",["handCount","eventTarget"],["handCount","eventSource"]]` 实时比较；敢决在真实发行时把装备印刷花色冻结为常量，例如 `["==",["handPrintedSuitCount","eventTarget","spade"],0]`。牌的花色改写不会改变这个印刷花色判断。没有手牌数据的估值 world 返回 unknown，不根据隐藏手牌猜测；原生效果 world 与响应 world 读同一个实体牌区。

目前这类已付签发仍由原生操作调用共用 issuer，尚未增加通用的已付响应效果 JSON 节点。持续技能状态/条件授予的 raw while 加载边界未开放这个新花色读取，不能把这些作用域混用。

## 永久物理牌标记

```json
{ "op": "markInitialHand", "target": "owner", "mark": "zheng" }
{ "op": "markCards", "bind": "shown-cards", "mark": "zheng" }
```

`markInitialHand` 只用于开局触发，读取真实开局母帧在任何开局子结算之前冻结的原始手牌；原候选已获签发后，来源消失或拥有者死亡不会把这批牌改成稍后的手牌。`markCards` 读取已有牌集合绑定，不重新选择、移动或支付。两者写同一种共享实体事实，重复标记不重复产生同一标记；初始牌的原主人是独立的来源信息，后续移动不覆盖它。

`["cardMarkCount","target","zheng","hand"]` 按当时受体的手牌计算；在持续状态里 `target` 是受体，效果表达式使用其支持的参与者名称。该读取也接受 `equipment`，离开牌区会改变计数，但不删除永久实体标记。标记本身不授予手牌上限、距离或其他收益，这些规则仍声明自己的有效技能及对象条件。

展示文件用 `cardMarks: {"zheng":"筝"}` 指定标签，同一标记的不同声明必须使用同一文字；加载器拒绝冲突。UI 只为原本允许该观察者看到的实体附加文字，标记 ID 与标签不混用。翻转标记、离区到期租约及尚未提交的批次不是永久标记，不能直接套用本节点。

## 数量表达式

`draw`、`recover`、`loseHp`、`revealTopCards` 接受可选的 `amountExpr`，与 `amount`、`numberExpression` 按字段是否出现互斥；`damage` 仍保持原协议。表达式必须返回整数，使用与条件相同的节点、类型、深度与只读事实。

```json
{ "op": "draw", "target": "owner",
  "amountExpr": ["-", ["handLimit", "owner"], ["handCount", "owner"]] }
```

在条件通过、步骤实际执行时求值；默认未知跳过这一步，负数按零处理。可显式设置 `"onUnknownAmount":"cancelBinding"`，复用整绑定取消与清理路径。取消型数量表达式的 `hp/lostHp` 依赖及 `captureNumber.valueExpr` 会接回原帧的独立体力池查询，不默认选择一人；默认跳步表达式仍把未确定的体力当作未知。`when(hasHp, ...)` 可保留旧规则要求的“有体力值才计算”，不改变 `maxHp` 这个读取本身。

后续步骤会重新读取当时的状态；同一个步骤的子结算恢复则使用首次签发的数量，不因手牌或体力已经变化而重新计算。`selectedTargets` 使用同一次求值的数量，仍逐个完成原生摸牌与观察窗口。零量没有牌或体力支付；带结果绑定的零量摸牌/翻牌仍生成空集合，供后续步骤读取和清理。

求得的非负数量保存在原 `ProgramSkillFrame.NumberBindings` 中，按指令游标签发，并由 `ProgramAmountEvaluatedEvent` 与通用帧不变量核对。已有回复、体力变化、濒死和摸牌返回继续验证该数量及原帧身份。表达式求值本身只读；签发数量属于步骤执行协议，不新增 checkpoint 字段或格式版本。

表达式摸牌带 `resultBind`、或表达式翻牌时，必须使用 `cancelBinding`：未知时终止整个绑定，避免后续读取不存在的集合；已知零则保持空集合。翻牌集合仍须通过原有移动节点完整消费，即使运行时可能为空。AI 数量估值复用同一表达式求值器，公开装备数量沿现有上下文读取，拥有者手牌估值随前序步骤更新；缺失事实仍未知。

## 冻结数值与牌集合

```json
{ "op": "captureNumber", "target": "owner", "resultBind": "before",
  "valueExpr": ["handCount", "owner"] }
```

`captureNumber` 把整数值保存到原帧的命名绑定；后续 `["number","before"]` 不会重算。值可为零或负数，不按操作数量截断；未知会取消整绑定。绑定只允许创建一次。原生复合操作也可通过同一捕获入口声明输出，例如伏枥的入口阵营数与伤害历史，不再保存第二份数值状态。

```json
{ "op": "captureCards", "target": "owner", "resultBind": "gained",
  "cardsExpr": ["filter", ["gainCards", "owner", "incoming"], ["cardActualGain", "card"]] }
{ "op": "markCards", "target": "owner", "bind": "gained", "mark": "lingkong" }
```

`captureCards` 将牌集合表达式结果的实体 ID 写入原帧的唯一命名绑定；表达式未知时取消整绑定，已知空集则正常绑定空集。该操作条件必须为 always，不支持可选公开可见性，结果固定 private；需要公开牌面时使用原有展示节点。捕获只建立观察引用，不选择、移动或支付，不把这些实体声明为稳定持有材料，也不取得 Processing 的清理权。过滤、选子集等派生绑定继承观察性质；同帧其他真正拥有的临时牌仍照原协议清理。卡牌随后移动不会改写收据中的冻结种类和花色，绑定后的其他操作仍遵守各自的实时材料校验。

观察引用不授予失源或死亡续接。原窗口已有相应授权时，纯捕获和实体标记可通过执行器的死亡目标门；支付、选择及未签发步骤不能借此取得额外权限。散文另经接受承诺固定展示后的付款范围，并读取实际移牌结果；不能将捕获集合的计划数量视作实际付款。

`chooseOption` 的全部选项都声明整数 `number` 时，后续 `["choiceNumber","选项绑定名"]` 读取已接受选项的冻结整数；加载器验证生产顺序，数值不能由回答命令提供。`offerGeneratedEquipment.amountExpr` 可复用该整数。`["number","equipment.destroyedThisTurn"]` 是共享实体来源事实，读取当前技能、拥有者在本回合产生装备的销毁批次数；它不创建第二个可变计数器。

牌集合读取必须引用前面已生产的绑定。`boundCount` 读取原绑定实体数，分配后也不是剩余数；花色读取优先使用生产者已冻结的花色列表，否则读取绑定实体的物理花色。`boundSuitCount` 排除 `Suit.None`，`boundDistinctSuitCount` 包含它，以保留需要区分两种口径的规则。`boundRank` 和 `boundIsEquipment` 要求一个实体；牌面读取只使用公开绑定或拥有者可知的原手牌/装备绑定，其他私有绑定未知，AI 不猜牌面。

数值与牌集合引用在加载时统一验证生产顺序及命名冲突；读取元数据不消费牌集合。选择、支付、移牌、随机及待决游标仍由各原生操作负责。

## 绑定后分配

```json
{ "op": "selectPublicPile", "target": "owner", "amount": 4,
  "skillIds": ["classic:bizhuan"], "resultBind": "pile" }
```

随后可用 `distributeBoundCards`，指定 `sourceBind`、`targetKind:"otherLiving"` 和普通表达式条件。先选定具体来源，再对该绑定判断门槛；不是在多个牌堆之间猜一个集合。来源、实体和花色有精确签发凭据。分配复用原公开牌堆游标与移动原因，条件只在发行分配时检查一次；已付子结算返回后继续剩余实体，不重新判断门槛或再次支付。该操作只消费原始 `selectPublicPile` 输出，不能输入任意手牌、Processing 集合或同一集合重复分配。

数量表达式首批迁移、删除项、指标及保留原因见[数量表达式迁移记录](AMOUNT_EXPRESSIONS_MIGRATION.md)。
后续公共窗口链、绑定及分配抽象的完成记录见[公共抽象记录](DSL_ABSTRACTION_COMPLETION.md)。

## 迁移样例

| 技能 | 之前 | 之后 |
| --- | --- | --- |
| 移动界荀彧·节命 | `drawMobileJiemingOwnerIfRecipientShort`，两个人物文件约 5.5 KB | `draw` + 上例表达式 |
| 移动界廖化·伏枥 | `turnOverBoundaryLiaoHuaIfUniqueHighestHp`，描述符与运行方法约 1.6 KB | `turnOver` + `["and", ["hasHp","owner"], ["all","livingOthers", ["or", ["not",["hasHp","it"]], ["<",["hp","it"],["hp","owner"]]]]]` |

伏枥的正反两种情况由 `--filter="program expression"` 的真实对局检查覆盖。

## 新增人物时的规则

1. 先查是否能用“通用操作 + 表达式条件”表达。只是判断“是否执行某个已有操作”的，不新增专用操作、`numberExpression` 或条件种类。
2. 表达式缺少某个只读事实时，优先在 `ProgramExpressions` 与 `GameEngine.ProgramExpressions.cs` 中补一个通用读取项（并补该项的行为检查），而不是为单个人物写操作。
3. 仍需专用操作的情况：需要新的选择交互、支付、移牌协议、随机或跨步骤状态。

## 后续阶段

按收益排序，后续逐步扩展同一套表达式，而不是另建体系：

1. **数量表达式**：`draw`、`recover`、`loseHp`、`revealTopCards` 已接入，包含结果绑定和原体力查询池的迁移。当前无内容引用且没有恢复依赖的旧枚举分支一并删除；仍有内容使用的兼容值保留。其他效果逐项核对，`damage` 尚未接入。
2. **绑定读取**：已支持牌集合数量、花色、牌点、装备类别与帧内冻结数值。其他只读事实按实际规则需要补充，复用已有签发与资源依赖检查。
3. **随机与循环**：本批公开堆逐张分配复用原游标与付款续接；其他受控随机选取和“按座位依次”循环仍需分别界定操作、输入与返回协议。
4. **跨版本参数化**：同名技能的不同版本（如归心、神赋、清俭）共用一个操作并以参数区分。

每个阶段先迁移少量代表人物并量化删除的专用代码，再推广。

## 本次用牌的响应规则

`issueCurrentResponseRule` 在原 `CardUseCommitted`、`CardUseTargetsFinalized` 或 `SlashBeforeResponse` 窗口给真实用牌签发一行规则；不重新发动源技能、不模拟一个用牌帧。最低响应数按 max 合并，`forbid` 按 OR 合并。

```json
{ "op": "issueCurrentResponseRule", "target": "owner", "minimum": 2,
  "actorRef": {"kind":"eventSource"}, "opponentRef": {"kind":"eventSource"},
  "incomingKinds": ["duel"], "requiredKinds": ["slash"],
  "condition": {"kind":"choiceIs","choiceBind":"response-kind","optionId":"duel"} }
```

`audience` 默认 `targets`，`all` 包含原使用者和非目标无懈候选。`actorRef`、`opponentRef`、`responderRef` 可冻结 owner/eventSource/eventTarget，后者只限制该角色；没有声明则不增加这项过滤。`incomingKinds`、`requiredKinds` 限制响应牌种对，`matchUseKind` 另要求本次原牌种类与 incoming 一致。`while` 复用响应规则公开事实的表达式子集，在实际查询时读逻辑响应者和原使用者，不把材料提供者当成响应者。

无前的原生额外目标步骤通过现有 ChoiceBindings 输出 slash/duel，完成该指令后由普通 JSON 条件签发实际一对；支付、原返回及已接受步骤的失源履约仍在原帧。当前 JSON host 需要真实 CardUse 上下文，未覆盖没有该上下文的直接决斗或反无懈链节点。`all` 也不会把原牌禁令传播到后继无懈的独立节点，濒死救援保持自己的边界。

## 实际移牌结果

普通同步移牌可将真正移动的实体写入一个新绑定，后续数量读取与原选择分开：

```json
{ "op":"moveBoundCards", "target":"owner", "sourceBind":"cost", "destination":"discardPile", "resultBind":"moved", "awaitMovementTriggers":true }
{ "op":"draw", "target":"owner", "amountExpr":["boundCount","moved"] }
```

`resultBind` 目前仅用于无条件的 discardPile／ownerHand／drawPileTop 路径，必须与输入、排除及其他绑定不同名。实际零张也产生空结果，`boundCount` 为零而不是未知。原输入绑定不变；全手牌选择等奖励继续读取原选择语义。

结果取本指令原生记录中的首次实际移动，按输入顺序冻结实体和真实目的地；OutsideGame 等标准目的地归一保留，后续再次移动不刷新结果。结果数是物理移动数，不自动等于“现在仍在目标牌区的数量”或某个技能的逻辑弃置数。

结果固定为 private 观察绑定，沿原帧恢复，不获得临时材料清理权或再次移动许可。绑定在已有移动观察者窗口之前写入，await 仍由原游标处理，续接不重新移动或再次支付。底部排序、指定目标赠牌和专用牌区的返回结果尚不支持此字段；接受材料的展示后履约按下述承诺协议执行。

## 接受材料后再履约

需要在展示或其他子窗之前接受材料时，先捕获集合，再明确签发原帧承诺：

```json
{ "op":"acceptBoundCardMovement", "target":"owner", "sourceBind":"candidates", "resultBind":"accepted", "destination":"discardPile",
  "continuation":{"sourceLoss":"retain","ownerDeath":"drain"} }
{ "op":"revealBoundCards", "target":"owner", "sourceBind":"witnesses" }
{ "op":"moveBoundCards", "target":"owner", "sourceBind":"accepted", "destination":"discardPile", "resultBind":"actual", "awaitMovementTriggers":true }
{ "op":"draw", "target":"owner", "amountExpr":["+",["boundCount","actual"],["boundCount","actual"]], "condition":{"kind":"expression","expr":["alive","owner"]} }
```

接受操作当前只支持本人可弃材料到 discardPile。`OwnDiscardQueries` 在接受时校验材料；接受集合为空则取消。后面的原目的地付款必须为单次、无条件步骤，不能把接受绑定转赠、筛分或重复消费。接受时冻结实体与来源；展示返回后仅过滤已离开原来源的实体，不重判见证、牌名或弃牌资格。真正移动的结果由 `actual` 绑定回答。

`sourceLoss` 可为 stop／retain，`ownerDeath` 可为 stop／drain，分别缺省 stop。retain 让本次已接受程序继续履约，不是重新授予技能。drain 排空已发出的原 post 窗口，只完成原承诺的付款，不新发摸牌、选择或其他步骤；原帧仍负责子返回与游标，观察绑定本身不创造这些权限。

散文的共享组合已接入原帧承诺，PublicHandReveal 使用共同窗口与原生返回；OwnDiscard 的旧事实桥接尚未退役。AI 只能消费已有授权范围内的事实，缺少私密收据或动态资格时保持未知，不能补猜暗手。

## 公开展示时签发的声明

真实公开展示可在原事件资格阶段签发后续调用：

```json
{
  "id":"first-public-heart", "window":"publicHandRevealed", "subject":"owner", "optional":false,
  "suits":["heart"], "usageScope":"round", "usageLimit":1, "namedUsageGroup":"first-public-heart",
  "invocation":{"source":"publicHandReveal","ownerDeath":"retain"},
  "effects":[{"op":"draw","target":"owner","amount":1,"awaitMovementTriggers":true}]
}
```

沿用 `suits` 的实际有效花色筛选与现有次数组，无须人物专属表达式。具名次数在原展示时占用，不含技能实例；开始已签发调用时不会再占用。失去再获得技能不清空本轮额度，真正的轮次边界才重置。未声明次数限制时不制造预留。

`ownerDeath` 与来源失效后的调用权限独立，默认 stop；retain 也不会启动已死者尚未开始的奖励。资格事实私存原帧，后续窗口数量读取仍来自原展示时点；它们不进入公共 claim 事件。此来源当前要求 mandatory owner，不支持 marker 付款、Event scope 预留或 `evaluateConditionAtResolution:true`，避免将已签发资格改为以后重新判断。

## 公开展示集合与原来源驻留

`["publicRevealCards","all"]` 返回每个已公开实体最后一份匹配的历史身份；scope 也可选 `actualTurn` 或 `actualPhase`，第三个参数缺省 `all`，可用 `explicit` 排除声明为材料披露的来源。先筛 scope／purpose，再去重实体。缺少实际阶段时读 actualPhase 为 unknown，不冒充空集合。集合只提供公开身份及原 Hand 驻留，不推测后来私区的牌面。

`["boundCards","drawn"]` 返回原程序绑定的只读实体与来源见证，加载器仍校验引用已经存在。`["cardStillAtSource","card"]` 要求实体还在原位置且原物理驻留未结束；`cardAt` 只判断当前位置，不能替代它。未捕获驻留时为 unknown，过滤、子集和再次捕获不得重新赋予一个当前 token。

```json
["==",["mod",["count",["publicRevealCards","all"],"shown",
  ["and",["cardAt","shown","owner","hand"],["cardStillAtSource","shown"]]],2],0]
```

`mod` 沿用整数余数语义，符号随被除数；零除数或未知操作数返回 unknown。零张已展示手牌的计数是零，因此上述偶数条件成立。真实摸牌结果可接：

```json
[
  {"op":"draw","target":"owner","amount":3,"resultBind":"drawn","awaitMovementTriggers":true},
  {"op":"captureCards","target":"owner","resultBind":"still-drawn",
   "cardsExpr":["filter",["boundCards","drawn"],["cardStillAtSource","card"]]},
  {"op":"revealBoundCards","target":"owner","sourceBind":"still-drawn",
   "condition":{"kind":"expression","expr":[">",["boundCount","still-drawn"],0]}}
]
```

结果绑定在真实生产时捕获原来源，排除后续子结算移走或离手重入的材料，也不包含后来另摸的牌。替代获得不作为 DrawPile 摸牌结果。历史集合和绑定集合缺少牌面时，cardKindIs、cardSuitIs、sameCardKind、sameCardSuit 等读取保持 unknown；AI 不借实体历史身份读取他人后来暗手里的具体位置或驻留。

实际回合后程序可声明 `"invocation":{"source":"afterTurnEnded"}`。它只在真实接受时保留原父候选的调用权限；失去来源可继续本次程序，死亡仍缺省停止新后缀。只曝光提示不取得授权，也不会给没有该声明的普通程序放宽续接。

## 真正声明的使用与原回合结束点

`["cardUses","turnEnd","actualUses"]` 读取原实际回合真正结束时的使用序列。ActualUses 包含真正原生声明及严格自有原生响应，按该源的位置排序；accepted-only 尝试不进入，Response 类型及无 Action 身份保持原义。Actor 是原声明／响应时的实际使用者，EffectiveKind 读取截至该点的真实更新。`turnEnd` 尚不存在时为 unknown，不能当作零次。

`window` 仍表示候选原窗口的截点，`now` 仍表示当前实际点；它们不会被 turnEnd 覆盖。回合后此前的使用可以进入 window，却不进入原 turnEnd。

这些是规则执行侧的原始事实，可能含尚未公开的声明目标。AI 的估值上下文不转交原始历史集合；缺少面向观察者的事实投影时保持 unknown，不能因为其他窗口读取已授权就开放全部历史。

`["takeFirst", factSet, 2]` 返回最前两条共同事实，负数按零处理；takeLast 保持原行为。`["useKindIs","use",["dodge"],"effective"]` 显式读取有效牌种，省略最后参数仍读原 DeclaredKind。对未原生声明的 Response，默认声明牌种仍可为 unknown，应根据实际响应身份选择 effective，而不是造一个声明。

例如按真正使用的前两张基本牌判定时，可先筛原生声明的基本牌和真实自有闪响应，再以 takeFirst 与 count／same 组合。不能把所有 Response 都排除或全部纳入，也不能把 useDeclarer 的 accepted 使用者当作原生声明时 Actor。

## 可选绑定给牌与原用牌窗口调用

`giveBoundCard` 可声明 `targetSetExpr`，在 `anyLiving`／`otherLiving` 的当前目标域内再筛选。提示和回答都使用原程序 world 求值；表达式未知时没有可猜测的收牌人。它只给原绑定中当前仍处于原来源区域的实体，空绑定直接结束，保留选项仍可跳过；实体离开后又回到同一区域依旧可给。需要锁定原驻留的规则应另用 `cardStillAtSource` 过滤。

可选的 `awaitMovementTriggers: true` 在真实给牌后排空原帧的移动子窗，复用既有 instruction delivery，不增加私有已付事件。回执保留已交付返回，不能授权未支付后缀。

真实用牌／响应触发可声明 `"invocation":{"source":"cardAction"}`。它在原窗口真正接受时绑定原父、候选索引和原程序；失源可完成本次，死亡缺省停止新步骤。它不是技能获得或持续有效性，也不授权后续候选。只需最终 Draw 的交付保留时，无须额外声明整次调用权限。

## 通用移牌的给予语义

`selectAndMoveOwnedCard` 和 `moveBoundCards` 可以声明 `"giveBy":"owner"`、`"giveBy":"selectedTarget"` 或 `"giveBy":"materialOwner"`，表示原技能所有者、已选角色或原材料共同持有人是逻辑给予者。只支持手牌目的地；省略时仍是普通移动。显式给予可以有非手牌来源，不能把普通获得／交换按同一物理路线自动提升为给予。

所选角色按既有参与者绑定校验；materialOwner 必须能从原材料得到唯一实际持有人，没有持有人的牌堆材料应明确声明真正的 giver。内建 `giveBoundCard` 等给予节点保留自己的逻辑给予者，不重复填写声明。

共享事实只记录完整请求中真正进入原收牌者手牌的实体，合法替代和销毁不会被数量表达式误算成给予。原生给予的通用 JSON 触发 occurrence 尚未声明化；当前两个消费者仍按原窗口读取共同事实，不新增人物专属读取语法。

`cardsMoved` 可用 `movementOccurrence: "perNativeGive"` 声明每次实际给予触发，`subject` 固定为 `owner`（逻辑给予者）。它不接受 sourceZones、destinationZones、牌面或原因等物理移动过滤；普通 perCard／perBatch 保持各自口径。接收者是 `eventTarget`，给予者是 `eventSource`，存活等资格由 expression 组合；声明本身自动要求共同给予事实，不需人物需求标记。候选仍留在原移动帧，一次给予多张牌只有一个逻辑 occurrence，完成的物理移动索引与它分开。

```json
{
  "id": "after-give", "window": "cardsMoved", "subject": "owner",
  "movementOccurrence": "perNativeGive", "optional": true,
  "condition": {"kind": "expression", "expr": ["alive", "eventTarget"]},
  "effects": [
    {"op": "selectTarget", "target": "owner", "targetKind": "eventTarget"},
    {"op": "draw", "target": "selectedTarget", "amount": 1}
  ]
}
```

CardsMoved／CardsGained 绑定可声明 `"invocation":{"source":"movement"}`，在实际接受后保留本次调用的来源权限；仅展示可选提示不授予权限。它绑定原移动窗口、候选游标及实际程序帧，不是技能授予，也不提前支付后续步骤。`ownerDeath` 缺省 `stop`；来源丢失和死亡不能互相替代。InitialHandGain 的原事实边界预签语义保持独立。

足额区域牌请求复用 `selectAndMoveOwnedCard`：固定 `count` 或 `countExpr` 二选一；新精确请求使用同一 chooserRef／cardOwnerRef、hand／equipment 与 Hand 目的地，要求 resultBind 和 awaitMovementTriggers。可用 exceptBind 排除已绑定实体，onInsufficient 默认 cancelBinding，bindEmpty 则在不足时返回空集合。结果是已经接受的选择，不是实际收到牌，不能以实际 Give 是否出现决定是否再次补偿。

```json
{ "op":"selectAndMoveOwnedCard", "target":"owner",
  "chooserRef":{"kind":"eventTarget"}, "cardOwnerRef":{"kind":"eventTarget"},
  "zones":["hand","equipment"], "destination":"ownerHand",
  "countExpr":["number","count"], "exceptBind":"forbidden",
  "onInsufficient":"bindEmpty", "resultBind":"reply",
  "awaitMovementTriggers":true, "giveBy":"materialOwner" }
```

`["windowAmount"]` 读取原窗口数量；`["givenCards","owner","eventTarget"]` 读取当前实际回合结束段内双方的已给予实体，可用 captureCards 冻结为排除集合。它保留实体与交付驻留，不猜牌面，非给予双方的私密读取返回未知。表达式自动声明共同给予事实需求。
