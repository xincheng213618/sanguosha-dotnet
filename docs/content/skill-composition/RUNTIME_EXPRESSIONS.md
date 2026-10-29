# 表达式条件

本页说明效果步骤的 `{"kind":"expression"}` 条件，以及用它替代人物专用操作的迁移方向。入口与其他条件见[技能配置、条件与顺序步骤](RUNTIME_CONFIGURATION.md)。

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
| 角色状态 | 布尔：`alive`、`hasHp`、`faceDown`、`chained`；整数：`hp`、`maxHp`、`lostHp`、`handCount`、`equipmentCount` |
| 运算 | `["+", i, ...]`、`["min", i, ...]`、`["max", i, ...]`（2–8 项）、`["-", i, i]` |
| 量词 | `["all", set, b]`、`["any", set, b]` 返回布尔；`["count", set, b]` 返回整数；`b` 内用 `"it"` 表示当前成员 |

加载时检查运算符、参数个数、类型、字面量范围、嵌套深度（8 层）以及 `"it"` 只能出现在量词谓词中。表达式随规则 JSON 规范化后进入技能玩法指纹，空白不影响指纹；新增该可选节点不改变规则格式版本。

## 求值语义

- 在步骤执行时读取当前公开状态，不冻结到发动时。先完成的摸牌、移动及其子结算会影响后续步骤的判断。
- 三值逻辑：缺失的参与者（例如未选目标、事件没有来源）、未定义体力（`hp`、`lostHp`）以及未知集合都是“未知”。比较与运算遇到未知返回未知；`not` 保持未知；`and` 遇到确定的假即为假，`or` 遇到确定的真即为真；整个条件最终为未知时按假处理并跳过步骤。
- 因此需要“没有体力值的角色不参与比较”时要明确写出，例如 `["or", ["not", ["hasHp", "it"]], ["<", ["hp", "it"], ["hp", "owner"]]]`。
- 表达式只读，不支付、不移动、不发事件。拥有者死亡、失去技能实例和终局仍由执行器统一处理。
- AI 预估只知道拥有者与已选目标的公开状态；其他参与者和集合视为未知，所以依赖它们的步骤在估值中按不发动处理。

## 适用范围

只允许出现在效果步骤的 `condition` 中（包括 `all`/`any`/`not` 组合内）。主动入口、选项、触发器和持续规则的条件在加载时拒绝表达式；这些位置没有运行中的程序帧。

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

1. **数量表达式**：允许 `draw`、`recover`、`loseHp`、`damage` 等以表达式给出数量，替代 `SkillProgramNumberExpression` 中 `FourMinusHandCount`、`OwnerLostHpPlusOne` 一类逐项枚举。现有枚举保留兼容，新内容不再追加。
2. **绑定读取**：表达式读取已有牌集合与选择结果（数量、花色、类别），覆盖 `record…`/`settle…` 中大量只读判断。
3. **随机与循环**：受控的随机选取和“按座位依次”循环，循环游标保存在原程序帧，覆盖逐张随机分配一类专用状态机。
4. **跨版本参数化**：同名技能的不同版本（如归心、神赋、清俭）共用一个操作并以参数区分。

每个阶段先迁移少量代表人物并量化删除的专用代码，再推广。
